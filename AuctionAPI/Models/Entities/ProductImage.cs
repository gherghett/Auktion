namespace AuctionAPI.Models;

public class ProductImage : Image
{
    public Product Product  { get; set; } = null!;
}