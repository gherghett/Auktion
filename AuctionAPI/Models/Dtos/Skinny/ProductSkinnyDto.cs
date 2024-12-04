namespace AuctionAPI.Models;

public class ProductSkinnyDto 
{

    public ProductSkinnyDto(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        OwnerId = product.OwnerId;
        SaleId = product.SaleId;
    }
    public ProductSkinnyDto() {}
    public int Id { get; set;}
    public string Name { get; set; } = null!;
    public string[] Images {get; set;} = null!;
    public string Description {get; set;} = null!;
    public int OwnerId {get; set;} 

    //Set when product is part of a successfully finalized sale (sold)
    public int? SaleId{ get; set; } = null;


}