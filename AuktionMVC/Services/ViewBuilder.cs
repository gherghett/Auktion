// using AuctionAPI.Models;
// using AuktionMVC.Models;

// namespace AuktionMVC.Services;

// public class ViewBuilder
// {
//     private readonly IAuctionRepository _repo;
//     private readonly AuctionService _service;

//     public ViewBuilder( IAuctionRepository repo, AuctionService auctionService )
//     {
//         _repo = repo;
//         _service = auctionService;
//     }

//     public AuctionCardView GetAuctionCard( int auctionID )
//     {
//         Auction? auction = _repo.GetAuctionById( auctionID );

//         // if ( auction == null )
//             throw new Exception( "Tried to load auction that doesnt exist" );

//         Product product = auction.Product;

//         return new AuctionCardView
//         {
//             Id = auction.Id,
//             Title = product.Title
//         };
//     }

//     public AuctionCardView CreateAuctionCardView( Auction auction ) =>
//         new()
//         {
//             Id = auction.Id,
//             Title = auction.Product.Title,
//             Description = auction.Product.Description,
//             Url = "",
//             Active = auction.IsActive(),
//             CurrentBid = _service.CurrentPrice( auction ),
//             TimeRemaining = auction.TimeRemaining()
//         };

//     public List<AuctionCardView> GetAuctionCardsByUserId( int userId )
//     {
//         User? user = _repo.GetUserById( userId );
//         if ( user == null )
//             throw new Exception( "Tried to Auctions for user that does not exist" );

//         return _repo.GetAuctionsByUserId( userId ).Select( a
//                         => CreateAuctionCardView( a ) )
//                     .ToList();
//     }

//     internal AuctionPageViewModel AuctionPageViewModelFor(Auction auction, int userId)
//     {
//         var hearts = _repo.GetHeartsByUserId( userId ) ?? [];
//         bool hearted = hearts.Any(h => h.Auction.Id == auction.Id);
//         List<Bid> bids = _repo.GetBidsByAuctionId(auction.Id).ToList(); 

//         List<BidViewModel> bidViewModels = bids.Select(b => new BidViewModel(b))
//         .OrderBy( b => b.PlacedAt )
//         .ToList();

//         var auctionSummary = AuctionSummaryViewModel.Create(auction, bids, userId, hearted);

//         User seller = auction.User;
//         SellerViewModel sellerViewModel = new SellerViewModel(seller);

//         return new AuctionPageViewModel(auctionSummary, bidViewModels, sellerViewModel, userId);
//     }
// }