// namespace AuctionAPI.Models;
// public class UserStatistics
// {
//     public int ActiveAuctionsCount { get; set; }
//     public int ActiveBidsCount { get; set; }
//     public int AuctionsWonCount { get; set; }
//     public int ItemsSoldCount { get; set; }

//     public static UserStatistics Create(UserFatDto user)
//     {
//         return new UserStatistics {
//             ActiveAuctionsCount = user.Auctions.Where(a => a.IsActive).Count(),
//             ActiveBidsCount = user.Bids.Where(b => b.AuctionIsActive).Count()
//         };
//     }
// }