using System.Text;
using System.Text.Json.Serialization;
namespace AuctionAPI.Models;


public class Auction : Entity
{
    public Product Product { get; set; } = null!;
    public int ProductId { get; set; }
    public AuctionPeriod AuctionPeriod { get; set; } = null!;
    public AuctionPricing AuctionPricing { get; set; } = null!;
    public User Owner { get; set; } = null!;
    public List<Bid> Bids { get; set; } = null!;
    //Set when product is part of a successfully finalized sale (sold)
    public Sale? Sale{ get; set; } = null;
    public int? OwnerId { get; set; }
    public int? SaleId { get; set; }

    public Auction() {}
    public static Result<Auction> Create(
        User user,
        Product product,
        DateTime endDate,
        decimal minimumBidPrice,
        decimal minimumSalePrice)
    {

        var periodResult = AuctionPeriod.CreateStartingNow(endDate);
        var pricingResult = AuctionPricing.Create(minimumBidPrice, minimumBidPrice);

        var errors = (periodResult.Errors ?? []).Concat(pricingResult.Errors ?? []);

        if (errors.Any())
            return Result.Failure<Auction>(errors);

        return new Auction
        {
            Owner = user,
            Product = product,
            AuctionPeriod = periodResult.Value!,
            AuctionPricing = pricingResult.Value!
        };
    }

    public override string ToString()
    {
        return Id + " " + Owner.Name + " \"" + Product.Name + "\"" +
            (IsActive() ? " t: " + TimeRemaining().ToString("c") : "") +
            (Bids is not null ? " Bids: " + Bids?.Count : "");
    }

    public string LongString()
    {
        var sb = new StringBuilder();
        sb.Append(ToString());
        if (Bids is not null)
        {
            foreach (Bid bid in Bids)
            {
                sb.Append(bid);
            }
        }
        return sb.ToString();
    }

    public bool IsActive() => AuctionPeriod.IsActive();
    public bool HasStarted() => AuctionPeriod.HasStarted();
    public bool HasEnded() => AuctionPeriod.HasEnded();
    public TimeSpan TimeRemaining() => AuctionPeriod.TimeRemaining();

    public Option<Bid> LeadingBid()
    {
        return Bids
            .OrderByDescending(bid => bid)
            .FirstOrDefault();
    }
}

// using AuktionMVC.Core.Exceptions;

// namespace AuktionMVC.Core.Models;

// public class Auction
// {
//     private AuctionPeriod _period;
//     private AuctionPricing _pricing;
//     private Product _product;

//     // Private constructor for Dapper
// #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
//     public Auction(){}
// #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
//     public int Id { get; set; }
//     public User User { get; init; }

//     public AuctionPeriod Period
//     {
//         get => _period;
//         set
//         {
//             ValidateNotStarted();
//             _period = value ?? throw new ArgumentNullException( nameof(value) );
//         }
//     }

//     public Product Product
//     {
//         get => _product;
//         set
//         {
//             ValidateNotStarted();
//             _product = value ?? throw new ArgumentNullException( nameof(value) );
//         }
//     }

//     public AuctionPricing Pricing
//     {
//         get => _pricing;
//         set
//         {
//             ValidateNotStarted();
//             _pricing = value ?? throw new ArgumentNullException( nameof(value) );
//         }
//     }

//     private void ValidateNotStarted()
//     {
//         if ( HasStarted() )
//             throw new AuctionValidationException( [ "Cannot modify auction after it starts." ] );
//     }
//     private Auction(
//         int id,
//         User user,
//         Product product,
//         AuctionPricing pricing,
//         AuctionPeriod period
//     )
//     {
//         Id = id;
//         User = user;
//         _product = product;
//         _pricing = pricing;
//         _period = period;
//     }

//     //Core Factory Method with Validation
//     public static Auction Create(
//         User user,
//         Product product,
//         DateTime startDate,
//         DateTime endDate,
//         decimal minimumBidPrice,
//         decimal minimumSalePrice )
//     {
//         var id = 0; //new entity
//         var pricing =
//             AuctionPricing.Create( minimumBidPrice, minimumSalePrice ); 
//         var period = AuctionPeriod.Create( startDate, endDate );

//         return new Auction
//         (
//             id,
//             user ?? throw new ArgumentNullException( nameof(user) ),
//             product ?? throw new ArgumentNullException( nameof(product) ),
//             pricing,
//             period
//         );
//     }

//     public bool IsActive() => Period.IsActive();
//     public bool HasStarted() => Period.HasStarted();
//     public bool HasEnded() => Period.HasEnded();
//     public TimeSpan TimeRemaining() => Period.TimeRemaining();
// }