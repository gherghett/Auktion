using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using AuctionAPI.Models;
using AuctionAPI.Util;

namespace AuctionAPI.Services;

public class AuctionService
{
    AuctionContext _context;
    public AuctionService(AuctionContext context)
    {
        _context = context;
    }

    public Result<Bid> PlaceBid(int auctionId, int placerId, decimal amount)
    {
        var auction = _context.Auctions.Where(a => a.Id  == auctionId)
            .Include(a => a.Owner)
            .Include(a => a.Product)
            .Include(a => a.Bids)
            .Include(a => a.Sale)
            .SingleOrDefault();
        var user = _context.Users.Where(u => u.Id == placerId)
            .SingleOrDefault();
        var errors = new List<string>();
        if(auction is null)
            errors.Add("Auction not found");
        if(user is null)
            errors.Add("User not found");
        
        if(errors.Any())
            return Result.Failure<Bid>(errors);

        Result<Bid> creationResult = Bid.Create(user!, auction!, amount);

        errors.AddRange(creationResult.Errors ?? []); 

        if (user!.Id == auction!.OwnerId)
            errors.Add("User cannot place bid on own auction");

        LeadingBid(auction)
            .Where(bid => bid.BidAmount >= amount)
            .IfSome(bid => errors.Add($"Cannot place bid lower than currently highest bid of {bid.BidAmount}"));

        if (errors.Any())
            return Result.Failure<Bid>(errors);

        Bid bid = creationResult.Value!;
        _context.Add(bid);
        _context.SaveChanges();

        return bid;
        //return creationResult.OrInvoke(()=> throw new Exception());
    }

    public Option<Bid> LeadingBid(Auction auction)
    {   
        return _context.Auctions.Where(a => a.Id == auction.Id)
            .Include(a => a.Bids)
            .Single()
            .LeadingBid();
    }
    public async Task<Result<Auction>> AddAuction(CreateAuctionDto auctionDto)
    {
        var userTask = _context.Users.Where(u => u.Id == auctionDto.OwnerId).SingleOrDefaultAsync();

        Product? product;
        if(auctionDto.ProductId is not null)
        {
            product = await _context.Products.Where(p => p.Id == auctionDto.ProductId).SingleOrDefaultAsync();
            if(product is null)
                return Result.Failure<Auction>($"The product with id {auctionDto.ProductId} did not exists");
        }
        else
        {
            var dto = auctionDto.Product;
            if(dto is null)
                return Result.Failure<Auction>("Could not create product bc no product info was in Dto");
            product = new Product(dto);
        }

        var user = await userTask;

        var errors = new List<string>();

        // Validate user and product
        if (user is null)
            errors.Add("User does not exist");

        if (product is null)
        {
            errors.Add("Product does not exist");
        }
        else
        {
            if(user is not null && product.OwnerId != user.Id)
                errors.Add("User is not owner of product");

            // Check for existing auctions with the product
            var existingAuctions = await _context.Auctions
                .Where(a => a.Product.Id == product.Id)
                .ToListAsync();

            if (existingAuctions.Any())
                errors.Add($"Cannot add Auction with Product Id:{product.Id} " +
                           $"that already is associated with another auction Id:{existingAuctions.First().Id}.");
        }

        if (errors.Any())
        {
            // Return an error result if there are validation errors
            return Result.Failure<Auction>(errors);
        }

        // Create the auction using the retrieved user and product
        var auctionResult = Auction.Create(
            user: user!,
            product: product!,
            endDate: auctionDto.EndDate,
            minimumBidPrice: auctionDto.MinimumBidPrice,
            minimumSalePrice: auctionDto.MinimumSalePrice
        );

        if (!auctionResult.IsSuccess)
        {
            // Return failure if auction creation fails
            return auctionResult;
        }

        // Add and save the auction
        _context.Add(auctionResult.Value!);
        await _context.SaveChangesAsync();

        return auctionResult;
    }
}