// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using AutoCursorLock.Dtos;
    using AutoCursorLock.Models;
    using AutoCursorLock.Native;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the program settings as defined by the user.
    /// </summary>
    public class UserSettings : INotifyPropertyChanged
    {
        private HotKey? hotKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserSettings"/> class.
        /// </summary>
        /// <param name="enabledProcesses">The processes enabled byh the user.</param>
        /// <param name="hotKey">The hot key to toggle the cursor lock.</param>
        public UserSettings(ObservableCollection<ProcessListItem> enabledProcesses, HotKey? hotKey)
        {
            EnabledProcesses = enabledProcesses ?? throw new ArgumentNullException(nameof(enabledProcesses));
            HotKey = hotKey;
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets the processes that the user has selected to lock the cursor on when in focus.
        /// </summary>
        /// <remarks>
        /// When a window from an enabled process is focused by the user, this program will lock the cursor
        /// to the bounds of the window until focus is changed to another process.
        /// </remarks>
        public ObservableCollection<ProcessListItem> EnabledProcesses { get; }

        /// <summary>
        /// Gets or sets the hot key used to toggle the programs locking feature.
        /// </summary>
        public HotKey? HotKey
        {
            get
            {
                return this.hotKey;
            }

            set
            {
                this.hotKey = value;
                NotifyPropertyChanged(nameof(HotKey));
            }
        }

        /// <summary>
        /// Loads and deserializes user settings from a json file.
        /// </summary>
        /// <returns>The user settings loaded from the json file.</returns>
        public static UserSettings Load()
        {
            var path = GetPath();
            if (!File.Exists(path))
            {
                return new UserSettings(new ObservableCollection<ProcessListItem>(), null);
            }

            var text = File.ReadAllText(GetPath());
            var settingsDto = JsonConvert.DeserializeObject<UserSettingsDto>(text);

            var processes = new ObservableCollection<ProcessListItem>();
            foreach (var processDto in settingsDto.EnabledProcesses)
            {
                if (File.Exists(processDto.Path))
                {
                    processes.Add(new ProcessListItem(processDto.Name, processDto.Path));
                }
            }

            return new UserSettings(processes, settingsDto.HotKey);
        }

        /// <summary>
        /// Serializes and saves the user settings to a json file.
        /// </summary>
        public void Save()
        {
            var path = GetPath();
            var file = new FileInfo(path);
            file.Directory?.Create();

            var text = JsonConvert.SerializeObject(this);
            File.WriteAllText(path, text);
        }

        /// <summary>
        /// Gets the path of the settings json file.
        /// </summary>
        /// <remarks>
        /// This path will be somethign like: C:\Users\<User></User>\AppData\Roaming\AutoCursorLock\settings.json.
        /// </remarks>
        /// <returns>The path to the settings file.</returns>
        private static string GetPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"AutoCursorLock", "settings.json");

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
