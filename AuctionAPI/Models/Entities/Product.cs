namespace AuctionAPI.Models;

public class Product : Entity
{
    public string Name { get; set; } = null!;
    public string Description { get; set;} = null!;
    public User Owner {get; set;} = null!;
    public int OwnerId { get; set;} 

    //Set when product is part of a successfully finalized sale (sold)
    public Sale? Sale{ get; set; } = null;
    public List<ProductImage> ProductImages = null!;
    public int SaleId { get; set;}

    public Product() {}

    public Product(ProductSkinnyDto dto)
    {
        Name = dto.Name;
        Description = dto.Description;
        OwnerId = dto.OwnerId;
        SaleId = dto.SaleId ?? 0; //this should alway be null i think? we only use it for new peoducts
    }
}