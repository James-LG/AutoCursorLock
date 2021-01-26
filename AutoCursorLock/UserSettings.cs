// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using AutoCursorLock.Models;
    using Newtonsoft.Json;

    public class UserSettings : INotifyPropertyChanged
    {
        private HotKey? hotKey;

        public UserSettings(ObservableCollection<ProcessListItem> enabledProcesses, HotKey? hotKey)
        {
            EnabledProcesses = enabledProcesses ?? throw new ArgumentNullException(nameof(enabledProcesses));
            HotKey = hotKey;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<ProcessListItem> EnabledProcesses { get; }

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

        public static UserSettings Load()
        {
            var path = GetPath();
            if (File.Exists(path))
            {
                var text = File.ReadAllText(GetPath());
                return JsonConvert.DeserializeObject<UserSettings>(text);
            }

            return new UserSettings(new ObservableCollection<ProcessListItem>(), null);
        }

        public void Save()
        {
            var path = GetPath();
            var file = new FileInfo(path);
            file.Directory?.Create();

            var text = JsonConvert.SerializeObject(this);
            File.WriteAllText(path, text);
        }

        private static string GetPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"AutoCursorLock", "settings.json");

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
