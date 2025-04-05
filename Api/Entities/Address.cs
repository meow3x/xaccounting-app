using Microsoft.EntityFrameworkCore;

namespace Api.Entities;

[Owned]
public record Address(
    string? Street,
    string? City,
    string? Province,
    string? LandlineNumber,
    string? MobileNumber)
{ }