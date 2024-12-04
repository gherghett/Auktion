using System.Diagnostics;
using AuctionAPI.Models;
using AuktionMVC.Models;
using AuktionMVC.Extensions;
using AuktionMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuktionMVC.Controllers;

/// <summary>
/// Controller for managing user-specific auction functionality.
/// Requires authentication for all actions.
/// Base route: /MinaSidor/
/// </summary>
[Authorize]
// [Route("MinaSidor")] 
public class MinaSidorController : Controller
{
    private readonly ILogger<MinaSidorController> _logger;
    private readonly AuctionService _auctionService;
    private readonly SaleService _saleService;

    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly UserService _userService;

    public MinaSidorController(
        ILogger<MinaSidorController> logger,
        AuctionService auctionService,
        SaleService saleService,

        IWebHostEnvironment webHostEnvironment,
        UserService userService
    )
    {
        _logger = logger;
        _auctionService = auctionService;
        _saleService = saleService;
        _webHostEnvironment = webHostEnvironment;
        _userService = userService;
    }

    /// <summary>
    /// GET: /MinaSidor/
    /// Displays the main dashboard page for the logged-in user
    /// </summary>
    public async Task<IActionResult> Index()
    {
        int userId = User.GetUserId();
        var userResult = await _userService.GetUserById(userId);
        if(userResult.IsFailure)
            throw new ArgumentNullException("No user found with id: " + userId);
        var user = userResult.Value!;

        var model = MinaSidorViewModel.Create(user);

        return View(model);
    }

    /// <summary>
    /// GET: /MinaSidor/Auktioner
    /// Shows all active auctions created by the current user
    /// </summary>
    public async Task<IActionResult> Auktioner()
    {
        int userId = User.GetUserId();
        var userResult = await _userService.GetUserById(userId);
        if(userResult.IsFailure)
            throw new ArgumentNullException("No user found with id: " + userId);
        var user = userResult.Value!;

        var userAuctionsResult = await _auctionService.GetAuctionsByUserId(userId);
        var auctions = new List<AuctionFatDto>();
        if(userAuctionsResult.IsSuccess)
        {
            auctions = userAuctionsResult.Value!;
        }

        var salesResult = await _saleService.GetSalesBySellerId(userId);
        var sales = new List<SaleFatDto>();
        if(salesResult.IsSuccess)
        {
            sales = salesResult.Value!;
        }

        var buysResult = await _saleService.GetSalesByBuyerId(userId);
        var buys = new List<SaleFatDto>();
        if(buysResult.IsSuccess)
        {
            buys = salesResult.Value!;
        }

        var viewModel = new AuctionManagementPageViewModel(user, auctions, buys, sales);
        return View(viewModel);
    }

    /// <summary>
    /// GET: /MinaSidor/Finished
    /// Displays completed auctions for the current user (both as seller and buyer)
    /// </summary>
    // public IActionResult Finished()
    // {
    //     int userId = User.GetUserId();

    //     var model = FinishedAuctionsViewModel.Create(
    //         _repo.GetSalesByBuyerId(userId).ToList(),
    //         _repo.GetSalesBySellerId(userId).ToList()
    //     );

    //     return View(model);
    // }

    /// <summary>
    /// GET: /MinaSidor/Bud
    /// Shows all bids placed by the current user on various auctions
    /// </summary>
    // public IActionResult Bud()
    // {

    //     int userId = User.GetUserId();
    //     User? user = _repo.GetUserById(userId);
    //     if(user == null)
    //         throw new Exception("Very strang error");

    //     var hearts = _repo.GetHeartsByUserId(userId).Select(h => h.Auction.Id);
    //     var model = new BudPageViewModel
    //     {
    //         ActiveAuctions = _repo.GetBidsByUserId(userId)
    //             .Select(b => b.Auction)
    //             .DistinctBy(a => a.Id)
    //             .Select(a => AuctionSummaryViewModel.Create(
    //                     a,
    //                     _repo.GetBidsByAuctionId(a.Id).ToList(),
    //                     userId, hearts.Contains(a.Id))
    //             )
    //             .ToList(),
    //         SavedAuctions = hearts.Select(h => _repo.GetAuctionById(h))
    //             .Select(a => AuctionSummaryViewModel.Create(
    //                     a,
    //                     _repo.GetBidsByAuctionId(a.Id).ToList(),
    //                     userId,
    //                     true)
    //             )
    //             .ToList()
    //     };
    //     model.LeadingBids = model.SavedAuctions
    //         .Where(a => a.YourHighestBid == a.CurrentBid && a.YourHighestBid != 0)
    //         .Count();

    //     return View(model);
    // }

    /// <summary>
    /// GET: /MinaSidor/Notiser
    /// Displays notifications and alerts for the current user
    /// Page isnt implemented
    /// </summary>
    public IActionResult Notiser() => View();

    /// <summary>
    /// GET: /MinaSidor/Profil
    /// Shows and allows editing of the current user's profile information
    /// Throws exception if user is not found in the database
    /// </summary>
    // public IActionResult Profil()
    // {
    //     int userId = User.GetUserId();
    //     User? user = _repo.GetUserById(userId);
    //     if(user == null)
    //         throw new Exception("Very strang error");

    //     return View(user);
    // }

