namespace AuctionAPI.Models;
public class CreateAuctionDto
{
    public int? ProductId { get; set; }
    public ProductSkinnyDto? Product{ get; set; } = null;
    public int OwnerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal MinimumBidPrice { get; set; }
    public decimal MinimumSalePrice { get; set; }
}
