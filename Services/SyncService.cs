using System;
using System.Threading;
using System.Threading.Tasks;
using GymCheckIn.Models;

namespace GymCheckIn.Services
{
    public class SyncService : IDisposable
    {
        private readonly DatabaseService _db;
        private readonly FitAddisApiService _api;
        private readonly ApiSettings _settings;
        private Timer _syncTimer;
        private bool _isSyncing;
        private bool _isOnline;

        public event EventHandler<SyncEventArgs> OnSyncStatusChanged;
        public event EventHandler<string> OnLog;
        public event EventHandler<bool> OnConnectionStatusChanged;

        public bool IsOnline => _isOnline;
        public bool IsSyncing => _isSyncing;

        public SyncService(DatabaseService db, FitAddisApiService api, ApiSettings settings)
        {
            _db = db;
            _api = api;
            _settings = settings;
            _api.OnLog += (s, msg) => Log(msg);
        }

        public void Start()
        {
            int intervalMs = _settings.SyncIntervalSeconds * 1000;
            _syncTimer = new Timer(async _ => await CheckAndSyncAsync(), null, 0, intervalMs);
            Log($"Sync service started (interval: {_settings.SyncIntervalSeconds}s)");
        }

        public void Stop()
        {
            _syncTimer?.Dispose();
            _syncTimer = null;
            Log("Sync service stopped");
        }

        public async Task CheckAndSyncAsync()
        {
            if (_isSyncing) return;

            try
            {
                bool wasOnline = _isOnline;
                _isOnline = _api.IsInternetAvailable();

                if (wasOnline != _isOnline)
                {
                    OnConnectionStatusChanged?.Invoke(this, _isOnline);
                    Log(_isOnline ? "Internet connection restored" : "Internet connection lost");
                }

                if (!_isOnline) return;

                int unsyncedCount = _db.GetUnsyncedCount();
                if (unsyncedCount == 0) return;

                _isSyncing = true;
                OnSyncStatusChanged?.Invoke(this, new SyncEventArgs 
                { 
                    Status = SyncStatus.Syncing, 
                    Message = $"Syncing {unsyncedCount} check-ins..." 
                });

                var unsyncedRecords = _db.GetUnsyncedCheckIns();
                var (synced, failed) = await _api.SyncCheckInsAsync(unsyncedRecords, (id, success, error) =>
                {
                    _db.MarkCheckInSynced(id, success, error);
                });

                string message = $"Sync complete: {synced} synced, {failed} failed";
                Log(message);

                OnSyncStatusChanged?.Invoke(this, new SyncEventArgs
                {
                    Status = failed == 0 ? SyncStatus.Success : SyncStatus.PartialSuccess,
                    Message = message,
                    SyncedCount = synced,
                    FailedCount = failed
                });
            }
            catch (Exception ex)
            {
                Log($"Sync error: {ex.Message}");
                OnSyncStatusChanged?.Invoke(this, new SyncEventArgs
                {
                    Status = SyncStatus.Error,
                    Message = $"Sync error: {ex.Message}"
                });
            }
            finally
            {
                _isSyncing = false;
            }
        }

        public async Task ForceSyncAsync()
        {
            if (_isSyncing)
            {
                Log("Sync already in progress");
                return;
            }

            Log("Manual sync triggered");
            await CheckAndSyncAsync();
        }

        private void Log(string message)
        {
            OnLog?.Invoke(this, $"[Sync] {message}");
        }

        public void Dispose()
        {
            Stop();
        }
    }

    public class SyncEventArgs : EventArgs
    {
        public SyncStatus Status { get; set; }
        public string Message { get; set; }
        public int SyncedCount { get; set; }
        public int FailedCount { get; set; }
    }

    public enum SyncStatus
    {
        Idle,
        Syncing,
        Success,
        PartialSuccess,
        Error
    }
}
