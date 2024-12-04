namespace AuctionAPI.Models;

public class SaleSkinnyDto
{
    public SaleSkinnyDto(Sale sale)
    {
        
        Id = sale.Id;
        AuctionId = sale.AuctionId;
        SaleDate = sale.SaleDate;
        WinningBidId = sale.WinningBidId; 
        BuyerId = sale.BuyerId;           
        SellerId = sale.SellerId;         
        ProductId = sale.ProductId;       

        DeliveryStatus = sale.DeliveryStatus;
        PaymentStatus = sale.PaymentStatus;
    }

    public SaleSkinnyDto(){}


    public int Id { get; set;}
    public int AuctionId { get; set; } 
    public DateTime SaleDate { get; set; }
    public int WinningBidId { get; set; } 
    public int BuyerId { get; set; } 
    public int SellerId { get; set; } 
    public int ProductId { get; set; } 
    public DeliveryStatus DeliveryStatus {get; set;} = null!;
    public PaymentStatus PaymentStatus {get; set;} = null!;
}