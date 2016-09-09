using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace NameDay
{
    /// <summary>
    /// This class is used to strore the settings in a localfolder
    /// </summary>
    public class Settings:ObservableObject
    {
        private bool _notificationEnabled = false;
        public bool NotificationEnabled
        {
            get
            {
                return _notificationEnabled;
            }
            set
            {
                if (Set(ref _notificationEnabled, value))
                    localSettings.Values[nameof(NotificationEnabled)] = value;
            }
        }
        private bool _updatingLiveTileEnabled = false;
        public bool UpdatingLiveTileEnabled
        {
            get
            {
                return _updatingLiveTileEnabled;
            }
            set
            {
                if (Set(ref _updatingLiveTileEnabled, value))
                    localSettings.Values[nameof(UpdatingLiveTileEnabled)] = value;
            }
        }
        private DateTime _lastSucessfullRun = DateTime.MinValue;
        public DateTime LastSucessfullRun
        {
            get
            {
                return _lastSucessfullRun;
            }

            set
            {
                if (Set(ref _lastSucessfullRun, value))
                    localSettings.Values[nameof(LastSucessfullRun)] = value;
            }
        }
        public Settings()
        {
            LoadSettings();
        }
        private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private void LoadSettings()
        {
            var notificationEnabled = localSettings.Values[nameof(NotificationEnabled)];
            if (notificationEnabled != null)
                NotificationEnabled = (bool)notificationEnabled;
            var updatingLiveTileEnabled = localSettings.Values[nameof(UpdatingLiveTileEnabled)];
            if (updatingLiveTileEnabled != null)
                UpdatingLiveTileEnabled = (bool)updatingLiveTileEnabled;
            var lastSucessfullRun = localSettings.Values[nameof(LastSucessfullRun)];
            if (lastSucessfullRun != null)
                LastSucessfullRun = DateTime.Parse(lastSucessfullRun.ToString(), CultureInfo.InvariantCulture);

        }
    }
}
