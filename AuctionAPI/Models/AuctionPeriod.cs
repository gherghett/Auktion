using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuctionAPI.Models;

public class AuctionPeriod 
{
    public DateTime StartDate { get; init;}
    public DateTime EndDate { get; init;}

    private AuctionPeriod(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public AuctionPeriod() {}

    public static Result<AuctionPeriod> Create(DateTime startDate, DateTime endDate)
    {
        var errors = new List<string>();

        if (startDate <= DateTime.Now) errors.Add("Auction must start in the future");
        if (endDate <= startDate) errors.Add("End date must be after start date");

        if (errors.Any())
            return Result.Failure<AuctionPeriod>(errors);

        return new AuctionPeriod(startDate, endDate);
    }

    public static Result<AuctionPeriod> CreateStartingNow(DateTime endDate)
    {
        var errors = new List<string>();
        var startDate = DateTime.Now;

        if (endDate <= startDate) errors.Add("End date must be after start date");

        if (errors.Any())
            return Result.Failure<AuctionPeriod>(errors);

        return new AuctionPeriod(startDate, endDate);
    }

    public bool IsActive() => EndDate >= DateTime.Now && HasStarted();
    public bool HasStarted() => StartDate <= DateTime.Now;
    public bool HasEnded() => EndDate <= DateTime.Now;
    public TimeSpan TimeRemaining() => EndDate - DateTime.Now;

    public override string ToString()
    {
        return StartDate + " - " +EndDate;
    }
}
