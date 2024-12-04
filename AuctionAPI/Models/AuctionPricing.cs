using System.ComponentModel.DataAnnotations;

namespace AuctionAPI.Models;

public class AuctionPricing 
{
    private AuctionPricing(decimal minimumBidPrice, decimal minimumSalePrice)
    {
        MinimumBidPrice = minimumBidPrice;
        MinimumSalePrice = minimumSalePrice;
    }

    public AuctionPricing() {}


    // production
    // public decimal MinimumBidPrice => _minimumBidPrice;
    // public decimal MinimumSalePrice => _minimumSalePrice;

    // //for in memory db:
    public decimal MinimumBidPrice { get; init; }

    public decimal MinimumSalePrice { get; init; }

    public static Result<AuctionPricing> Create(decimal minimumBidPrice, decimal minimumSalePrice)
    {
        var errors = new List<string>();

        if(minimumBidPrice < 0)
            errors.Add("Minimum bid cannot be negative");

        if(minimumSalePrice < 0)
            errors.Add("Minimum sale price cannot be negative");

        if(errors.Any())
            return Result.Failure<AuctionPricing>(errors);

        return new AuctionPricing(minimumBidPrice, minimumSalePrice);
    }

    // // factory only for the repo
    // public static AuctionPricing Reconstruct(decimal minimumBidPrice, decimal minimumSalePrice) =>
    //     new(minimumBidPrice, minimumSalePrice);

    // i had this helper method before but im unsure how to use result type with it
    private static void ValidatePricing(decimal minimumBid, decimal minimumSalePrice)
    {
        
    }

    public override string ToString()
    {
        return "min: "+MinimumBidPrice+" sale:"+MinimumSalePrice;
    }
}