using AuctionAPI.Util; //Result
namespace AuctionAPI.Models;



public record PaymentStatus 
{
    public DateTime? ReceivedDate { get; init; } 
    public DateTime? SentDate { get; init; } 
    public PaymentState CurrentState { get; init; } = PaymentState.Pending; 

    public PaymentStatus() {}

    // Factory method to initialize a pending payment
    public static PaymentStatus CreatePendingPayment() => new PaymentStatus();

    public Result<PaymentStatus> AsReceived()
    {
        if(CurrentState != PaymentState.Pending) 
            return Result.Failure<PaymentStatus>($"Cannot transition from {CurrentState} to {PaymentState.Received}");

        return new PaymentStatus
        {
            CurrentState = PaymentState.Received,
            ReceivedDate = DateTime.Now,
        };
    }
    public Result<PaymentStatus> AsComplete()
    {
        if(CurrentState != PaymentState.Received) 
            return Result.Failure<PaymentStatus>($"Cannot transition from {CurrentState} to {PaymentState.Complete}");

        return new PaymentStatus
        {
            CurrentState = PaymentState.Complete,
            SentDate = DateTime.Now,
        };
    }

    // Enum for persistence layer to represent the state
    public enum PaymentState
    {
        Pending,  
        Received,         
        Complete      // Payment is sent
    }
}
