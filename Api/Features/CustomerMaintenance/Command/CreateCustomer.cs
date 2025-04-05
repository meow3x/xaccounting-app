using Api.Database;
using Api.Entities;
using Ardalis.Result;
using FluentValidation.Results;
using FluentValidation;
using MediatR;
using Ardalis.Result.FluentValidation;
using Api.Features.Shared.Dto;

namespace Api.Features.CustomerMaintenance.Command;

public record CreateCustomerCommand(
    string CustomerId,
    string Name,
    AddressDto Address,
    string? Tin,
    decimal? Discount,
    decimal? CreditLimit,
    int? PaymentTermId) : IRequest<Result<Customer>>;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(e => e.CustomerId).MinimumLength(3);
        RuleFor(e => e.Name).MinimumLength(3);
        RuleFor(e => e.Address).SetValidator(new AddressDtoValidator());
        RuleFor(e => e.Tin).MinimumLength(3).When(e => !string.IsNullOrEmpty(e.Tin));
        RuleFor(e => e.Discount).GreaterThanOrEqualTo(0).When(e => e.Discount != null);
        RuleFor(e => e.CreditLimit).GreaterThan(0).When(e => e.CreditLimit != null);
        RuleFor(e => e.PaymentTermId).GreaterThan(0).When(e => e.PaymentTermId != null);
    }
}

internal sealed class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<Customer>>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateCustomerCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Customer>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var validation = (new CreateCustomerCommandValidator()).Validate(request);

        // Extra validation
        PaymentTerm? paymentTerm = null;
        if (request.PaymentTermId.HasValue)
        {
            paymentTerm = _dbContext.PaymentTerms.Find(request.PaymentTermId);
            if (paymentTerm == null) { validation.Errors.Add(new ValidationFailure(nameof(request.PaymentTermId), "Payment term not found")); }
        }

        bool supplierIdExisting = _dbContext.Customers.Any(e => e.CustomerId == request.CustomerId);
        if (supplierIdExisting) { validation.Errors.Add(new ValidationFailure(nameof(request.CustomerId), "Customer Id already exists")); }

        if (!validation.IsValid) { return Result<Customer>.Invalid(validation.AsErrors()); }

        var customer = new Customer
        {
            CustomerId = request.CustomerId,
            Name = request.Name,
            Address = new Address(
                request.Address.Street,
                request.Address.City,
                request.Address.Province,
                request.Address.LandlineNumber,
                request.Address.MobileNumber
            ),
            Tin = request.Tin,
            Discount = request.Discount,
            CreditLimit = request.CreditLimit,
            PaymentTerm = paymentTerm
        };

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Customer>.Success(customer);
    }
}