// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        private KeyHandler? keyHandler;
        private ApplicationHandler? applicationHandler;

        private bool globalLockEnabled = true;
        private bool applicationLockEnabled = false;

        public MainWindow()
        {
            InitializeComponent();

            UserSettings = UserSettings.Load();

            this.mainGrid.DataContext = this;

            MinimizeToTray.Enable(this);
        }

        public ObservableCollection<ProcessListItem> Processes { get; } = new ObservableCollection<ProcessListItem>();

        public UserSettings UserSettings { get; }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var source = (HwndSource)PresentationSource.FromVisual(this);
            source.AddHook(WndProc);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            this.keyHandler?.Unregister();
            MouseHandler.UnlockCursor();

            UserSettings.Save();
        }

        private void HandleHotkey()
        {
            Trace.WriteLine("Key pressed");

            this.globalLockEnabled = !this.globalLockEnabled;

            this.applicationHandler?.Update();
        }

        private void OnApplicationChanged(object? sender, ApplicationEventArgs e)
        {
            this.applicationLockEnabled = UserSettings.EnabledProcesses.Any(x => x.Name == e.ProcessName);

            AdjustLock(e.Handle);
        }

        private void AdjustLock(IntPtr hwnd)
        {
            if (this.globalLockEnabled && this.applicationLockEnabled)
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
            {
                HandleHotkey();
            }

            return IntPtr.Zero;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hotkey = new HotKey(0, Key.Home)
            {
                WithShift = true,
            };

            var hWnd = new WindowInteropHelper(this).Handle;
            this.keyHandler = new KeyHandler(hotkey, hWnd);
            var success = this.keyHandler.Register();
            Trace.WriteLine("Register hotkey hook: " + success);
            if (!success)
            {
                var errorCode = Marshal.GetLastWin32Error();
                Trace.WriteLine("Hotkey error code " + errorCode);
            }

            this.applicationHandler = new ApplicationHandler();
            success = this.applicationHandler.Register();
            Trace.WriteLine("Register application hook: " + success);
            this.applicationHandler.ApplicationChanged += OnApplicationChanged;

            this.applicationHandler.Update();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var processItem = (ProcessListItem)this.enabledProcessList.SelectedItem;
            UserSettings.EnabledProcesses.Remove(processItem);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var processItem = (ProcessListItem)this.processList.SelectedItem;
            UserSettings.EnabledProcesses.Add(processItem);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            var processes = Process.GetProcesses();
            foreach (var p in processes)
            {
                try
                {
                    if (p.MainWindowHandle != IntPtr.Zero && p.Id != Environment.ProcessId && p.MainModule?.FileName != null)
                    {
                        Processes.Add(new ProcessListItem(p.ProcessName, p.MainModule.FileName));
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Get process failed: " + ex);
                }
            }
        }
    }
}
