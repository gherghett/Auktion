namespace AuctionAPI.Models;

public record DeliveryStatus 
{
    public DateTime? SentDate { get; init; } 
    public DateTime? ReceivedDate { get; init; } 
    public DeliveryState CurrentState { get; init; } = DeliveryState.Pending; 

    public DeliveryStatus() {}

    public static DeliveryStatus CreatePending() => new DeliveryStatus();

    public Result<DeliveryStatus> AsSent()
    {
        if(CurrentState != DeliveryState.Pending)
            return Result.Failure<DeliveryStatus>($"Cannot transition from {CurrentState} to {DeliveryState.Sent}");

        return new DeliveryStatus
        {
            CurrentState = DeliveryState.Sent,
            ReceivedDate = DateTime.Now,
        };
    }

    public Result<DeliveryStatus> AsComplete()
    {
        if(CurrentState != DeliveryState.Sent)
            return Result.Failure<DeliveryStatus>($"Cannot transition from {CurrentState} to {DeliveryState.Complete}");

        return new DeliveryStatus
        {
            CurrentState = DeliveryState.Complete,
            SentDate = DateTime.Now,
        };
    }


    // Enum for persistence layer to represent the state
    public enum DeliveryState
    {
        Pending,
        Sent,
        Complete  // Delivery is received
    }
}
