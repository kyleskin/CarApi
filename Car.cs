using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarApi;

public class Car
{
    public int Id { get; set; }
    [Required]
    public string Make { get; set; } = string.Empty;
    [Required]
    public string Model { get; set; } = string.Empty;
    [Required]
    public int Year { get; set; }
    [Required]
    public string Vin { get; set; } = string.Empty;
    [Required]
    public string Color { get; set; } = string.Empty;
}
