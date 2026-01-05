using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using GymCheckIn.Models;

namespace GymCheckIn.Services
{
    public class DatabaseService : IDisposable
    {
        private readonly string _connectionString;
        private readonly string _dbPath;

        public DatabaseService(string dataFolder)
        {
            if (!Directory.Exists(dataFolder))
                Directory.CreateDirectory(dataFolder);

            _dbPath = Path.Combine(dataFolder, "gymcheckin.db");
            _connectionString = $"Data Source={_dbPath};Version=3;";
            
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                string createMembersTable = @"
                    CREATE TABLE IF NOT EXISTS Members (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FitAddisMemberCode TEXT UNIQUE NOT NULL,
                        Name TEXT,
                        Phone TEXT,
                        Email TEXT,
                        MembershipPlan TEXT,
                        MembershipExpiryDate TEXT,
                        FingerprintTemplate TEXT,
                        FingerprintTemplate10 TEXT,
                        FingerprintId INTEGER,
                        EnrolledDate TEXT
                    )";

                string createCheckInsTable = @"
                    CREATE TABLE IF NOT EXISTS CheckIns (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        FitAddisMemberCode TEXT NOT NULL,
                        MemberName TEXT,
                        CheckInTime TEXT NOT NULL,
                        Status TEXT,
                        IsSynced INTEGER DEFAULT 0,
                        SyncedAt TEXT,
                        SyncError TEXT
                    )";

                string createSettingsTable = @"
                    CREATE TABLE IF NOT EXISTS Settings (
                        Key TEXT PRIMARY KEY,
                        Value TEXT
                    )";

                using (var cmd = new SQLiteCommand(createMembersTable, conn))
                    cmd.ExecuteNonQuery();

                using (var cmd = new SQLiteCommand(createCheckInsTable, conn))
                    cmd.ExecuteNonQuery();

                using (var cmd = new SQLiteCommand(createSettingsTable, conn))
                    cmd.ExecuteNonQuery();

                // Migration: Add MembershipPlan column if it doesn't exist
                try
                {
                    using (var cmd = new SQLiteCommand("ALTER TABLE Members ADD COLUMN MembershipPlan TEXT", conn))
                        cmd.ExecuteNonQuery();
                }
                catch { /* Column already exists */ }
            }
        }

        #region Members

