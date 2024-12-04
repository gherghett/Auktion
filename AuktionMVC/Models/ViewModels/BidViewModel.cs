    using AuctionAPI.Models;
    namespace AuktionMVC.Models;
    public class    BidViewModel
    {
        public int Id { get; set; }
        public int BidderId { get; set; }
        public string BidderName { get; set; } = null!;
        public string BidderAvatarUrl 
        { 
            get { return "https://picsum.photos/40/"; }
            set {} 
         }
        public decimal Amount { get; set; }
        public DateTime PlacedAt { get; set; }

        public TimeSpan TimeSinceBid => DateTime.UtcNow - PlacedAt;

        public  BidViewModel(BidSkinnyDto bid) {
            Id = bid.Id;
            BidderId = bid.UserId;
            BidderName = bid.UserName;
            // BidderAvatarUrl = "https://picsum.photos/40/";
            Amount = bid.BidAmount;
            PlacedAt = bid.BidDate;
        }

    }
