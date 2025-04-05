using Api.Database;
using Api.Entities;
using Api.Features.Shared.Dto;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Api.Features.SupplierMaintenance.Command;

public record CreateSupplierCommand(
    string SupplierId,
    string Name,
    AddressDto Address,
    string? Tin,
    decimal? Discount,
    decimal? CreditLimit,
    int? PaymentTermId) : IRequest<Result<Supplier>> { }

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(e => e.SupplierId).MinimumLength(3);
        RuleFor(e => e.Name).MinimumLength(3);
        RuleFor(e => e.Address).SetValidator(new AddressDtoValidator());
        RuleFor(e => e.Tin).MinimumLength(3).When(e => !string.IsNullOrEmpty(e.Tin));
        RuleFor(e => e.Discount).GreaterThanOrEqualTo(0).When(e => e.Discount != null);
        RuleFor(e => e.CreditLimit).GreaterThan(0).When(e => e.CreditLimit != null);
        RuleFor(e => e.PaymentTermId).GreaterThan(0).When(e => e.PaymentTermId != null);
    }
}

internal sealed class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, Result<Supplier>>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateSupplierCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Supplier>> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var validation = (new CreateSupplierCommandValidator()).Validate(request);

        // Extra validation
        PaymentTerm? paymentTerm = null;
        if (request.PaymentTermId.HasValue)
        {
            paymentTerm = _dbContext.PaymentTerms.Find(request.PaymentTermId);
            if (paymentTerm == null) { validation.Errors.Add(new ValidationFailure(nameof(request.PaymentTermId), "Payment term not found")); }
        }

        bool supplierIdExisting = _dbContext.Suppliers.Any(e => e.SupplierId == request.SupplierId);
        if (supplierIdExisting) { validation.Errors.Add(new ValidationFailure(nameof(request.SupplierId), "Supplier Id already exists")); }

        if (!validation.IsValid) { return Result<Supplier>.Invalid(validation.AsErrors()); }

        var supplier = new Supplier
        {
            SupplierId = request.SupplierId,
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

        _dbContext.Suppliers.Add(supplier);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Supplier>.Success(supplier);
    }
}