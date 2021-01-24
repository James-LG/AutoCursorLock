// Copyright (c) James La Novara-Gsell. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace AutoCursorLock
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using Newtonsoft.Json;

    public class UserSettings
    {
        public UserSettings(ObservableCollection<ProcessListItem> enabledProcesses)
        {
            EnabledProcesses = enabledProcesses ?? throw new ArgumentNullException(nameof(enabledProcesses));
        }

        public ObservableCollection<ProcessListItem> EnabledProcesses { get; }

        public static UserSettings Load()
        {
            var path = GetPath();
            if (File.Exists(path))
            {
                var text = File.ReadAllText(GetPath());
                return JsonConvert.DeserializeObject<UserSettings>(text);
            }

            return new UserSettings(new ObservableCollection<ProcessListItem>());
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
    }
}
