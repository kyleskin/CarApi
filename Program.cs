using CarApi;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<CarDb>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddCors();


var app = builder.Build();
// app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyHeader();
    options.AllowAnyMethod();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

async Task<List<Car>> SearchWithFilters(CarDb db, string? make, string? model, string? color, int? year) =>
    await db.Cars.Where(c =>
                        (make == null || c.Make.ToLower() == make.ToLower())
                        && (model == null || c.Model.ToLower() == model.ToLower())
                        && (color == null || c.Color.ToLower() == color.ToLower())
                        && (year == null || c.Year == year)).ToListAsync<Car>();

app.MapGet("api/cars", async (CarDb db, [FromBody] CarSearchParams searchParams) =>
    await SearchWithFilters(db, searchParams.Make, searchParams.Model, searchParams.Color, searchParams.Year))
    .Produces<List<Car>>(StatusCodes.Status200OK)
    .WithTags("Getters");

app.MapGet("api/cars/", async (CarDb db,
                          string? make,
                          string? model,
                          string? color,
                          int? year) =>
    await SearchWithFilters(db, make, model, color, year))
    .Produces<List<Car>>(StatusCodes.Status200OK)
    .WithTags("Getters");


app.MapGet("api/cars/{id}", async (Guid id, CarDb db) =>
    await db.Cars.FindAsync(id)
     is Car car
        ? Results.Ok(car)
        : Results.NotFound())
    .Produces<Car>(StatusCodes.Status200OK)
    .WithTags("Getters");

app.MapPost("api/cars", async (Car car, CarDb db) =>
{
    db.Cars.Add(car);
    await db.SaveChangesAsync();

    return Results.Created($"/cars/{car.Id}", car);
})
    .Produces<Car>(StatusCodes.Status201Created)
    .WithTags("Post");

app.MapPut("api/cars/{id}", async (Guid id, Car inputCar, CarDb db) =>
{
    var car = await db.Cars.FindAsync(id);
    if (car is null) return Results.NotFound();

    car.Make = inputCar.Make;
    car.Model = inputCar.Model;
    car.Year = inputCar.Year;
    car.Vin = inputCar.Vin;
    car.Color = inputCar.Color;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("api/cars/{id}", async (Guid id, CarDb db) =>
{
    if (await db.Cars.FindAsync(id) is Car car)
    {
        db.Cars.Remove(car);
        await db.SaveChangesAsync();
        return Results.Ok();
    }

    return Results.NotFound();
});

app.Run();

