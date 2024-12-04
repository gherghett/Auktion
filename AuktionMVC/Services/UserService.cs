using AuctionAPI.Models;
using AuctionAPI.Util;
using AuctionAPI;
using AuktionMVC.Models;
using System.Net;

namespace AuktionMVC.Services;

public class UserService
{
    private readonly HttpClient _httpClient;
    private const string apiUrl = "http://localhost:5166/api/Users/";

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // här kansne vi vill kunna int CalculateRating(int userId), based på alla reviews, 
    // så använda repot för att hänta alla reviews för en user, sen ta fram medelvärdet
    // en annan metod kanske updaterar använderens rating, den körs kanske av en background job en gång om
    // dagen

    // //TODO
    // public Result<bool> IsHearted(int userId, int auctionId)
    // {
    //     var anyHeart = _repository.GetHeartsByUserId(userId)
    //         .Where(h => h.Auction.Id == auctionId)
    //         .Any();

    //     if (anyHeart)
    //         return Result.Success(true);
    //     else
    //         return Result.Success(false);
    // }

    // public Result<bool> HeartAuction(int userId, int auctionId)
    // {
    //     bool result = _repository.AddHeart(userId, auctionId);
    //     return Result.Success(result);
    // }

    // public Result<bool> UnHeartAuction(int userId, int auctionId)
    // {
    //     bool result = _repository.RemoveHeart(userId, auctionId);
    //     return Result.Success(result);
    // }

    // public Result<int> CalculateUserRating(int userId)
    // {
    //     if (_repository.GetUserById(userId) == null)
    //         return Result.Failure<int>("No user found with id: " + userId);

    //     var reviews = _repository.GetReviewsBySubjectId(userId).ToList();

    //     decimal totalScore = 0;
    //     decimal totalReviews = reviews.Count;

    //     if (totalReviews == 0)
    //         return Result.Success(0);

    //     foreach (var r in reviews)
    //         totalScore += r.Rating;

    //     int finalRating = (int)Math.Round(totalScore / totalReviews, MidpointRounding.ToEven);
    //     return Result.Success(finalRating);
    // }

    // /// <summary>
    // /// Updates the user's rating based on their review ratings.
    // /// </summary>
    // /// <param name="userId"></param>
    // /// <returns>True if the operation was successful.</returns>
    // public Result<bool> UpdateUserRating(int userId)
    // {
    //     User? user = _repository.GetUserById(userId);
    //     if (user == null)
    //         return Result.Failure<bool>("No user found with id: " + userId);

    //     var newRating = CalculateUserRating(userId);
    //     if (!newRating.IsSuccess)
    //         return Result.Failure<bool>(newRating.Errors);

    //     user.Rating = newRating.Value;
    //     return Result.Success(true);
    // }

    public async Task<Result<UserFatDto>> ValidateUser(LoginModel loginModel)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(apiUrl+"ValidateLogin/", loginModel);
            if (!response.IsSuccessStatusCode)
                return Result.Failure<UserFatDto>("");

            var userDto = await response.Content.ReadFromJsonAsync<UserFatDto>();
            if (userDto is null)
                return Result.Failure<UserFatDto>("");
            
            return userDto;
        }
        catch
        {
            return Result.Failure<UserFatDto>("Could not login");
        }
    }

    public async Task<Result<UserFatDto>> GetUserById(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync(apiUrl+id);
            if (!response.IsSuccessStatusCode)
                return Result.Failure<UserFatDto>("");

            var userDto = await response.Content.ReadFromJsonAsync<UserFatDto>();
            if (userDto is null)
                return Result.Failure<UserFatDto>("");
            
            return userDto;
        }
        catch
        {
            return Result.Failure<UserFatDto>("Could not get user");
        }
    }

    // public Result<User> CreateUser(CreateUserModel user)
    // {
    //     try
    //     {
    //         User newUser;
    //         int savedUser = _repository.CreateUser(User.Create(user));
    //         newUser = _repository.GetUserById(savedUser);
    //         return Result.Success(newUser);
    //     }
    //     catch
    //     {
    //         return Result.Failure<User>("Could not create user");
    //     }
    // }

    // public Result<User> Update(int userId, UpdateProfileModel model)
    // {
    //     try
    //     {
    //         User user = _repository.GetUserById(userId);
    //         user.Address = model.Address;
    //         user.Email = model.Email;
    //         bool success = _repository.UpdateUser(user);
    //         if (!success)
    //             Result.Failure<User>("Could not save updates for user");
    //         User newUser = _repository.GetUserById(user.Id);
    //         return Result.Success(newUser);
    //     }
    //     catch
    //     {
    //         return Result.Failure<User>("Could not update user");
    //     }
    // }

    // public Result<User> ChangePassword(int userId, string newPassword)
    // {
    //     try
    //     {
    //         User user = _repository.GetUserById(userId);
    //         user.Password = newPassword;
    //         bool success = _repository.UpdateUser(user);
    //         if (!success)
    //             Result.Failure<User>("Could not save updates for user");
    //         User newUser = _repository.GetUserById(user.Id);
    //         return Result.Success(newUser);
    //     }
    //     catch
    //     {
    //         return Result.Failure<User>("Could not update user");
    //     }
    // }

    // internal int GetUserPurchaseCount(int userId) =>
    //     GetUserCompletedPurchases(userId).Count();

    // public IEnumerable<Sale> GetUserCompletedPurchases(int userId) =>
    //     _repository.GetSalesByBuyerId(userId);

    // internal int GetUserSalesCount(int userId) =>
    //     GetUserCompletedSales(userId).Count();

    // internal int GetUserActiveAuctionsCount(int userId) =>
    //     GetUserActiveAuctions(userId).Count();
    

    // internal IEnumerable<Auction> GetUserActiveAuctions(int userId) =>
    //     _repository.GetAuctionsByUserId(userId)
    //         .Where(a => a.IsActive());

    // internal IEnumerable<Sale> GetUserCompletedSales(int userId) => 
    //     _repository.GetSalesBySellerId(userId);

    // internal int GetUserPendingAuctionsCount(int userId) =>
    //     GetUserPendingAuctions(userId).Count();
    

    // internal int GetUserCompletedSalesCount(int userId) =>
    //     GetUserCompletedSales(userId).Count();
    

    // internal IEnumerable<Auction> GetUserPendingAuctions(int userId) =>
    //     _repository.GetAuctionsByUserId(userId)
    //         .Where(a => !a.HasStarted());
}