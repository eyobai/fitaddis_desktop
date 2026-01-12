using System;

namespace GymCheckIn.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string FitAddisMemberCode { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string MembershipPlan { get; set; }
        public DateTime? MembershipExpiryDate { get; set; }
        public string MembershipStatus { get; set; }  // active, expiring_soon, expired, never_paid
        public string FingerprintTemplate { get; set; }
        public string FingerprintTemplate10 { get; set; }
        public int? FingerprintId { get; set; }
        public DateTime EnrolledDate { get; set; }
        public bool IsEnrolled => !string.IsNullOrEmpty(FingerprintTemplate);
        public bool IsExpired => MembershipExpiryDate.HasValue && MembershipExpiryDate.Value.Date < DateTime.Now.Date;
        public int DaysRemaining => MembershipExpiryDate.HasValue 
            ? Math.Max(0, (int)(MembershipExpiryDate.Value.Date - DateTime.Now.Date).TotalDays) 
            : 0;

        public override string ToString()
        {
            string status = IsEnrolled ? "[Enrolled]" : "[Not Enrolled]";
            return $"{Name} - {FitAddisMemberCode} {status}";
        }
    }
}