    /// <summary>
    /// POST: /MinaSidor/UpdateUser
    /// Updates the current user's profile information
    /// Returns to profile page with errors if validation fails
    /// Throws exception if user is not found
    /// </summary>
    // [HttpPost]
    // public IActionResult UpdateUser([FromForm] UpdateProfileModel model)
    // {
    //     int userId = User.GetUserId();
    //     User? user = _repo.GetUserById(userId);
    //     if(user == null)
    //         throw new Exception("Very strang error");

    //     if(!ModelState.IsValid) //TODO feedback
    //         return RedirectToAction("Profil", "MinaSidor");

    //     var result = _userService.Update(userId, model);
    //     if(!result.IsSuccess)
    //         throw new Exception("Very strang error");

    //     return RedirectToAction("Profil", "MinaSidor");
    // }

    /// <summary>
    /// POST: /MinaSidor/ChangePassword
    /// Updates the current user's password
    /// Validates current password and confirms new password match
    /// Returns to profile page on success
    /// Throws exception for validation failures
    /// </summary>
    // [HttpPost]
    // public IActionResult ChangePassword([FromForm] ChangePasswordModel model)
    // {
    //     int userId = User.GetUserId();
    //     User? user = _repo.GetUserById(userId);
    //     if(user == null)
    //         throw new Exception("Very strang error");

    //     if(model.NewPassword != model.ConfirmPassword)
    //         return RedirectToAction("Profil", "MinaSidor");

    //     var validateResult = _userService.ValidateUser(user.Email, model.CurrentPassword);
    //     if(!validateResult.IsSuccess)
    //         throw new Exception("Very, very strang error");

    //     var result = _userService.ChangePassword(user.Id, model.NewPassword);
    //     if(!result.IsSuccess)
    //         throw new Exception("Very, very, very strang error");

    //     return RedirectToAction("Profil", "MinaSidor");

    // }

    /// <summary>
    /// GET: /MinaSidor/NewAuction
    /// Displays the form for creating a new auction
    /// </summary>
    public IActionResult NewAuction() =>
        View();

    /// <summary>
    /// POST: /MinaSidor/CreateAuction
    /// Creates a new auction with the provided details
    /// Handles image upload, product creation, and auction setup
    /// Returns to form with errors if validation fails
    /// Redirects to auction view page on success
    /// </summary>
    /// <param name="model">CreateAuctionViewModel containing auction details</param>
    [HttpPost]
    public async Task<IActionResult> CreateAuction([FromForm] CreateAuctionFormModel model)
    {
        //TODO this method does not work
        if(!ModelState.IsValid)
        {
            return View("NewAuction", model);
        }

        int userId = User.GetUserId();
        var userResult = await _userService.GetUserById(userId);
        if(userResult.IsFailure)
            throw new ArgumentNullException("No user found with id: " + userId);
        var user = userResult.Value!;
        // CreateAuctionDto o;
        CreateAuctionDto createAuctionDto = CreateAuctionDto.Create(model);
        var auction = await _auctionService.CreateAuctionAsync(createAuctionDto);
        if(auction is null)
            throw new Exception(".... somethings... wrong");

        return RedirectToAction("Auction", "Home", new { id = auction.Id });
    }

    /// <summary>
    /// POST: /MinaSidor/NewBid
    /// Places a new bid on an auction for the current user
    /// Returns to auction page on success
    /// Returns BadRequest if model validation fails
    /// </summary>
    /// <param name="model">Bid details including auction ID and bid amount</param>
    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> NewBid([FromForm] BidInputModel model)
    // {
    //     int userId = User.GetUserId();
    //     User? user = _repo.GetUserById(userId);
    //     if(user == null)
    //         throw new Exception("Very strang error");

    //     if(!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState);
    //     }

    //     try
    //     {
    //         _auctionService.PlaceBid(model.AuctionId, userId, model.BidAmount);
    //     }
    //     catch(Exception ex)
    //     {
    //         throw ex;
    //     }

    //     return RedirectToAction("Auction", "Home", new { id = model.AuctionId });
    // }

   /// <summary>
    /// GET: /MinaSidor/AddHeart/{auctionId}
    /// Adds a heart/favorite marking to the specified auction for the current user
    /// Returns HTTP 200 OK on success
    /// Throws InvalidOperationException if user not found
    /// </summary>
    /// <param name="auctionId">ID of the auction to favorite</param>
    // [Route("[controller]/[action]/{auctionId}")]
    // public async Task<IActionResult> AddHeart(int auctionId)
    // {
    //     int userId = User.GetUserId();
    //     User? user = _repo.GetUserById(userId);

    //     if(user == null)
    //         throw new InvalidOperationException("User not found");

    //     _repo.AddHeart(userId, auctionId);

    //     return Ok();
    // }

    /// <summary>
    /// GET: /MinaSidor/RemoveHeart/{auctionId}
    /// Remove heart from auction for user
    /// </summary>
    /// <returns>Task that completes when the heart is added</returns>
    // [Route("[controller]/[action]/{auctionId}")]
    // public async Task<IActionResult> RemoveHeart(int auctionId)
    // {
    //     int userId = User.GetUserId();
    //     User? user = _repo.GetUserById(userId);

    //     if(user == null)
    //         throw new InvalidOperationException("User not found");

    //     _repo.RemoveHeart(userId, auctionId);

    //     return Ok();
    // }

    /// <summary>
    /// GET: /MinaSidor/Error
    /// Displays error information for the current request
    /// No caching enabled for error pages
    /// </summary>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel
    { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}