using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuktionMVC.Models;
using AuktionMVC.Services;
using AuctionAPI.Models;
using AuktionMVC.Extensions;
using NuGet.Protocol;


namespace AuktionMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AuktionMVC.Services.AuctionService _auctionService;
    private readonly AuktionMVC.Services.SaleService _saleService;
    private readonly AuktionMVC.Services.UserService _userService;



    public HomeController(ILogger<HomeController> logger,
                        AuktionMVC.Services.AuctionService  auctionService,
                        AuktionMVC.Services.SaleService     saleService,
                        AuktionMVC.Services.UserService     userService)

    {
        _logger = logger;
        _auctionService = auctionService;
        _saleService = saleService;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        
        // Fetch data from the API
        var result = await _auctionService.GetAuctionsAsync();
        
        ViewData["Title"] = $"Auktioner: Hitta och skapa ";
        // Pass the result to the View
        return View(result);
    }

    public async Task<IActionResult> Auction(int id)
    {
        int userId = 0;
        if (User.Identity?.IsAuthenticated ?? false)
            userId = User.GetUserId();

        var auctionResult = await _auctionService.GetAuctionByIdAsync(id);
        if (auctionResult.IsFailure)
            return RedirectToAction("AuctionNotFound", new { id });
        AuctionFatDto auction = auctionResult.Value!;

        AuctionPageViewModel auctionViewModel = new AuctionPageViewModel(auction, userId);

        ViewData["Title"] = $"{auctionViewModel.Auction.Title} Auktion";

        //active or pending auction
        if (!auction.HasEnded)
        {
            return View("Auction", auctionViewModel);
        }

        //if theres no sale object, then the auction was without winner, unsold
        if(auction.Sale is null)
        {
            return View("UnsoldAuction", auctionViewModel);
        }

        var saleResult = await _saleService.GetSaleForAuctionId(id); 
        if (saleResult.IsFailure) 
            return RedirectToAction("AuctionNotFound", new { id });
            
        var saleSummary = SaleSummaryViewModel.Create(saleResult.Value!,
            userId);
        auctionViewModel.SaleSummary = saleSummary;

        return View("Sale", auctionViewModel);
    }

        /// <summary>
    /// GET: /Home/Profil
    /// Displays public profile information
    /// Note: This is different from the authenticated profile in MinaSidor
    /// </summary>
    public async Task<IActionResult> Profil(int id)
    {
        int otherUserId = id;
        int userId = 0;
        if(User.Identity?.IsAuthenticated ?? false)
            userId = User.GetUserId();

        var otherUserResult = await _userService.GetUserById(otherUserId);
        if(otherUserResult.IsFailure)
            return NotFound();
        var otherUser = otherUserResult.Value!;

        var otherUserAuctionsResult = await _auctionService.GetAuctionsByUserId(otherUserId);
        var otherActiveAuctions = new List<AuctionFatDto>();
        if(otherUserAuctionsResult.IsSuccess)
        {
            otherActiveAuctions = otherUserAuctionsResult.Value!.Where(a => a.IsActive).ToList();
        }

        var otherSalesResult = await _saleService.GetSalesBySellerId(otherUserId);
        var otherSales = new List<SaleFatDto>();
        if(otherSalesResult.IsSuccess)
        {
            otherSales = otherSalesResult.Value!;
        }
        

        var viewModel = new OtherProfilePageViewModel
        {
            UserSummary = new UserSummary
            {
                Id = otherUser.Id,
                Username = otherUser.Name,
                Email = otherUser.Email,
                Address = otherUser.Address,
                MemberSince = otherUser.CreationDate,
                Rating = otherUser.Rating
            },
            Statistics = new UserStatistics
            {
                TotalPurchases = otherUser.Buys.Count,
                TotalSales = otherUser.Sales.Count,
                ActiveAuctions = otherActiveAuctions.Count() 
            },
            ActiveAuctions = otherActiveAuctions
                .Select(a => AuctionSummaryViewModel.Create(a, userId))
                .ToList(),
            CompletedSales = otherSales
                .Select(s => SaleSummaryViewModel.Create(s, userId))
                .ToList()
        };
        ViewData["Title"] = $"Profil {viewModel.UserSummary.Username}";
        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        ViewData["Title"] = $"Privacy";
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        ViewData["Title"] = $"Error";
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
