using FluentValidation;

namespace Api.Features.Shared.Dto;

public record AddressDto(
    string? Street, // Fixme: Move to dedicated class
    string? City,
    string? Province,
    string? LandlineNumber,
    string? MobileNumber);

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(e => e.Street).MinimumLength(3).When(e => e != null);
        RuleFor(e => e.City).MinimumLength(3).When(e => e != null);
        RuleFor(e => e.Province).MinimumLength(3).When(e => e != null);
        RuleFor(e => e.LandlineNumber).MinimumLength(3).When(e => e != null);
        RuleFor(e => e.MobileNumber).MinimumLength(3).When(e => e != null);
    }
}