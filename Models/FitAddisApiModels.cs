using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GymCheckIn.Models
{
    public class FitAddisMember
    {
        [JsonProperty("member_id")]
        public int MemberId { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("check_in_code")]
        public string CheckInCode { get; set; }

        [JsonProperty("membership_name")]
        public string MembershipName { get; set; }

        [JsonProperty("membership_expiry_date")]
        public DateTime? MembershipExpiryDate { get; set; }

        [JsonProperty("latest_billing_status")]
        public string LatestBillingStatus { get; set; }

        [JsonProperty("total_check_ins")]
        public string TotalCheckIns { get; set; }

        [JsonProperty("last_check_in_time")]
        public DateTime? LastCheckInTime { get; set; }

        public string MemberCode => CheckInCode;
    }

    public class FitAddisMembersResponse
    {
        [JsonProperty("members")]
        public List<FitAddisMember> Members { get; set; }

        [JsonProperty("pagination")]
        public PaginationInfo Pagination { get; set; }
    }

    public class PaginationInfo
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("totalMembers")]
        public int TotalMembers { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("hasNextPage")]
        public bool HasNextPage { get; set; }

        [JsonProperty("hasPrevPage")]
        public bool HasPrevPage { get; set; }
    }

    public class CheckInRequest
    {
        [JsonProperty("checkInCode")]
        public string CheckInCode { get; set; }

        [JsonProperty("fitnessCenterId")]
        public int FitnessCenterId { get; set; }

        [JsonProperty("checkInTime")]
        public DateTime? CheckInTime { get; set; }
    }

    public class CheckInResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string CheckInId { get; set; }
    }

    public class ApiSettings
    {
        public string BaseUrl { get; set; } = "https://fitaddis-app-53y6g.ondigitalocean.app";
        public string FitnessCenterId { get; set; } = "1";
        public string ApiKey { get; set; } = "";
        public string DeviceId { get; set; } = Environment.MachineName;
        public int SyncIntervalSeconds { get; set; } = 30;
    }

    public class LoginRequest
    {
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        [JsonProperty("fitnessCenter")]
        public FitnessCenterInfo FitnessCenter { get; set; }

        [JsonProperty("modules")]
        public List<object> Modules { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }

    public class FitnessCenterInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("specific_location")]
        public string SpecificLocation { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("membership_expiry_start")]
        public string MembershipExpiryStart { get; set; }
    }
}
