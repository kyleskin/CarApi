using Microsoft.EntityFrameworkCore;

namespace CarApi;

public class CarDb : DbContext
{
    public CarDb() { }
    public CarDb(DbContextOptions<CarDb> options)
        : base(options) { }

    public DbSet<Car> Cars => Set<Car>();
}
