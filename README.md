# Fit Addis Gym Check-In System

A Windows desktop application for managing gym check-ins using ZKTeco ZK950 USB fingerprint reader with offline-first architecture and automatic cloud sync to Fit Addis API.

## Features

### Member Enrollment
- Fetch member list from Fit Addis API (`GET /members`)
- Select member from dropdown and enroll their fingerprint via ZKTeco SDK
- Local storage (SQLite) maps: `fingerprint_id → fitaddis_member_code`
- Stores: name, phone, membership expiry date

### Offline Check-In
- Member scans fingerprint
- ZKTeco SDK identifies fingerprint → returns fingerprint ID
- Lookup local mapping → get Fit Addis member code
- Check membership expiry → allow or deny check-in
- Save check-in locally with: `member_code, timestamp, synced_status`
- **Works without internet**

### Auto-Sync
- Periodically checks internet availability (every 30 seconds)
- Sends all unsynced check-ins to Fit Addis API (`POST /checkin`)
- Marks synced check-ins locally
- Visual status indicators for online/offline state

### Reporting
- View check-in logs by date range
- Export all check-ins to CSV
- Export member list to CSV

## Technical Stack

- **Framework:** C# WinForms (.NET Framework 4.7.2)
- **Fingerprint SDK:** ZKTeco ZKFPEngX ActiveX Control
- **Local Database:** SQLite
- **HTTP Client:** System.Net.Http with Newtonsoft.Json
- **Target Platform:** x86 (required for ZKTeco SDK)

## Prerequisites

1. **ZKTeco SDK:** Install ZKFinger SDK (includes ZKFPEngX.dll ActiveX control)
2. **ZKTeco ZK950:** USB fingerprint reader connected
3. **.NET Framework 4.7.2** or later
4. **NuGet Packages:** (restored automatically)
   - Newtonsoft.Json 13.0.3
   - System.Data.SQLite.Core 1.0.118.0

## Installation

1. Clone or download this project
2. Open `GymCheckIn.sln` in Visual Studio
3. Restore NuGet packages:
   ```
   nuget restore GymCheckIn.sln
   ```
4. Build the solution (x86 platform)
5. Run the application

## Configuration

### API Settings (Settings Tab)
- **API Base URL:** Your Fit Addis API endpoint (e.g., `https://api.fitaddis.com`)
- **API Key:** Your API authentication key

Settings are saved locally in the SQLite database.

## Usage

### 1. Connect Fingerprint Sensor
- Go to **Check-In** tab
- Click **Connect** to initialize the ZKTeco sensor
- Status will show "CONNECTED" when ready

### 2. Enroll Members
- Go to **Enrollment** tab
- Click **Fetch Members from Fit Addis** to load member list
- Select a member from the dropdown
- Click **Enroll Selected Member's Fingerprint**
- Have member scan finger 3 times

### 3. Check-In Members
- Go to **Check-In** tab
- Members scan their fingerprint on the sensor
- System displays:
  - ✅ **Green:** CHECK-IN SUCCESS (valid membership)
  - 🔴 **Red:** MEMBERSHIP EXPIRED
  - 🟠 **Orange:** NOT RECOGNIZED

### 4. View & Export Logs
- Go to **Check-In Logs** tab
- Select date range and click **View**
- Click **Export All to CSV** for full export

## Data Storage

All data is stored locally in `Data/gymcheckin.db` (SQLite):

| Table | Purpose |
|-------|---------|
| Members | Enrolled members with fingerprint templates |
| CheckIns | All check-in records with sync status |
| Settings | API configuration |

## Security

- **Fingerprint data never leaves local PC** - only member codes are sent to API
- API key stored locally (consider additional encryption for production)
- All check-ins logged locally even when offline

## API Endpoints Expected

| Method | Endpoint | Purpose |
|--------|----------|---------|
| GET | `/api/members` | Fetch member list |
| POST | `/api/checkin` | Submit check-in record |
| GET | `/api/health` | Test API connectivity |

### Expected Response Formats

**GET /api/members:**
```json
{
  "success": true,
  "data": [
    {
      "memberCode": "FIT001",
      "firstName": "John",
      "lastName": "Doe",
      "phone": "+251912345678",
      "email": "john@example.com",
      "membershipExpiry": "2024-12-31T23:59:59",
      "status": "active"
    }
  ]
}
```

**POST /api/checkin:**
```json
{
  "memberCode": "FIT001",
  "checkInTime": "2024-01-15T10:30:00",
  "deviceId": "DESKTOP-ABC123"
}
```

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Sensor not detected | Ensure ZKTeco SDK is installed and device connected |
| COM error on startup | Run as Administrator or re-register ZKFPEngX.dll |
| API fetch fails | Check API URL and key in Settings tab |
| Sync not working | Check internet connection and API endpoint |

## License

Internal use for Fit Addis gyms.
