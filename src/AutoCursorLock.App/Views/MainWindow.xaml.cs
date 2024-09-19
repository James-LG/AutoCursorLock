// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock.App.Views;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using AutoCursorLock.App.Extensions;
using AutoCursorLock.App.Models;
using AutoCursorLock.App.Services;
using AutoCursorLock.Native;
using AutoCursorLock.Sdk.Models;
using Microsoft.Extensions.Logging;

/// <summary>
/// Interaction logic for MainWindow.xaml.
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private readonly WindowInteropHelper windowInteropHelper;
    private readonly SaveUserSettingsOperation saveUserSettingsOperation;
    private readonly ILogger<MainWindow> logger;

    private ApplicationEventSource applicationEventSource;

    private bool globalLockEnabled = true;
    private bool applicationLockEnabled = false;
    private ProcessListItem? activeProcess;
    private Key selectedKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    /// <param name="applicationEventSource">The application event source.</param>
    /// <param name="saveUserSettingsOperation">The save user settings operation.</param>
    /// <param name="userSettings">The user settings.</param>
    /// <param name="logger">The logger.</param>
    public MainWindow(
        ApplicationEventSource applicationEventSource,
        SaveUserSettingsOperation saveUserSettingsOperation,
        UserSettings userSettings,
        ILogger<MainWindow> logger)
    {
        this.applicationEventSource = applicationEventSource;
        this.saveUserSettingsOperation = saveUserSettingsOperation;
        this.logger = logger;
        InitializeComponent();

        NotifyPropertyChanged(nameof(UserSettings));

        this.mainGrid.DataContext = this;

        MinimizeToTray.Enable(this);

        this.windowInteropHelper = new WindowInteropHelper(this);
        UserSettings = userSettings;
    }

    /// <summary>
    /// Event for property changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets the list of processes.
    /// </summary>
    public ObservableCollection<ProcessListItem> Processes { get; } = new ObservableCollection<ProcessListItem>();

    /// <summary>
    /// Gets the user settings.
    /// </summary>
    public UserSettings UserSettings { get; }

    /// <summary>
    /// Gets or sets the selected key.
    /// </summary>
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

    /// <summary>
    /// Gets or sets a value indicating whether global lock is enabled.
    /// </summary>
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

    /// <summary>
    /// Gets or sets a value indicating whether application lock is enabled.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the currently in focus process, if it is an enabled process.
    /// </summary>
    /// <remarks>
    /// Does not keep track of the process if it is not a process enabled by the user.
    /// </remarks>
    public ProcessListItem? ActiveProcess
    {
        get
        {
            return this.activeProcess;
        }

        set
        {
            this.activeProcess = value;
            ApplicationLockEnabled = value != null;
            NotifyPropertyChanged(nameof(ActiveProcess));
        }
    }

    private IntPtr Hwnd => this.windowInteropHelper.Handle;

    /// <summary>
    /// Gets the list of enabled processes.
    /// </summary>
    /// <param name="e">Event args.</param>
    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        var source = (HwndSource)PresentationSource.FromVisual(this);
        source.AddHook(WndProc);
    }

    /// <summary>
    /// Event for when the window is closed.
    /// </summary>
    /// <param name="e">Event args.</param>
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

        this.applicationEventSource?.Update();
    }

    private void OnApplicationChanged(object? sender, ApplicationEventArgs e)
    {
        this.logger.LogDebug("Application changed: {ProcessName}", e.ProcessName);

        ActiveProcess = UserSettings.EnabledProcesses.FirstOrDefault(x => x.Name == e.ProcessName);

        AdjustLock(e.Handle);
    }

    private void AdjustLock(IntPtr hwnd)
    {
        if (GlobalLockEnabled && ActiveProcess is not null)
        {
            this.logger.LogInformation("Locking cursor to {ProcessName} {AppLockType}", ActiveProcess.Name, ActiveProcess.AppLockType);

            var border = ActiveProcess.AppLockType switch
            {
                AppLockType.Window => ApplicationHandler.GetApplicationBorders(hwnd),
                AppLockType.Monitor => ApplicationHandler.GetMonitorBorders(hwnd),
                _ => throw new NotImplementedException("Invalid AppLockType")
            };
            if (!MouseHandler.LockCursorToBorder(border))
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

        var success = this.applicationEventSource.Register();
        this.logger.LogDebug("Register application hook: {Success}", success);

        if (!success)
        {
            var errorCode = Marshal.GetLastWin32Error();
            this.logger.LogError("Application hook error code {ErrorCode}", errorCode);
        }

        this.applicationEventSource.ApplicationChanged += OnApplicationChanged;

        this.applicationEventSource.Update();
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
                    var process = ProcessListItemExtensions.FromNameAndPath(p.ProcessName, p.MainModule.FileName);
                    Processes.Add(process);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogWarning(ex, "Failed to get module info for process: {ProcessName}", p.ProcessName);

                try
                {
                    var process = ProcessListItemExtensions.FromNameAndPath(p.ProcessName, null);
                    Processes.Add(process);
                }
                catch (Exception ex2)
                {
                    this.logger.LogError(ex2, "Failed to create process list item for process: {ProcessName}", p.ProcessName);
                }
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

    private void AppLockSettingsButton_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var processItem = (ProcessListItem)button.DataContext;

        var appLockSettingsWindow = new AppLockSettingsWindow(processItem);
        appLockSettingsWindow.ShowDialog();
    }
}
