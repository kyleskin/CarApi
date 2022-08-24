using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarApi;

public class Car
{
    public Guid Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Vin { get; set; }
    public string Color { get; set; }
}
