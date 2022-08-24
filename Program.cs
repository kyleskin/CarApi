using CarApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CarDb>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

async Task<List<Car>> SearchWithFilters(CarDb db, string? make, string? model, string? color, int? year) =>
    await db.Cars.Where(c =>
                        (make == null || c.Make == make)
                        && (model == null || c.Model == model)
                        && (color == null || c.Color == color)
                        && (year == null || c.Year == year)).ToListAsync<Car>();

app.MapGet("/cars", async (CarDb db,
                           [FromBody] CarSearchParams searchParams) =>
    await SearchWithFilters(db, searchParams.Make, searchParams.Model, searchParams.Color, searchParams.Year))
    .Produces<List<Car>>(StatusCodes.Status200OK)
    .WithTags("Getters");

app.MapGet("/cars/", async (CarDb db,
                          string? make,
                          string? model,
                          string? color,
                          int? year) =>
    await SearchWithFilters(db, make, model, color, year))
    .Produces<List<Car>>(StatusCodes.Status200OK)
    .WithTags("Getters");


app.MapGet("/cars/{id}", async (Guid id, CarDb db) =>
    await db.Cars.FindAsync(id)
     is Car car
        ? Results.Ok(car)
        : Results.NotFound())
    .Produces<Car>(StatusCodes.Status200OK)
    .WithTags("Getters");

app.MapPost("/cars", async (Car car, CarDb db) =>
{
    db.Cars.Add(car);
    await db.SaveChangesAsync();

    return Results.Created($"/cars/{car.Id}", car);
})
    .Produces<Car>(StatusCodes.Status201Created)
    .WithTags("Post");

app.MapPut("/cars/{id}", async (Guid id, Car inputCar, CarDb db) =>
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

app.Run();

