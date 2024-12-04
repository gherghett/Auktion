namespace AuctionAPI.Models;

public class Sale : Entity
{
    public Auction Auction { get; set; } = null!;
    public int AuctionId { get; set; }
    public DateTime SaleDate { get; set; }
    public Bid WinningBid { get; set; } = null!;
    public int WinningBidId { get; set;}
    public User Buyer { get; set; } = null!;
    public int BuyerId { get; set; }
    public User Seller { get; set; } = null!;
    public int SellerId { get; set;}
    public Product Product { get; set; } = null!;
    public int ProductId {get; set;}
    public DeliveryStatus DeliveryStatus {get; set;} = null!;
    public PaymentStatus PaymentStatus {get; set;} = null!;

    public Sale() {}


    public static Sale Create(Auction auction, Bid winningBid)
    {
        return new Sale
        {
            Auction = auction,
            SaleDate = auction.AuctionPeriod.EndDate,
            WinningBid = winningBid,
            Buyer = winningBid.User,
            Seller = auction.Owner,
            Product = auction.Product,
            DeliveryStatus = DeliveryStatus.CreatePending(),
            PaymentStatus = PaymentStatus.CreatePendingPayment()
        };
    } 

    public override string ToString()
    {
        return $"{Id} {Product.Name} sold by {Seller.Name} to {Buyer.Name} {SaleDate}";
    }
}