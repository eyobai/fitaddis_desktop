using System;

namespace GymCheckIn.Models
{
    public class CheckInRecord
    {
        public int Id { get; set; }
        public string FitAddisMemberCode { get; set; }
        public string MemberName { get; set; }
        public DateTime CheckInTime { get; set; }
        public string Status { get; set; } // OK, EXPIRED, DENIED
        public bool IsSynced { get; set; }
        public DateTime? SyncedAt { get; set; }
        public string SyncError { get; set; }

        public string SyncStatus => IsSynced ? "Synced" : "Pending";
    }
}
