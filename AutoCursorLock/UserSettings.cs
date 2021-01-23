using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace AutoCursorLock
{
    public class UserSettings
    {
        private static string GetPath() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"AutoCursorLock", "settings.json");

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
    }
}
