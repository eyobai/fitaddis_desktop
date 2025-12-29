using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using libzkfpcsharp;

namespace GymCheckIn.Services
{
    public class ZKFingerService : IDisposable
    {
        // Using the managed libzkfpcsharp wrapper (zkfp2 class)
        private IntPtr _deviceHandle = IntPtr.Zero;
        private IntPtr _dbHandle = IntPtr.Zero;
        private int _imageWidth = 0;
        private int _imageHeight = 0;
        private bool _isInitialized = false;
        private bool _isCapturing = false;
        private CancellationTokenSource _captureCts;

        // Events
        public event EventHandler<FingerprintCapturedEventArgs> OnFingerprintCaptured;
        public event EventHandler<EnrollmentCompleteEventArgs> OnEnrollmentComplete;
        public event EventHandler<string> OnLog;

        // Enrollment state
        private bool _isEnrolling = false;
        private int _enrollCount = 0;
        private byte[][] _enrollTemplates = new byte[3][];

        public bool IsConnected => _deviceHandle != IntPtr.Zero;
        public bool IsEnrolling => _isEnrolling;

        public int Initialize()
        {
            if (_isInitialized) return 0;

            try
            {
                int result = zkfp2.Init();
                if (result == 0 || result == 1) // 0 = OK, 1 = Already initialized
                {
                    _isInitialized = true;
                    Log("ZKFinger SDK initialized");
                    return 0;
                }

                Log($"Failed to initialize ZKFinger SDK. Error: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Log($"Initialize exception: {ex.Message}");
                return -1;
            }
        }

        public int GetDeviceCount()
        {
            try
            {
                return zkfp2.GetDeviceCount();
            }
            catch (Exception ex)
            {
                Log($"GetDeviceCount exception: {ex.Message}");
                return 0;
            }
        }

        public int OpenDevice(int index = 0)
        {
            if (_deviceHandle != IntPtr.Zero)
            {
                Log("Device already open");
                return 0;
            }

            try
            {
                _deviceHandle = zkfp2.OpenDevice(index);
                if (_deviceHandle == IntPtr.Zero)
                {
                    Log($"Failed to open device at index {index}");
                    return -6; // ZKFP_ERR_OPEN
                }

                // Get image dimensions
                byte[] paramValue = new byte[4];
                int size = 4;

                if (zkfp2.GetParameters(_deviceHandle, 1, paramValue, ref size) == 0)
                {
                    _imageWidth = BitConverter.ToInt32(paramValue, 0);
                }

                size = 4;
                if (zkfp2.GetParameters(_deviceHandle, 2, paramValue, ref size) == 0)
                {
                    _imageHeight = BitConverter.ToInt32(paramValue, 0);
                }

                // Initialize database for fingerprint matching
                _dbHandle = zkfp2.DBInit();
                if (_dbHandle == IntPtr.Zero)
                {
                    Log("Warning: Failed to initialize fingerprint database");
                }

                Log($"Device opened. Image size: {_imageWidth}x{_imageHeight}");
                return 0;
            }
            catch (Exception ex)
            {
                Log($"OpenDevice exception: {ex.Message}");
                return -6;
            }
        }

        public void CloseDevice()
        {
            try
            {
                StopCapture();

                if (_dbHandle != IntPtr.Zero)
                {
                    zkfp2.DBFree(_dbHandle);
                    _dbHandle = IntPtr.Zero;
                }

                if (_deviceHandle != IntPtr.Zero)
                {
                    zkfp2.CloseDevice(_deviceHandle);
                    _deviceHandle = IntPtr.Zero;
                }

                Log("Device closed");
            }
            catch (Exception ex)
            {
                Log($"CloseDevice exception: {ex.Message}");
            }
        }

        public void Terminate()
        {
            try
            {
                CloseDevice();
                if (_isInitialized)
                {
                    zkfp2.Terminate();
                    _isInitialized = false;
                }
            }
            catch (Exception ex)
            {
                Log($"Terminate exception: {ex.Message}");
            }
        }

        public void StartCapture()
        {
            if (_isCapturing) return;
            if (_deviceHandle == IntPtr.Zero)
            {
                Log("Cannot start capture - device not open");
                return;
            }

            _isCapturing = true;
            _captureCts = new CancellationTokenSource();

            Task.Run(() => CaptureLoop(_captureCts.Token));
            Log("Capture started");
        }

        public void StopCapture()
        {
            if (!_isCapturing) return;

            _isCapturing = false;
            _captureCts?.Cancel();
            Log("Capture stopped");
        }

        private void CaptureLoop(CancellationToken token)
        {
            try
            {
                // Ensure valid image dimensions
                int imgSize = _imageWidth * _imageHeight;
                if (imgSize <= 0)
                {
                    imgSize = 300 * 400; // Default size
                    Log($"Warning: Using default image buffer size {imgSize}");
                }

                byte[] imgBuffer = new byte[imgSize];
                byte[] template = new byte[2048];

                Log($"Capture loop started. Buffer size: {imgSize}");

                while (!token.IsCancellationRequested && _isCapturing)
                {
                    try
                    {
                        int templateLen = 2048;
                        int result = zkfp2.AcquireFingerprint(_deviceHandle, imgBuffer, template, ref templateLen);

                        if (result == 0 && templateLen > 0)
                        {
                            // Fingerprint captured successfully
                            byte[] capturedTemplate = new byte[templateLen];
                            Array.Copy(template, capturedTemplate, templateLen);

                            Log($"Fingerprint captured! Template size: {templateLen}");

                            // Create image from buffer
                            Bitmap image = CreateBitmapFromBuffer(imgBuffer, _imageWidth > 0 ? _imageWidth : 300, _imageHeight > 0 ? _imageHeight : 400);

                            if (_isEnrolling)
                            {
                                ProcessEnrollmentCapture(capturedTemplate);
                            }
                            else
                            {
                                // Raise capture event for check-in mode
                                OnFingerprintCaptured?.Invoke(this, new FingerprintCapturedEventArgs
                                {
                                    Template = capturedTemplate,
                                    Image = image
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log($"Capture error: {ex.Message}");
                    }

                    Thread.Sleep(200); // Small delay between capture attempts
                }
            }
            catch (Exception ex)
            {
                Log($"Capture loop error: {ex.Message}");
            }
            finally
            {
                Log("Capture loop ended");
            }
        }

        public void BeginEnroll()
        {
            if (_isEnrolling)
            {
                Log("Already enrolling");
                return;
            }

            _isEnrolling = true;
            _enrollCount = 0;
            _enrollTemplates = new byte[3][];
            Log("Enrollment started - scan finger 1 of 3");
        }

        public void CancelEnroll()
        {
            _isEnrolling = false;
            _enrollCount = 0;
            _enrollTemplates = new byte[3][];
            Log("Enrollment cancelled");
        }

        private void ProcessEnrollmentCapture(byte[] template)
        {
            _enrollTemplates[_enrollCount] = template;
            _enrollCount++;

            Log($"Enrollment capture {_enrollCount} of 3 complete");

            if (_enrollCount >= 3)
            {
                // Merge templates
                try
                {
                    byte[] regTemplate = new byte[2048];
                    int regTemplateLen = 2048;

                    int result = zkfp2.DBMerge(_dbHandle, _enrollTemplates[0], _enrollTemplates[1], 
                        _enrollTemplates[2], regTemplate, ref regTemplateLen);

                    _isEnrolling = false;

                    if (result == 0)
                    {
                        byte[] finalTemplate = new byte[regTemplateLen];
                        Array.Copy(regTemplate, finalTemplate, regTemplateLen);

                        OnEnrollmentComplete?.Invoke(this, new EnrollmentCompleteEventArgs
                        {
                            Success = true,
                            Template = finalTemplate
                        });
                        Log("Enrollment successful!");
                    }
                    else
                    {
                        OnEnrollmentComplete?.Invoke(this, new EnrollmentCompleteEventArgs
                        {
                            Success = false,
                            ErrorMessage = $"Failed to merge templates. Error: {result}"
                        });
                        Log($"Enrollment failed. Error: {result}");
                    }
                }
                catch (Exception ex)
                {
                    OnEnrollmentComplete?.Invoke(this, new EnrollmentCompleteEventArgs
                    {
                        Success = false,
                        ErrorMessage = ex.Message
                    });
                    Log($"Enrollment exception: {ex.Message}");
                }

                _enrollCount = 0;
                _enrollTemplates = new byte[3][];
            }
        }

        public int GetEnrollProgress()
        {
            return _enrollCount;
        }

        public int AddTemplateToDb(int fingerprintId, byte[] template)
        {
            if (_dbHandle == IntPtr.Zero) return -7;
            try
            {
                return zkfp2.DBAdd(_dbHandle, fingerprintId, template);
            }
            catch { return -7; }
        }

        public int RemoveTemplateFromDb(int fingerprintId)
        {
            if (_dbHandle == IntPtr.Zero) return -7;
            try
            {
                return zkfp2.DBDel(_dbHandle, fingerprintId);
            }
            catch { return -7; }
        }

        public int ClearDb()
        {
            if (_dbHandle == IntPtr.Zero) return -7;
            try
            {
                return zkfp2.DBClear(_dbHandle);
            }
            catch { return -7; }
        }

        public int Identify(byte[] template, out int fingerprintId, out int score)
        {
            fingerprintId = 0;
            score = 0;

            if (_dbHandle == IntPtr.Zero) return -7;
            try
            {
                return zkfp2.DBIdentify(_dbHandle, template, ref fingerprintId, ref score);
            }
            catch { return -7; }
        }

        private Bitmap CreateBitmapFromBuffer(byte[] buffer, int width, int height)
        {
            try
            {
                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

                // Set grayscale palette
                ColorPalette palette = bmp.Palette;
                for (int i = 0; i < 256; i++)
                {
                    palette.Entries[i] = Color.FromArgb(i, i, i);
                }
                bmp.Palette = palette;

                // Copy data
                BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, width, height),
                    ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

                int bytesToCopy = Math.Min(buffer.Length, width * height);
                Marshal.Copy(buffer, 0, bmpData.Scan0, bytesToCopy);
                bmp.UnlockBits(bmpData);

                return bmp;
            }
            catch
            {
                return null;
            }
        }

        public static string TemplateToBase64(byte[] template)
        {
            return Convert.ToBase64String(template);
        }

        public static byte[] Base64ToTemplate(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        private void Log(string message)
        {
            try
            {
                OnLog?.Invoke(this, message);
            }
            catch { }
        }

        public void Dispose()
        {
            Terminate();
        }
    }

    public class FingerprintCapturedEventArgs : EventArgs
    {
        public byte[] Template { get; set; }
        public Bitmap Image { get; set; }
    }

    public class EnrollmentCompleteEventArgs : EventArgs
    {
        public bool Success { get; set; }
        public byte[] Template { get; set; }
        public string ErrorMessage { get; set; }
    }
}
