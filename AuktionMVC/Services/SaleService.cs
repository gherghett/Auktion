using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionAPI.Models;

namespace AuktionMVC.Services
{
    public class SaleService
    {
        private readonly HttpClient _httpClient;
        private const string apiUrl = "http://localhost:5166/api/Sales/";

        public SaleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get Sale for Auction
        public async Task<Result<SaleFatDto>> GetSaleForAuctionId(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{apiUrl}ForAuction/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    // Extract any error details if possible
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Result.Failure<SaleFatDto>($"Request failed with status {response.StatusCode}: {errorMessage}");
                }

                var result = await response.Content.ReadFromJsonAsync<SaleFatDto>();

                // Handle null result (unexpected JSON structure)
                if (result == null)
                {
                    return Result.Failure<SaleFatDto>("Received null response while attempting to fetch sale for auction.");
                }

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                // Catch and handle any exceptions that occur during the request
                return Result.Failure<SaleFatDto>($"An exception occurred: {ex.Message}");
            }
        }

        public async Task<Result<List<SaleFatDto>>> GetSalesBySellerId(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{apiUrl}Seller/{id}");

                
                if (!response.IsSuccessStatusCode)
                {
                    // Extract any error details if possible
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Result.Failure<List<SaleFatDto>>($"Request failed with status {response.StatusCode}: {errorMessage}");
                }

                var result = await response.Content.ReadFromJsonAsync<List<SaleFatDto>>();

                // Handle null result (unexpected JSON structure)
                if (result == null)
                {
                    return Result.Failure<List<SaleFatDto>>("Received null response while attempting to fetch sale for auction.");
                }

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                // Catch and handle any exceptions that occur during the request
                return Result.Failure<List<SaleFatDto>>($"An exception occurred: {ex.Message}");
            }
        }

        public async Task<Result<List<SaleFatDto>>> GetSalesByBuyerId(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{apiUrl}Buyer/{id}");

                
                if (!response.IsSuccessStatusCode)
                {
                    // Extract any error details if possible
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return Result.Failure<List<SaleFatDto>>($"Request failed with status {response.StatusCode}: {errorMessage}");
                }

                var result = await response.Content.ReadFromJsonAsync<List<SaleFatDto>>();

                // Handle null result (unexpected JSON structure)
                if (result == null)
                {
                    return Result.Failure<List<SaleFatDto>>("Received null response while attempting to fetch sale for auction.");
                }

                return Result.Success(result);
            }
            catch (Exception ex)
            {
                // Catch and handle any exceptions that occur during the request
                return Result.Failure<List<SaleFatDto>>($"An exception occurred: {ex.Message}");
            }
        }
    }
}

        
