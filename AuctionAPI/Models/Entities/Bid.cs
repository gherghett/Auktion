
using System.ComponentModel.DataAnnotations;
namespace AuctionAPI.Models;


public class    Bid : Entity, IComparable<Bid>
{
    public User User { get; init; } = null!;
    public Auction Auction { get; init; } = null!;
    public int AuctionId { get; init; } 
    public DateTime BidDate { get; set; }
    public decimal BidAmount { get; set; }
    public int BidId { get; set; }
    public int UserId { get; set; }

    public Bid() {}

    public static Result<Bid> Create(
        User user,
        Auction auction,
        decimal bidAmount
    )
    {
        var errors = new List<string>();

        if (auction.HasEnded())
            errors.Add("Cannot place Bid on Auction that has ended");

        if (!auction.HasStarted())
            errors.Add("Cannot place Bid on Auction that has not started");

        if (bidAmount < 0)
            errors.Add("Cannot bid less than 0");

        if (errors.Any())
            return Result.Failure<Bid>(errors);

        return new Bid
        {
            User = user,
            Auction = auction,
            BidDate = DateTime.Now,
            BidAmount = bidAmount
        };
    }

    public override string ToString()
    {
        return User.Name + " " + BidAmount;
    }

    public int CompareTo(Bid? other)
    {
        if (other == null) return 1;
        return BidAmount.CompareTo(other.BidAmount);
    }

    public static bool operator >(Bid? left, Bid? right)
    {
        if (ReferenceEquals(left, right)) return false;
        if (left is null) return false;
        if (right is null) return true;
        return left.CompareTo(right) > 0;
    }

    public static bool operator <(Bid? left, Bid? right)
    {
        if (ReferenceEquals(left, right)) return false;
        if (left is null) return true;
        if (right is null) return false;
        return left.CompareTo(right) < 0;
    }

    public static bool operator >=(Bid? left, Bid? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null) return false;
        if (right is null) return true;
        return left.CompareTo(right) >= 0;
    }

    public static bool operator <=(Bid? left, Bid? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null) return true;
        if (right is null) return false;
        return left.CompareTo(right) <= 0;
    }
}