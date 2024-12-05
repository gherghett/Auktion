using System.ComponentModel.DataAnnotations;
namespace AuktionMVC.Models;

public class NewBidFormModel
{
    [Required]
    public int AuctionId { get; set; } 

    [Required]
    [Range(1, 99999999, ErrorMessage = "Bid amount must be between 1 and 99,999,999.")]
    public decimal BidAmount { get; set; } 
}