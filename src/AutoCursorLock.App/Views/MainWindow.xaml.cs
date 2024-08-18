// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Views
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
    using AutoCursorLock.App.Extensions;
    using AutoCursorLock.App.Models;
    using AutoCursorLock.App.Services;
    using AutoCursorLock.Models;
    using AutoCursorLock.Native;
    using AutoCursorLock.Sdk.Models;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ApplicationHandler applicationHandler;

        private bool globalLockEnabled = true;
        private bool applicationLockEnabled = false;
        private WindowInteropHelper windowInteropHelper;

        private Key selectedKey;

        private readonly SaveUserSettingsOperation saveUserSettingsOperation;
        private readonly ILogger<MainWindow> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow(
            ApplicationHandler applicationHandler,
            SaveUserSettingsOperation saveUserSettingsOperation,
            UserSettings userSettings,
            ILogger<MainWindow> logger)
        {
            this.applicationHandler = applicationHandler;
            this.saveUserSettingsOperation = saveUserSettingsOperation;
            this.logger = logger;
            InitializeComponent();

            NotifyPropertyChanged(nameof(UserSettings));

            this.mainGrid.DataContext = this;

            MinimizeToTray.Enable(this);

            this.windowInteropHelper = new WindowInteropHelper(this);
            UserSettings = userSettings;
        }

        private IntPtr Hwnd => this.windowInteropHelper.Handle;

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

        protected override async void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (UserSettings.HotKey != null)
            {
                KeyHandler.Unregister(UserSettings.HotKey, Hwnd);
            }

            MouseHandler.UnlockCursor();

            var userSettingsModel = UserSettings.ToModel();

            await this.saveUserSettingsOperation.InvokeAsync(userSettingsModel);
        }

        private void HandleHotkey()
        {
            this.logger.LogDebug("Hotkey pressed");

            GlobalLockEnabled = !GlobalLockEnabled;

            this.applicationHandler?.Update();
        }

        private void OnApplicationChanged(object? sender, ApplicationEventArgs e)
        {
            this.logger.LogDebug("Application changed: {ProcessName}", e.ProcessName);

            ApplicationLockEnabled = UserSettings.EnabledProcesses.Any(x => x.Name == e.ProcessName);

            if (ApplicationLockEnabled)
            {
                this.logger.LogInformation("Locking cursor for {ProcessName}", e.ProcessName);
            }

            AdjustLock(e.Handle);
        }

        private void AdjustLock(IntPtr hwnd)
        {
            if (GlobalLockEnabled && ApplicationLockEnabled)
            {
                if (!MouseHandler.LockCursor(hwnd))
                {
                    this.logger.LogError("Lock cursor error code {ErrorCode}", Marshal.GetLastWin32Error());
                }
                else
                {
                    this.logger.LogDebug("Cursor locked");
                }
            }
            else
            {
                if (!MouseHandler.UnlockCursor())
                {
                    this.logger.LogError("Unlock cursor error code {ErrorCode}", Marshal.GetLastWin32Error());
                }
                else
                {
                    this.logger.LogDebug("Cursor unlocked");
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
                var success = KeyHandler.Register(UserSettings.HotKey, Hwnd);
                this.logger.LogDebug("Register hotkey hook: {Success}", success);

                if (!success)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    this.logger.LogError("Error registering hotkey: {ErrorCode}", errorCode);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterHotKey();

            var success = this.applicationHandler.Register();
            this.logger.LogDebug("Register application hook: {Success}", success);

            if (!success)
            {
                var errorCode = Marshal.GetLastWin32Error();
                this.logger.LogError("Application hook error code {ErrorCode}", errorCode);
            }

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
                        var process = ProcessListItemExtensions.FromPath(p.MainModule.FileName);
                        Processes.Add(process);
                    }
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "Failed to get process list item for process: {ProcessName}", p.ProcessName);
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
                var modifierKeys = new ModifierKey[2];
                if (this.cmbModifierKey.SelectedValue != null)
                {
                    modifierKeys[0] = (ModifierKey)this.cmbModifierKey.SelectedValue;
                }

                if (this.cmbModifier2Key.SelectedValue != null)
                {
                    modifierKeys[1] = (ModifierKey)this.cmbModifier2Key.SelectedValue;
                }

                UserSettings.HotKey = new HotKey(0, modifierKeys, this.selectedKey.ToVirtualKey());

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
