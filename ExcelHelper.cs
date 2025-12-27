using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GymCheckIn
{
    public class ExcelHelper
    {
        private readonly string _membersFile;
        private readonly string _checkInsFile;

        public ExcelHelper(string dataFolder)
        {
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);

            _membersFile = Path.Combine(dataFolder, "Members.csv");
            _checkInsFile = Path.Combine(dataFolder, "CheckIns.csv");

            InitializeFiles();
        }

        private void InitializeFiles()
        {
            if (!File.Exists(_membersFile))
            {
                File.WriteAllText(_membersFile, "Id,Name,Phone,RegistrationDate,ExpiryDate,FingerprintTemplate,FingerprintTemplate10\n", Encoding.UTF8);
            }

            if (!File.Exists(_checkInsFile))
            {
                File.WriteAllText(_checkInsFile, "MemberId,MemberName,CheckInTime,Status\n", Encoding.UTF8);
            }
        }

        public int GetNextMemberId()
        {
            var members = LoadMembers();
            int maxId = 0;
            foreach (var m in members)
            {
                if (m.Id > maxId) maxId = m.Id;
            }
            return maxId + 1;
        }

        public void SaveMember(Member member)
        {
            var line = $"{member.Id},{EscapeCsv(member.Name)},{EscapeCsv(member.Phone)},{member.RegistrationDate:yyyy-MM-dd HH:mm:ss},{member.ExpiryDate:yyyy-MM-dd HH:mm:ss},{EscapeCsv(member.FingerprintTemplate)},{EscapeCsv(member.FingerprintTemplate10)}\n";
            File.AppendAllText(_membersFile, line, Encoding.UTF8);
        }

        public void UpdateMember(Member member)
        {
            var members = LoadMembers();
            for (int i = 0; i < members.Count; i++)
            {
                if (members[i].Id == member.Id)
                {
                    members[i] = member;
                    break;
                }
            }
            SaveAllMembers(members);
        }

        public void DeleteMember(int memberId)
        {
            var members = LoadMembers();
            members.RemoveAll(m => m.Id == memberId);
            SaveAllMembers(members);
        }

        private void SaveAllMembers(List<Member> members)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Id,Name,Phone,RegistrationDate,ExpiryDate,FingerprintTemplate,FingerprintTemplate10");
            foreach (var m in members)
            {
                sb.AppendLine($"{m.Id},{EscapeCsv(m.Name)},{EscapeCsv(m.Phone)},{m.RegistrationDate:yyyy-MM-dd HH:mm:ss},{m.ExpiryDate:yyyy-MM-dd HH:mm:ss},{EscapeCsv(m.FingerprintTemplate)},{EscapeCsv(m.FingerprintTemplate10)}");
            }
            File.WriteAllText(_membersFile, sb.ToString(), Encoding.UTF8);
        }

        public List<Member> LoadMembers()
        {
            var members = new List<Member>();
            if (!File.Exists(_membersFile)) return members;

            var lines = File.ReadAllLines(_membersFile, Encoding.UTF8);
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var parts = ParseCsvLine(lines[i]);
                if (parts.Length >= 7)
                {
                    try
                    {
                        var member = new Member
                        {
                            Id = int.Parse(parts[0]),
                            Name = parts[1],
                            Phone = parts[2],
                            RegistrationDate = DateTime.Parse(parts[3]),
                            ExpiryDate = DateTime.Parse(parts[4]),
                            FingerprintTemplate = parts[5],
                            FingerprintTemplate10 = parts[6]
                        };
                        members.Add(member);
                    }
                    catch { }
                }
            }
            return members;
        }

        public void LogCheckIn(Member member, string status)
        {
            var line = $"{member.Id},{EscapeCsv(member.Name)},{DateTime.Now:yyyy-MM-dd HH:mm:ss},{status}\n";
            File.AppendAllText(_checkInsFile, line, Encoding.UTF8);
        }

        public List<CheckInRecord> LoadCheckIns(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var records = new List<CheckInRecord>();
            if (!File.Exists(_checkInsFile)) return records;

            var lines = File.ReadAllLines(_checkInsFile, Encoding.UTF8);
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                var parts = ParseCsvLine(lines[i]);
                if (parts.Length >= 4)
                {
                    try
                    {
                        var checkInTime = DateTime.Parse(parts[2]);
                        
                        if (fromDate.HasValue && checkInTime < fromDate.Value) continue;
                        if (toDate.HasValue && checkInTime > toDate.Value) continue;

                        records.Add(new CheckInRecord
                        {
                            MemberId = int.Parse(parts[0]),
                            MemberName = parts[1],
                            CheckInTime = checkInTime,
                            Status = parts[3]
                        });
                    }
                    catch { }
                }
            }
            return records;
        }

        private string EscapeCsv(string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }
            return value;
        }

        private string[] ParseCsvLine(string line)
        {
            var result = new List<string>();
            var current = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }
            result.Add(current.ToString());
            return result.ToArray();
        }
    }

    public class CheckInRecord
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public DateTime CheckInTime { get; set; }
        public string Status { get; set; }
    }
}
