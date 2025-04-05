using Api.Database;
using Api.Entities;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Api.Features.ItemMaintenance.Command;

public record UpdateItemCommand(
    [property: JsonIgnore] int Id,
    string Code,
    string Name,
    int UnitOfMeasurementId,
    decimal UnitPrice,
    decimal UnitCost,
    int CategoryId /*,
    decimal Wholesale,
    decimal Reorder */ ) : IRequest<Result<Item>> {}
public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(e => e.Code).MinimumLength(3);
        RuleFor(e => e.Name).MinimumLength(3);
        RuleFor(e => e.UnitOfMeasurementId).NotEmpty();
        RuleFor(e => e.UnitPrice).GreaterThanOrEqualTo(0);
        RuleFor(e => e.UnitCost).GreaterThanOrEqualTo(0);
        RuleFor(e => e.CategoryId).NotEmpty();
        //RuleFor(e => e.WholeSale).GreaterThanOrEqualTo(0);
        //RuleFor(e => e.Reorder).GreaterThanOrEqualTo(0);
    }
}

internal sealed class UpdateItemHandler : IRequestHandler<UpdateItemCommand, Result<Item>>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateItemHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Item>> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
    {
        var validation = new UpdateItemCommandValidator().Validate(command);
        var item = _dbContext.Items
            .Include(e => e.Uom)
            .Include(e => e.Category)
            .SingleOrDefault(e => e.Id == command.Id);
        var uom = _dbContext.UnitOfMeasurements.Find(command.UnitOfMeasurementId);
        var category = _dbContext.ItemCategories.Find(command.CategoryId);
        bool duplicateCode = _dbContext.Items.Any(e => e.Id != command.Id && e.Code == command.Code);

        if (item is null) { validation.Errors.Add(new ValidationFailure(nameof(command.Id), "Item not found")); }
        if (uom is null) { validation.Errors.Add(new(nameof(command.UnitOfMeasurementId), "Uom not found")); }
        if (category is null) { validation.Errors.Add(new(nameof(command.CategoryId), "Category not found")); }
        if (duplicateCode) { validation.Errors.Add(new(nameof(command.Code), "Code already exists"));  }

        if (!validation.IsValid) { return Result<Item>.Invalid(validation.AsErrors()); }

        item.Code = command.Code;
        item.Name = command.Name;
        item.Uom = uom;
        item.UnitPrice = command.UnitPrice;
        item.UnitCost = command.UnitCost;
        item.Category = category;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Item>.Success(item);
    }
}