        public List<Member> GetAllMembers()
        {
            var members = new List<Member>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Members ORDER BY Name";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        members.Add(ReadMember(reader));
                    }
                }
            }
            return members;
        }

        public List<Member> GetEnrolledMembers()
        {
            var members = new List<Member>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Members WHERE FingerprintTemplate IS NOT NULL AND FingerprintTemplate != '' ORDER BY Name";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        members.Add(ReadMember(reader));
                    }
                }
            }
            return members;
        }

        public Member GetMemberByFingerprintId(int fingerprintId)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Members WHERE FingerprintId = @FingerprintId";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FingerprintId", fingerprintId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return ReadMember(reader);
                    }
                }
            }
            return null;
        }

        public Member GetMemberByCode(string memberCode)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM Members WHERE FitAddisMemberCode = @Code";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Code", memberCode);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            return ReadMember(reader);
                    }
                }
            }
            return null;
        }

        public void SaveMember(Member member)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    INSERT OR REPLACE INTO Members 
                    (Id, FitAddisMemberCode, Name, Phone, Email, MembershipPlan, MembershipExpiryDate, 
                     FingerprintTemplate, FingerprintTemplate10, FingerprintId, EnrolledDate)
                    VALUES 
                    (@Id, @Code, @Name, @Phone, @Email, @Plan, @Expiry, 
                     @Template, @Template10, @FingerprintId, @EnrolledDate)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", member.Id > 0 ? (object)member.Id : DBNull.Value);
                    cmd.Parameters.AddWithValue("@Code", member.FitAddisMemberCode);
                    cmd.Parameters.AddWithValue("@Name", member.Name ?? "");
                    cmd.Parameters.AddWithValue("@Phone", member.Phone ?? "");
                    cmd.Parameters.AddWithValue("@Email", member.Email ?? "");
                    cmd.Parameters.AddWithValue("@Plan", member.MembershipPlan ?? "");
                    cmd.Parameters.AddWithValue("@Expiry", member.MembershipExpiryDate?.ToString("o") ?? "");
                    cmd.Parameters.AddWithValue("@Template", member.FingerprintTemplate ?? "");
                    cmd.Parameters.AddWithValue("@Template10", member.FingerprintTemplate10 ?? "");
                    cmd.Parameters.AddWithValue("@FingerprintId", member.FingerprintId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EnrolledDate", member.EnrolledDate.ToString("o"));
                    cmd.ExecuteNonQuery();
                }

                if (member.Id == 0)
                {
                    member.Id = (int)conn.LastInsertRowId;
                }
            }
        }

        public void UpdateMemberFromApi(string memberCode, string name, string phone, string email, string membershipPlan, DateTime? expiryDate)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    UPDATE Members SET 
                        Name = @Name,
                        Phone = @Phone,
                        Email = @Email,
                        MembershipPlan = @Plan,
                        MembershipExpiryDate = @Expiry
                    WHERE FitAddisMemberCode = @Code";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name ?? "");
                    cmd.Parameters.AddWithValue("@Phone", phone ?? "");
                    cmd.Parameters.AddWithValue("@Email", email ?? "");
                    cmd.Parameters.AddWithValue("@Plan", membershipPlan ?? "");
                    cmd.Parameters.AddWithValue("@Expiry", expiryDate?.ToString("o") ?? "");
                    cmd.Parameters.AddWithValue("@Code", memberCode);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateMemberFingerprint(string memberCode, string template, string template10, int fingerprintId)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    UPDATE Members SET 
                        FingerprintTemplate = @Template,
                        FingerprintTemplate10 = @Template10,
                        FingerprintId = @FingerprintId,
                        EnrolledDate = @EnrolledDate
                    WHERE FitAddisMemberCode = @Code";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Template", template);
                    cmd.Parameters.AddWithValue("@Template10", template10);
                    cmd.Parameters.AddWithValue("@FingerprintId", fingerprintId);
                    cmd.Parameters.AddWithValue("@EnrolledDate", DateTime.Now.ToString("o"));
                    cmd.Parameters.AddWithValue("@Code", memberCode);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteMember(int memberId)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM Members WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", memberId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int GetNextFingerprintId()
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT COALESCE(MAX(FingerprintId), 0) + 1 FROM Members";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private Member ReadMember(SQLiteDataReader reader)
        {
            return new Member
            {
                Id = Convert.ToInt32(reader["Id"]),
                FitAddisMemberCode = reader["FitAddisMemberCode"].ToString(),
                Name = reader["Name"].ToString(),
                Phone = reader["Phone"].ToString(),
                Email = reader["Email"].ToString(),
                MembershipPlan = reader["MembershipPlan"]?.ToString() ?? "",
                MembershipExpiryDate = string.IsNullOrEmpty(reader["MembershipExpiryDate"].ToString()) 
                    ? (DateTime?)null 
                    : DateTime.Parse(reader["MembershipExpiryDate"].ToString()),
                FingerprintTemplate = reader["FingerprintTemplate"].ToString(),
                FingerprintTemplate10 = reader["FingerprintTemplate10"].ToString(),
                FingerprintId = reader["FingerprintId"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["FingerprintId"]),
                EnrolledDate = string.IsNullOrEmpty(reader["EnrolledDate"].ToString()) 
                    ? DateTime.Now 
                    : DateTime.Parse(reader["EnrolledDate"].ToString())
            };
        }

        #endregion

        #region CheckIns

        public void SaveCheckIn(CheckInRecord checkIn)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO CheckIns 
                    (FitAddisMemberCode, MemberName, CheckInTime, Status, IsSynced, SyncedAt, SyncError)
                    VALUES 
                    (@Code, @Name, @Time, @Status, @IsSynced, @SyncedAt, @SyncError)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Code", checkIn.FitAddisMemberCode);
                    cmd.Parameters.AddWithValue("@Name", checkIn.MemberName ?? "");
                    cmd.Parameters.AddWithValue("@Time", checkIn.CheckInTime.ToString("o"));
                    cmd.Parameters.AddWithValue("@Status", checkIn.Status ?? "");
                    cmd.Parameters.AddWithValue("@IsSynced", checkIn.IsSynced ? 1 : 0);
                    cmd.Parameters.AddWithValue("@SyncedAt", checkIn.SyncedAt?.ToString("o") ?? "");
                    cmd.Parameters.AddWithValue("@SyncError", checkIn.SyncError ?? "");
                    cmd.ExecuteNonQuery();
                }

                checkIn.Id = (int)conn.LastInsertRowId;
            }
        }

        public List<CheckInRecord> GetUnsyncedCheckIns()
        {
            var records = new List<CheckInRecord>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM CheckIns WHERE IsSynced = 0 ORDER BY CheckInTime";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        records.Add(ReadCheckIn(reader));
                    }
                }
            }
            return records;
        }

        public List<CheckInRecord> GetCheckIns(DateTime from, DateTime to)
        {
            var records = new List<CheckInRecord>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM CheckIns WHERE CheckInTime >= @From AND CheckInTime <= @To ORDER BY CheckInTime DESC";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@From", from.ToString("o"));
                    cmd.Parameters.AddWithValue("@To", to.ToString("o"));
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            records.Add(ReadCheckIn(reader));
                        }
                    }
                }
            }
            return records;
        }

        public List<CheckInRecord> GetAllCheckIns()
        {
            var records = new List<CheckInRecord>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT * FROM CheckIns ORDER BY CheckInTime DESC";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        records.Add(ReadCheckIn(reader));
                    }
                }
            }
            return records;
        }

        public void MarkCheckInSynced(int checkInId, bool success, string error = null)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    UPDATE CheckIns SET 
                        IsSynced = @IsSynced,
                        SyncedAt = @SyncedAt,
                        SyncError = @SyncError
                    WHERE Id = @Id";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@IsSynced", success ? 1 : 0);
                    cmd.Parameters.AddWithValue("@SyncedAt", success ? DateTime.Now.ToString("o") : "");
                    cmd.Parameters.AddWithValue("@SyncError", error ?? "");
                    cmd.Parameters.AddWithValue("@Id", checkInId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int GetUnsyncedCount()
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM CheckIns WHERE IsSynced = 0";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        private CheckInRecord ReadCheckIn(SQLiteDataReader reader)
        {
            return new CheckInRecord
            {
                Id = Convert.ToInt32(reader["Id"]),
                FitAddisMemberCode = reader["FitAddisMemberCode"].ToString(),
                MemberName = reader["MemberName"].ToString(),
                CheckInTime = DateTime.Parse(reader["CheckInTime"].ToString()),
                Status = reader["Status"].ToString(),
                IsSynced = Convert.ToInt32(reader["IsSynced"]) == 1,
                SyncedAt = string.IsNullOrEmpty(reader["SyncedAt"].ToString()) 
                    ? (DateTime?)null 
                    : DateTime.Parse(reader["SyncedAt"].ToString()),
                SyncError = reader["SyncError"].ToString()
            };
        }

        #endregion

        #region Settings

        public string GetSetting(string key, string defaultValue = "")
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Value FROM Settings WHERE Key = @Key";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Key", key);
                    var result = cmd.ExecuteScalar();
                    return result?.ToString() ?? defaultValue;
                }
            }
        }

        public void SaveSetting(string key, string value)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT OR REPLACE INTO Settings (Key, Value) VALUES (@Key, @Value)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Key", key);
                    cmd.Parameters.AddWithValue("@Value", value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        public void Dispose()
        {
            // Connection is created and disposed per operation
        }
    }
}
