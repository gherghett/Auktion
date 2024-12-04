using AuktionMVC.Models;
using AuctionAPI.Models;
public class SellerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Fredrik";
        public string AvatarUrl { get; set; } = "https://picsum.photos/50";
        public int Rating { get; set; } = 3;
        public int TotalRatings { get; set; } = 125;

        public SellerViewModel (UserSkinnyDto user)
        {
            Id = user.Id;
            Name = user.Name;
            Rating = user.Rating;
        }
    }