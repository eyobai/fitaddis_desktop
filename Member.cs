using System;

namespace GymCheckIn
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string FingerprintTemplate { get; set; }
        public string FingerprintTemplate10 { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime? LastCheckIn { get; set; }

        public bool IsExpired => DateTime.Now > ExpiryDate;
        
        public int DaysRemaining => IsExpired ? 0 : (ExpiryDate - DateTime.Now).Days;

        public Member()
        {
            RegistrationDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Id} - {Name} (Expires: {ExpiryDate:dd/MM/yyyy})";
        }
    }
}
