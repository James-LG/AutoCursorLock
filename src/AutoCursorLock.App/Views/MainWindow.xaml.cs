// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interop;
    using AutoCursorLock.Models;
    using AutoCursorLock.Native;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly IntPtr hWnd;
        private ApplicationHandler? applicationHandler;

        private bool globalLockEnabled = true;
        private bool applicationLockEnabled = false;

        private Key selectedKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            UserSettings = UserSettings.Load();
            NotifyPropertyChanged(nameof(UserSettings));

            this.mainGrid.DataContext = this;

            MinimizeToTray.Enable(this);

            this.hWnd = new WindowInteropHelper(this).Handle;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<ProcessListItem> Processes { get; } = new ObservableCollection<ProcessListItem>();

        public UserSettings UserSettings { get; }

        public Key SelectedKey
        {
            get
            {
                return this.selectedKey;
            }

            set
            {
                this.selectedKey = value;
                NotifyPropertyChanged(nameof(SelectedKey));
            }
        }

        public bool GlobalLockEnabled
        {
            get
            {
                return this.globalLockEnabled;
            }

            set
            {
                this.globalLockEnabled = value;
                NotifyPropertyChanged(nameof(GlobalLockEnabled));
            }
        }

        public bool ApplicationLockEnabled
        {
            get
            {
                return this.applicationLockEnabled;
            }

            set
            {
                this.applicationLockEnabled = value;
                NotifyPropertyChanged(nameof(ApplicationLockEnabled));
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
            if (UserSettings.HotKey != null)
            {
                KeyHandler.Unregister(UserSettings.HotKey, this.hWnd);
            }

            MouseHandler.UnlockCursor();

            UserSettings.Save();
        }

        private void HandleHotkey()
        {
            Trace.WriteLine("Key pressed");

            GlobalLockEnabled = !GlobalLockEnabled;

            this.applicationHandler?.Update();
        }

        private void OnApplicationChanged(object? sender, ApplicationEventArgs e)
        {
            ApplicationLockEnabled = UserSettings.EnabledProcesses.Any(x => x.Name == e.ProcessName);

            AdjustLock(e.Handle);
        }

        private void AdjustLock(IntPtr hwnd)
        {
            if (GlobalLockEnabled && ApplicationLockEnabled)
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

        private void RegisterHotKey()
        {
            if (UserSettings.HotKey != null)
            {
                var success = KeyHandler.Register(UserSettings.HotKey, this.hWnd);
                Trace.WriteLine("Register hotkey hook: " + success);

                if (!success)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    Trace.WriteLine("Hotkey error code " + errorCode);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterHotKey();

            this.applicationHandler = new ApplicationHandler();
            var success = this.applicationHandler.Register();
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

            if (processItem != null)
            {
                UserSettings.EnabledProcesses.Add(processItem);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Processes.Clear();
            var processes = Process.GetProcesses();
            foreach (var p in processes)
            {
                try
                {
                    if (p.MainWindowHandle != IntPtr.Zero && p.MainModule?.FileName != null)
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

        private void AboutItem_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow();
            about.ShowDialog();
        }

        private void BtnRegisterHotKey_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedKey != Key.None)
            {
                ModifierKey modifierKey = ModifierKey.None;
                if (this.cmbModifierKey.SelectedValue != null)
                {
                    modifierKey = (ModifierKey)this.cmbModifierKey.SelectedValue;
                }

                UserSettings.HotKey = new HotKey(0, this.selectedKey, modifierKey);

                RegisterHotKey();
            }
        }

        private void BtnSetKey_Click(object sender, RoutedEventArgs e)
        {
            var keyDialog = new SelectKeyDialog();
            keyDialog.ShowDialog();

            SelectedKey = keyDialog.SelectedKey;
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
