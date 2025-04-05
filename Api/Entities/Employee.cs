using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Entities;

[JsonConverter(typeof(JsonStringEnumConverter<SalaryUnit>))]
public enum SalaryUnit
{
    Hourly,
    Weekly,
    Monthly,
    Annual
}

[Index(nameof(EmployeeId), IsUnique = true)]
public class Employee : BaseEntity
{
    [MaxLength(25)]
    public required string EmployeeId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? MiddleName { get; set; }
    public required Address Address { get; set; }
    public string? Tin { get; set; }
    public string? PagIbigId { get; set; }
    public string? PhilhealthId { get; set; }
    public decimal? Rate { get; set; }
    public SalaryUnit SalaryUnit { get; set; }
}
