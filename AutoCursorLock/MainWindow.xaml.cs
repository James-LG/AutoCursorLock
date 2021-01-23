using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
        private ApplicationHandler? applicationHandler;

        private bool globalLockEnabled = true;
        private bool applicationLockEnabled = false;

        private System.Windows.Forms.NotifyIcon notifyIcon1;

        public MainWindow()
        {
            InitializeComponent();

            UserSettings = UserSettings.Load();

            mainGrid.DataContext = this;

            var processes = Process.GetProcesses();
            foreach (var p in processes)
            {
                if (p.MainWindowHandle != IntPtr.Zero && p.Id != Process.GetCurrentProcess().Id)
                {
                    try
                    {
                        this.Processes.Add(new ProcessListItem(p.ProcessName, p.MainModule.FileName));
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("Get process failed: " + ex);
                    }
                }
            }

            //var components = new System.ComponentModel.Container();
            //this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);

            //notifyIcon1.DoubleClick += new System.EventHandler(notifyIcon1_DoubleClick);
        }
        public UserSettings UserSettings { get; }

        private void notifyIcon1_DoubleClick()
        {

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

            UserSettings.Save();
        }

        private void HandleHotkey()
        {
            Trace.WriteLine("Key pressed");

            globalLockEnabled = !globalLockEnabled;

            applicationHandler?.Update();
        }

        private void OnApplicationChanged(object? sender, ApplicationEventArgs e)
        {
            applicationLockEnabled = UserSettings.EnabledProcesses.Any(x => x.Name == e.ProcessName);

            AdjustLock(e.Handle);
        }

        private void AdjustLock(IntPtr hwnd)
        {
            if (globalLockEnabled && applicationLockEnabled)
            {
                if (!MouseHandler.LockCursor(hwnd))
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

            applicationHandler = new ApplicationHandler();
            success = applicationHandler.Register();
            Trace.WriteLine("Register application hook: " + success);
            applicationHandler.ApplicationChanged += OnApplicationChanged;

            applicationHandler.Update();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var processItem = (ProcessListItem)enabledProcessList.SelectedItem;
            UserSettings.EnabledProcesses.Remove(processItem);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var processItem = (ProcessListItem)processList.SelectedItem;
            UserSettings.EnabledProcesses.Add(processItem);
        }
    }
}
