using Microsoft.EntityFrameworkCore;
namespace AuctionAPI.Models;

public class AuctionContextFactory(DbContextOptions<AuctionContext> options) : IDbContextFactory<AuctionContext>
{
    private DbContextOptions<AuctionContext> _options = options;
    public AuctionContext CreateDbContext()
    {
        return new AuctionContext(_options);
    }
}
