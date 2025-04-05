using Api.Database;
using Api.Entities;
using Api.Features.Shared.Dto;
using Ardalis.Result;
using FluentValidation.Results;
using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Ardalis.Result.FluentValidation;

namespace Api.Features.CustomerMaintenance.Command;

public record UpdateCustomerCommand(
    [property: JsonIgnore] int Id,
    string CustomerId,
    string Name,
    AddressDto Address,
    string? Tin,
    decimal? Discount,
    decimal? CreditLimit,
    int? PaymentTermId) : IRequest<Result<Customer>>
{ }

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(e => e.CustomerId).MinimumLength(3);
        RuleFor(e => e.Name).MinimumLength(3);
        RuleFor(e => e.Address).SetValidator(new AddressDtoValidator());
        RuleFor(e => e.Tin).MinimumLength(3).When(e => !string.IsNullOrEmpty(e.Tin));
        RuleFor(e => e.Discount).GreaterThanOrEqualTo(0).When(e => e.Discount != null);
        RuleFor(e => e.CreditLimit).GreaterThan(0).When(e => e.CreditLimit != null);
        RuleFor(e => e.PaymentTermId).GreaterThan(0).When(e => e.PaymentTermId != null);
    }
}
internal sealed class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<Customer>>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateCustomerCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Customer>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var validation = new UpdateCustomerCommandValidator().Validate(command);

        PaymentTerm? paymentTerm = null;
        if (command.PaymentTermId.HasValue)
        {
            paymentTerm = await _dbContext.PaymentTerms.FindAsync(command.PaymentTermId, cancellationToken);
            if (paymentTerm == null) { validation.Errors.Add(new ValidationFailure(nameof(command.PaymentTermId), "Payment term not found")); }
        }

        var customer = await _dbContext.Customers
            .Include(e => e.PaymentTerm)
            .SingleOrDefaultAsync(e => e.Id == command.Id, cancellationToken);
        if (customer is null) { validation.Errors.Add(new ValidationFailure(nameof(command.Id), "Customer not found")); }

        if (!validation.IsValid) { return Result<Customer>.Invalid(validation.AsErrors()); }

        customer.CustomerId = command.CustomerId;
        customer.Name = command.Name;
        customer.Address = new Address(
            Street: command.Address.Street,
            City: command.Address.City,
            Province: command.Address.Province,
            LandlineNumber: command.Address.LandlineNumber,
            MobileNumber: command.Address.MobileNumber
        );
        customer.Tin = command.Tin;
        customer.Discount = command.Discount;
        customer.CreditLimit = command.CreditLimit;
        customer.PaymentTerm = paymentTerm;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return customer;
    }
}