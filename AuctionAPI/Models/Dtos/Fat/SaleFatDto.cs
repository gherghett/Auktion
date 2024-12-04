namespace AuctionAPI.Models;

public class SaleFatDto
{
    public SaleFatDto(Sale sale)
    {
        Id = sale.Id;
        Auction = new AuctionSkinnyDto(sale.Auction);
        SaleDate = sale.SaleDate;
        WinningBid = new BidSkinnyDto(sale.WinningBid); 
        Buyer = new UserSkinnyDto(sale.Buyer);           
        Seller = new UserSkinnyDto(sale.Seller);         
        Product = new ProductSkinnyDto(sale.Product);       
        Bids = sale.Auction.Bids.Select(b => new BidSkinnyDto(b)).ToList();
        DeliveryStatus = sale.DeliveryStatus;
        PaymentStatus = sale.PaymentStatus;
    }

    public SaleFatDto(){}


    public int Id { get; set;}
    public AuctionSkinnyDto Auction { get; set; } = null!;
    public DateTime SaleDate { get; set; }
    public BidSkinnyDto WinningBid { get; set; } = null!;
    public List<BidSkinnyDto> Bids {get; set;} = null!;
    public UserSkinnyDto Buyer { get; set; } = null!;
    public UserSkinnyDto Seller { get; set; } = null!;
    public ProductSkinnyDto Product { get; set; } = null!;
    public DeliveryStatus DeliveryStatus {get; set;} = null!;
    public PaymentStatus PaymentStatus {get; set;} = null!;
}