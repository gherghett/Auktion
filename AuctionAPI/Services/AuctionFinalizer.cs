using AuctionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Services;

public class AuctionFinalizer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public AuctionFinalizer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    private void FinalizeEndedAuctions(AuctionContext context)
    {
        var auctionsWithSales = context.Auctions.Where(a => a.Sale != null).ToList();
        var unhandledAuctions = context.Auctions
            .Include(a => a.Bids)
                .ThenInclude(b => b.User)
            .Include(a => a.Product)
            .Include(a => a.Owner)
            .ToList()
            .Where(a => a.HasEnded() && !auctionsWithSales.Contains(a));

        foreach (var unhandledAuction in unhandledAuctions)
        {
            var bids = unhandledAuction.Bids;

            if (!bids.Any())
                continue;

            var winningBid = bids
                .OrderByDescending(b => b.BidAmount)
                .First();

            Console.WriteLine("Finishing auction: " + unhandledAuction.Id);

            var sale = Sale.Create(unhandledAuction, winningBid);
            context.Add(sale);
            context.SaveChanges(); // Save changes within this context instance
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(60000, stoppingToken); // Delay for 1 second or until cancellation
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AuctionContext>();
                FinalizeEndedAuctions(context);
            }
            catch (TaskCanceledException)
            {
                // Expected exception on cancellation, exit the loop
                break;
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                Console.WriteLine($"Error in AuctionFinalizer: {ex.Message}");
            }
        }
    }
}
