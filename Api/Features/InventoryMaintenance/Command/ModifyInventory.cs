using Api.Database;
using Api.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.InventoryMaintenance.Command;

public record ModifyStockCommand(
    int ItemId,
    int Quantity) : IRequest<InventoryLog>; // positive if addition, negative if deletion

public class ModifyStockCommandHandler : IRequestHandler<ModifyStockCommand, InventoryLog>
{
    private readonly ApplicationDbContext _dbContext;

    public ModifyStockCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<InventoryLog> Handle(ModifyStockCommand request, CancellationToken cancellationToken)
    {
        var inventory = await _dbContext.Inventory
            .Include(e => e.Logs)
            .SingleOrDefaultAsync(
                e => e.Item.Id == request.ItemId,
                cancellationToken
            );
        if (inventory == null)
        {
            var item = await _dbContext.Items.FindAsync([request.ItemId], cancellationToken);

            // FIXME: Possible race condition (two POs referring to the same item is submitted, but no inventory record yet)
            inventory = new Inventory
            {
                Item = item!, // FIXME!
                Stock = 0 //lineItem.Quantity
            };

            await _dbContext.Inventory.AddAsync(inventory, cancellationToken);
        }

        inventory.Logs.Add(new InventoryLog
        {
            Inventory = inventory,
            LineItem = null,
            StockBefore = inventory.Stock,
            Quantity = request.Quantity,
            StockAfter = inventory.Stock + request.Quantity // Fixme: race condition
        });
        inventory.Stock += request.Quantity; // FIXME: race condtition
        _dbContext.Entry(inventory).State = EntityState.Modified; // Make sure modifications are detected, in case this is a new inventory entry

        await _dbContext.SaveChangesAsync(cancellationToken);
        return inventory.Logs.Last();
    }
}
