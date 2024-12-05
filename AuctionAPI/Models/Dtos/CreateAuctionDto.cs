using AuktionMVC.Models;

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

    public static CreateAuctionDto CreateFromForm( CreateAuctionFormModel formModel)
    {
        // if(formModel.Product is null && formModel.ProductId is null)
        //     throw new ArgumentException("Not valid product"); //TODO should be result object
        
        ProductSkinnyDto? productSkinnyDto = null;
        if(formModel.ProductId is null)
        {
            productSkinnyDto = new ProductSkinnyDto {
                Name = formModel.Title,
                Description = formModel.Description,
                Images = [],
                OwnerId = formModel.OwnerId,
            };
        }
        return new CreateAuctionDto {
            OwnerId = formModel.OwnerId,
            Product = productSkinnyDto,
            ProductId = formModel.ProductId,
            StartDate = formModel.StartTime,
            EndDate = formModel.EndTime,
            MinimumBidPrice = formModel.StartingBid,
            MinimumSalePrice = formModel.ReservePrice,
        };
    }
}
