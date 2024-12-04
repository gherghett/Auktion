using System.ComponentModel.DataAnnotations;
namespace AuktionMVC.Models;

public class CreateAuctionFormModel
{
    [Required]
    public int OwnerId { get; set; }

    //Either this or the 
    public int? ProductId {get; set;}

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public IList<IFormFile> Images { get; set; } = new List<IFormFile>(); // Support multiple images

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a category")]
    public int CategoryId { get; set; }

    public string? Tags { get; set; }

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Starting bid must be greater than 0")]
    public decimal StartingBid { get; set; }

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Reserve price must be greater than 0")]
    public decimal ReservePrice { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }
}
