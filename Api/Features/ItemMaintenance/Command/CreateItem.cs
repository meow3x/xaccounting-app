using Api.Database;
using Api.Entities;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.ItemMaintenance.Command;

public record CreateItemCommand(
    string Code,
    string Name,
    int UnitOfMeasurementId,
    decimal UnitPrice,
    decimal UnitCost,
    int CategoryId,
    decimal WholeSale = 0,
    decimal Reorder = 0) : IRequest<Result<Item>> { }

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(e => e.Code).MinimumLength(3);
        RuleFor(e => e.Name).MinimumLength(3);
        RuleFor(e => e.UnitOfMeasurementId).NotEmpty();
        RuleFor(e => e.UnitPrice).GreaterThanOrEqualTo(0);
        RuleFor(e => e.UnitCost).GreaterThanOrEqualTo(0);
        RuleFor(e => e.CategoryId).NotEmpty();
        RuleFor(e => e.WholeSale).GreaterThanOrEqualTo(0);
        RuleFor(e => e.Reorder).GreaterThanOrEqualTo(0);
    }
}

internal class CreateItemHandler : IRequestHandler<CreateItemCommand, Result<Item>>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateItemHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Item>> Handle(CreateItemCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateItemCommandValidator();
        var validation = validator.Validate(command);

        var uom = _dbContext.UnitOfMeasurements.Find(command.UnitOfMeasurementId);
        var category = _dbContext.ItemCategories.Find(command.CategoryId);
        bool codeExisting = _dbContext.Items.Any(e => e.Code == command.Code);

        if (uom is null) { validation.Errors.Add(new(nameof(command.UnitOfMeasurementId), "UoM not found")); }
        if (category is null) { validation.Errors.Add(new(nameof(command.CategoryId), "Category not found")); }
        if (codeExisting) { validation.Errors.Add(new(nameof(command.Code), "Code already exists"));  }

        if (!validation.IsValid) { return Result<Item>.Invalid(validation.AsErrors()); }

        // Create item
        var item = new Item
        {
            Code = command.Code,
            Name = command.Name,
            Uom = uom!,
            UnitPrice = command.UnitPrice,
            UnitCost = command.UnitCost,
            Category = category!,
            Wholesale = command.WholeSale,
            Reorder = command.Reorder
        };

        _dbContext.Items.Add(item);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Item>.Success(item);
    }
}