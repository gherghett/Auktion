using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionAPI.Models;
using AuktionMVC.Models;
using System.Text.Json;

namespace AuktionMVC.Services
{
    public class AuctionService
    {
        private readonly HttpClient _httpClient;
        private const string apiUrl = "http://localhost:5166/api/Auctions/";

        public AuctionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all auctions
        public async Task<List<AuctionFatDto>> GetAuctionsAsync()
        {
            Console.WriteLine(apiUrl);
            var result = await _httpClient.GetFromJsonAsync<List<AuctionFatDto>>(apiUrl);
            return result ?? new List<AuctionFatDto>();
        }

        // Get a specific auction by ID
        public async Task<Result<AuctionFatDto>> GetAuctionByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{apiUrl}{id}");

                
                if (!response.IsSuccessStatusCode)
                {
                    // Extract any error details if possible
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Result.Failure<AuctionFatDto>($"Request failed with status {response.StatusCode}: {errorMessage}");
                }

                var result = await response.Content.ReadFromJsonAsync<AuctionFatDto>();

                // Handle null result (unexpected JSON structure)
                if (result == null)
                {
                    return Result.Failure<AuctionFatDto>("Received null response while attempting to fetch sale for auction.");
                }

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                // Catch and handle any exceptions that occur during the request
                return Result.Failure<AuctionFatDto>($"An exception occurred: {ex.Message}");
            }
        }

        // Get active auctions
        public async Task<List<AuctionSkinnyDto>> GetActiveAuctionsAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<AuctionSkinnyDto>>($"{apiUrl}Active");
            return result ?? new List<AuctionSkinnyDto>();
        }

        public async Task<Result<List<AuctionFatDto>>> GetAuctionsByUserId(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{apiUrl}User/{id}");

                
                if (!response.IsSuccessStatusCode)
                {
                    // Extract any error details if possible
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Result.Failure<List<AuctionFatDto>>($"Request failed with status {response.StatusCode}: {errorMessage}");
                }

                var result = await response.Content.ReadFromJsonAsync<List<AuctionFatDto>>();

                // Handle null result (unexpected JSON structure)
                if (result == null)
                {
                    return Result.Failure<List<AuctionFatDto>>("Received null response while attempting to fetch sale for auction.");
                }

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                // Catch and handle any exceptions that occur during the request
                return Result.Failure<List<AuctionFatDto>>($"An exception occurred: {ex.Message}");
            }
        }

        // Create a new auction
        public async Task<AuctionFatDto?> CreateAuctionAsync(CreateAuctionFormModel formModel)
        {
            var auctionDto = CreateAuctionDto.CreateFromForm(formModel);
            var response = await _httpClient.PostAsJsonAsync(apiUrl, auctionDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuctionFatDto>();
            }
            else
            {
                Console.WriteLine($"Failed to create auction. Status Code: {response.StatusCode}");
                Console.WriteLine($"Request URL: {apiUrl}");
                Console.WriteLine($"Request Body: {JsonSerializer.Serialize(auctionDto)}");
                Console.WriteLine($"Response Status: {response.StatusCode}");
                Console.WriteLine($"Response Body: {await response.Content.ReadAsStringAsync()}");
                return null;
            }
        }


        // Place a bid on an auction
        public async Task<Result<AuctionFatDto>> PlaceBidAsync(int auctionId, int userId, decimal amount)
        {
            var dto = new CreateBidDto{
                UserId = userId,
                BidAmount = amount,
                AuctionId = auctionId
            };
            var json = System.Text.Json.JsonSerializer.Serialize(dto);
            Console.WriteLine($"Request Body: {json}");
            var response = await _httpClient.PostAsJsonAsync($"{apiUrl}PlaceBid/{auctionId}", dto);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to place bid. Status Code: {response.StatusCode}");
                Console.WriteLine(response.Content);
                return Result.Failure<AuctionFatDto>("Failed to place the bid....");
            }

            var result = await response.Content.ReadFromJsonAsync<AuctionFatDto>();
            if(result is null)
                return Result.Failure<AuctionFatDto>("Could not parse bid");

            return result;
        }

        // Update an existing auction
        public async Task<bool> UpdateAuctionAsync(int id, Auction auction)
        {
            var response = await _httpClient.PutAsJsonAsync($"{apiUrl}{id}", auction);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to update auction. Status Code: {response.StatusCode}");
                return false;
            }
        }

        // Delete an auction
        public async Task<bool> DeleteAuctionAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{apiUrl}{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"Failed to delete auction. Status Code: {response.StatusCode}");
                return false;
            }
        }

    }
}
