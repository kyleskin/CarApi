using Microsoft.EntityFrameworkCore;

namespace CarApi;

public class CarDb : DbContext
{
    public CarDb() { }
    public CarDb(DbContextOptions<CarDb> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql()
                      .UseSnakeCaseNamingConvention();

    public DbSet<Car> Cars => Set<Car>();
}
