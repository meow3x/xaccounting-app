using Api.Database;
using Api.Entities;
using Api.Features.Shared.Dto;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Api.Features.SupplierMaintenance.Command;

public record UpdateSupplierCommand(
    [property: JsonIgnore] int Id,
    string SupplierId,
    string Name,
    AddressDto Address,
    string? Tin,
    decimal? Discount,
    decimal? CreditLimit,
    int? PaymentTermId) : IRequest<Result<Supplier>>
{ }

public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
{
    public UpdateSupplierCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(e => e.SupplierId).MinimumLength(3);
        RuleFor(e => e.Name).MinimumLength(3);
        RuleFor(e => e.Address).SetValidator(new AddressDtoValidator());
        RuleFor(e => e.Tin).MinimumLength(3).When(e => !string.IsNullOrEmpty(e.Tin));
        RuleFor(e => e.Discount).GreaterThanOrEqualTo(0).When(e => e.Discount != null);
        RuleFor(e => e.CreditLimit).GreaterThan(0).When(e => e.CreditLimit != null);
        RuleFor(e => e.PaymentTermId).GreaterThan(0).When(e => e.PaymentTermId != null);
    }
}
internal sealed class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand, Result<Supplier>>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateSupplierCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Supplier>> Handle(UpdateSupplierCommand command, CancellationToken cancellationToken)
    {
        var validation = new UpdateSupplierCommandValidator().Validate(command);

        PaymentTerm? paymentTerm = null;
        if (command.PaymentTermId.HasValue)
        {
            paymentTerm = await _dbContext.PaymentTerms.FindAsync(command.PaymentTermId, cancellationToken);
            if (paymentTerm == null) { validation.Errors.Add(new ValidationFailure(nameof(command.PaymentTermId), "Payment term not found")); }
        }

        var supplier = await _dbContext.Suppliers
            .Include(e => e.PaymentTerm)
            .SingleOrDefaultAsync(e => e.Id == command.Id, cancellationToken);
        if (supplier is null) { validation.Errors.Add(new ValidationFailure(nameof(command.Id), "Supplier not found"));  }
        
        if (!validation.IsValid) { return Result<Supplier>.Invalid(validation.AsErrors()); }

        supplier.SupplierId = command.SupplierId;
        supplier.Name = command.Name;
        supplier.Address = new Address(
            Street: command.Address.Street,
            City: command.Address.City,
            Province: command.Address.Province,
            LandlineNumber: command.Address.LandlineNumber,
            MobileNumber: command.Address.MobileNumber
        );
        supplier.Tin = command.Tin;
        supplier.Discount = command.Discount;
        supplier.CreditLimit = command.CreditLimit;
        supplier.PaymentTerm = paymentTerm;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return supplier;
    }
}