namespace AuktionMVC.Models;
using System;
using System.Linq;
using AuctionAPI.Models;

public class SaleSummaryViewModel
{
    public int Id { get; private set; }
    public bool UsersIsSeller { get; private set; }
    public bool UsersIsBuyer { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ImageUrl { get; private set; }
    public decimal FinalPrice { get; private set; }
    public DateTime SaleDate { get; private set; }
    public string BuyerName { get; private set; }
    public string SellerName { get; private set; }
    public int BuyerId { get; private set; }
    public int SellerId { get; private set; }
    public bool IsPaymentReceived { get; private set; }
    public bool IsPaymentSent { get; private set; }
    public int TotalBids { get; private set; }

    private SaleSummaryViewModel(
        int id,
        bool usersIsSeller,
        bool usersIsBuyer,
        string title,
        string description,
        string imageUrl,
        decimal finalPrice,
        DateTime saleDate,
        string buyerName,
        string sellerName,
        int buyerId,
        int sellerId,
        DateTime? paymentReceived,
        DateTime? paymentSent,
        int totalBids)
    {
        Id = id;
        UsersIsSeller = usersIsSeller;
        UsersIsBuyer = usersIsBuyer;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
        FinalPrice = finalPrice;
        SaleDate = saleDate;
        BuyerName = buyerName ?? throw new ArgumentNullException(nameof(buyerName));
        SellerName = sellerName ?? throw new ArgumentNullException(nameof(sellerName));
        BuyerId = buyerId;
        SellerId = sellerId;
        IsPaymentReceived = paymentReceived.HasValue;
        IsPaymentSent = paymentSent.HasValue;
        TotalBids = totalBids;
    }

    public static SaleSummaryViewModel  Create(SaleFatDto sale, int currentUserId)
    {
        if (sale is null) throw new ArgumentNullException(nameof(sale));
        if (sale.Product is null) throw new ArgumentNullException(nameof(sale.Product));
        if (sale.Buyer is null) throw new ArgumentNullException(nameof(sale.Buyer));
        if (sale.Seller is null) throw new ArgumentNullException(nameof(sale.Seller));
        if (sale.WinningBid is null) throw new ArgumentNullException(nameof(sale.WinningBid));

        var viewModel = new SaleSummaryViewModel(
            id: sale.Id,
            usersIsSeller: sale.Seller.Id == currentUserId,
            usersIsBuyer: sale.Buyer.Id == currentUserId,
            title: sale.Product.Name,
            description: "description", //sale.Product.Description,
            imageUrl: "", //sale.Product.Pictures?.FirstOrDefault() ?? "/images/products/pants.jpg",
            finalPrice: sale.WinningBid.BidAmount,
            saleDate: sale.SaleDate,
            buyerName: sale.Buyer.Name,
            sellerName: sale.Seller.Name,
            buyerId: sale.Buyer.Id,
            sellerId: sale.Seller.Id,
            paymentReceived: 
                sale.PaymentStatus.CurrentState != PaymentStatus.PaymentState.Pending 
                    ? sale.PaymentStatus.ReceivedDate 
                    : null, // PaymentRecieved == default ? null : sale.PaymentRecieved,
            paymentSent: 
                sale.PaymentStatus.CurrentState == PaymentStatus.PaymentState.Complete 
                    ? sale.PaymentStatus.SentDate 
                    : null,//sale.PaymentSent == default ? null : sale.PaymentSent,
            totalBids: sale.Bids?.Count ?? 0
        );

        return viewModel;
    }
}
