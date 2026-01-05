using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GymCheckIn.Models;

namespace GymCheckIn.Services
{
    public class FitAddisApiService
    {
        private HttpClient _httpClient;
        private ApiSettings _settings;

        public event EventHandler<string> OnLog;

        public FitAddisApiService(ApiSettings settings)
        {
            _settings = settings;
            InitializeHttpClient();
        }

        private void InitializeHttpClient()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };

            if (!string.IsNullOrEmpty(_settings.ApiKey))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.ApiKey}");
            }
            _httpClient.DefaultRequestHeaders.Remove("X-Device-Id");
            _httpClient.DefaultRequestHeaders.Add("X-Device-Id", _settings.DeviceId);
        }

        public void UpdateSettings(ApiSettings settings)
        {
            _settings = settings;
            InitializeHttpClient();
        }

        public void UpdateApiKey(string apiKey)
        {
            _settings.ApiKey = apiKey;
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            if (!string.IsNullOrEmpty(apiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            }
        }

        public void UpdateBaseUrl(string baseUrl)
        {
            if (!string.IsNullOrEmpty(baseUrl))
            {
                _settings.BaseUrl = baseUrl.TrimEnd('/');
            }
        }

        public void UpdateFitnessCenterId(string fitnessCenterId)
        {
            _settings.FitnessCenterId = fitnessCenterId;
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                string url = $"{_settings.BaseUrl}/fitness-center/{_settings.FitnessCenterId}/members?page=1";
                var response = await _httpClient.GetAsync(url);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<(LoginResponse response, string error)> LoginAsync(string phoneNumber, string password)
        {
            try
            {
                using (var client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) })
                {
                    string url = "http://localhost:3000/login-fitness-center";
                    var request = new LoginRequest
                    {
                        PhoneNumber = phoneNumber,
                        Password = password
                    };

                    var json = JsonConvert.SerializeObject(request);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
                    var responseJson = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseJson);
                        return (loginResponse, null);
                    }

                    return (null, $"Login failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                return (null, $"Connection error: {ex.Message}");
            }
        }

        public bool IsInternetAvailable()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<FitAddisMember>> GetMembersAsync()
        {
            var allMembers = new List<FitAddisMember>();
            int currentPage = 1;
            int maxPages = 50; // Safety limit

            try
            {
                Log("Fetching members from Fit Addis API...");

                while (currentPage <= maxPages)
                {
                    string url = $"{_settings.BaseUrl}/fitness-center/{_settings.FitnessCenterId}/members?page={currentPage}";
                    Log($"Fetching page {currentPage}...");
                    
                    var response = await _httpClient.GetAsync(url);
                    
                    if (!response.IsSuccessStatusCode)
                    {
                        Log($"Failed to fetch page {currentPage}: {response.StatusCode}");
                        break;
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<FitAddisMembersResponse>(json);
                    
                    if (result?.Members == null || result.Members.Count == 0)
                    {
                        Log($"Page {currentPage}: no members, stopping.");
                        break;
                    }

                    allMembers.AddRange(result.Members);
                    Log($"Page {currentPage}: fetched {result.Members.Count} members");

                    // Check if there are more pages
                    bool hasNextPage = result.Pagination?.HasNextPage ?? false;
                    int totalPages = result.Pagination?.TotalPages ?? 1;
                    
                    if (!hasNextPage || currentPage >= totalPages)
                    {
                        break;
                    }
                    
                    currentPage++;
                }

                Log($"Total members fetched: {allMembers.Count}");
                return allMembers;
            }
            catch (Exception ex)
            {
                Log($"Error fetching members: {ex.Message}");
                return allMembers;
            }
        }

        public async Task<CheckInResponse> SendCheckInAsync(CheckInRequest request)
        {
            try
            {
                // Set fitnessCenterId from settings
                request.FitnessCenterId = int.Parse(_settings.FitnessCenterId);
                
                string url = $"{_settings.BaseUrl}/record-check-in";
                var json = JsonConvert.SerializeObject(request);
                
                Log($"Sending check-in: {json}");
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(url, content);
                var responseJson = await response.Content.ReadAsStringAsync();
                
                Log($"Response: {response.StatusCode} - {responseJson}");

                if (response.IsSuccessStatusCode)
                {
                    // API returns checkIn object on success, not a Success boolean
                    return new CheckInResponse { Success = true, Message = "Check-in recorded" };
                }

                // Parse error response
                string errorMessage = $"API Error: {response.StatusCode}";
                if (!string.IsNullOrEmpty(responseJson))
                {
                    errorMessage += $" - {responseJson}";
                }

                return new CheckInResponse
                {
                    Success = false,
                    Message = errorMessage
                };
            }
            catch (Exception ex)
            {
                Log($"Check-in exception: {ex.Message}");
                return new CheckInResponse
                {
                    Success = false,
                    Message = $"Connection Error: {ex.Message}"
                };
            }
        }

        public async Task<(int synced, int failed)> SyncCheckInsAsync(List<CheckInRecord> checkIns, Action<int, bool, string> onSynced)
        {
            int synced = 0;
            int failed = 0;

            foreach (var checkIn in checkIns)
            {
                var request = new CheckInRequest
                {
                    CheckInCode = checkIn.FitAddisMemberCode,
                    CheckInTime = checkIn.CheckInTime
                };

                var response = await SendCheckInAsync(request);
                
                if (response.Success)
                {
                    synced++;
                    onSynced?.Invoke(checkIn.Id, true, null);
                    Log($"Synced check-in for {checkIn.MemberName}");
                }
                else
                {
                    failed++;
                    onSynced?.Invoke(checkIn.Id, false, response.Message);
                    Log($"Failed to sync check-in for {checkIn.MemberName}: {response.Message}");
                }
            }

            return (synced, failed);
        }

        private void Log(string message)
        {
            OnLog?.Invoke(this, message);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
