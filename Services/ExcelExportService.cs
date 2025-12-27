using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GymCheckIn.Models;

namespace GymCheckIn.Services
{
    public class ExcelExportService
    {
        public string ExportCheckIns(List<CheckInRecord> checkIns, string filePath)
        {
            try
            {
                var sb = new StringBuilder();
                
                // CSV Header
                sb.AppendLine("ID,Member Code,Member Name,Check-In Time,Status,Synced,Synced At,Sync Error");

                foreach (var record in checkIns)
                {
                    sb.AppendLine($"{record.Id}," +
                        $"\"{record.FitAddisMemberCode}\"," +
                        $"\"{record.MemberName}\"," +
                        $"\"{record.CheckInTime:yyyy-MM-dd HH:mm:ss}\"," +
                        $"\"{record.Status}\"," +
                        $"\"{(record.IsSynced ? "Yes" : "No")}\"," +
                        $"\"{record.SyncedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? ""}\"," +
                        $"\"{record.SyncError ?? ""}\"");
                }

                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                return null; // Success
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ExportMembers(List<Member> members, string filePath)
        {
            try
            {
                var sb = new StringBuilder();
                
                // CSV Header
                sb.AppendLine("ID,Fit Addis Code,Name,Phone,Email,Expiry Date,Enrolled,Enrolled Date");

                foreach (var member in members)
                {
                    sb.AppendLine($"{member.Id}," +
                        $"\"{member.FitAddisMemberCode}\"," +
                        $"\"{member.Name}\"," +
                        $"\"{member.Phone}\"," +
                        $"\"{member.Email}\"," +
                        $"\"{member.MembershipExpiryDate?.ToString("yyyy-MM-dd") ?? ""}\"," +
                        $"\"{(member.IsEnrolled ? "Yes" : "No")}\"," +
                        $"\"{member.EnrolledDate:yyyy-MM-dd HH:mm:ss}\"");
                }

                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                return null; // Success
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
