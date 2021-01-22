using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace AutoCursorLock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ProcessListItem> Processes { get; } = new ObservableCollection<ProcessListItem>();

        private KeyHandler? keyHandler;
        private MouseHandler? mouseHandler;
        private ApplicationHandler? applicationHandler;

        private bool globalLockEnabled = true;
        private bool applicationLockEnabled = false;

        public MainWindow()
        {
            InitializeComponent();

            listBox1.DataContext = this;

            var processes = Process.GetProcesses();
            foreach (var p in processes)
            {
                if (p.MainWindowHandle != IntPtr.Zero && p.Id != Process.GetCurrentProcess().Id)
                {
                    try
                    {
                        using var ico = System.Drawing.Icon.ExtractAssociatedIcon(p.MainModule.FileName);
                        var iconSource = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        this.Processes.Add(new ProcessListItem(p.ProcessName, new Uri(p.MainModule.FileName), iconSource));
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("Get process failed: " + ex);
                    }
                }
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var source = (HwndSource)PresentationSource.FromVisual(this);
            source.AddHook(WndProc);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            keyHandler?.Unregister();
            MouseHandler.UnlockCursor();
        }

        private void HandleHotkey()
        {
            Trace.WriteLine("Key pressed");

            globalLockEnabled = !globalLockEnabled;

            AdjustLock();
        }

        private void OnApplicationChanged(object? sender, ApplicationEventArgs e)
        {
            applicationLockEnabled = e.ProcessName == "AutoCursorLock";

            AdjustLock();
        }

        private void AdjustLock()
        {
            if (mouseHandler != null && globalLockEnabled && applicationLockEnabled)
            {
                if (!mouseHandler.LockCursor())
                {
                    Trace.WriteLine("Lock cursor error code " + Marshal.GetLastWin32Error());
                }
                else
                {
                    Trace.WriteLine("Cursor locked");
                }
            }
            else
            {
                if (!MouseHandler.UnlockCursor())
                {
                    Trace.WriteLine("Unlock cursor error code " + Marshal.GetLastWin32Error());
                }
                else
                {
                    Trace.WriteLine("Cursor unlocked");
                }
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeMethods.WM_HOTKEY_MSG_ID)
                HandleHotkey();

            return IntPtr.Zero;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hotkey = new HotKey(0, Key.Home)
            {
                WithShift = true
            };

            var hWnd = new WindowInteropHelper(this).Handle;
            keyHandler = new KeyHandler(hotkey, hWnd);
            var success = keyHandler.Register();
            Trace.WriteLine("Register hotkey hook: " + success);
            if (!success)
            {
                var errorCode = Marshal.GetLastWin32Error();
                Trace.WriteLine("Hotkey error code " + errorCode);
            }

            mouseHandler = new MouseHandler(hWnd);
            applicationHandler = new ApplicationHandler();
            success = applicationHandler.Register();
            Trace.WriteLine("Register application hook: " + success);
            applicationHandler.ApplicationChanged += OnApplicationChanged;

            applicationHandler.Update();
        }
    }
}
