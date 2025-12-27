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
        public DateTime? MembershipExpiryDate { get; set; }
        public string FingerprintTemplate { get; set; }
        public string FingerprintTemplate10 { get; set; }
        public int? FingerprintId { get; set; }
        public DateTime EnrolledDate { get; set; }
        public bool IsEnrolled => !string.IsNullOrEmpty(FingerprintTemplate);
        public bool IsExpired => MembershipExpiryDate.HasValue && DateTime.Now > MembershipExpiryDate.Value;
        public int DaysRemaining => MembershipExpiryDate.HasValue 
            ? Math.Max(0, (MembershipExpiryDate.Value - DateTime.Now).Days) 
            : 0;

        public override string ToString()
        {
            string status = IsEnrolled ? "[Enrolled]" : "[Not Enrolled]";
            return $"{Name} - {FitAddisMemberCode} {status}";
        }
    }
}
