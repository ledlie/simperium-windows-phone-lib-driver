using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.IO.IsolatedStorage;

namespace SimperiumLibraryDriver
{
    // Wrap Simperium.Settings and save it to local storage
    public class AccountViewModel : INotifyPropertyChanged
    {
        const string SETTINGS_APP_ID = "app_id";
        const string SETTINGS_API_KEY = "api_key";
        const string SETTINGS_ADMIN_KEY = "admin_key";

        IsolatedStorageSettings isoSettings = IsolatedStorageSettings.ApplicationSettings;
        Simperium.Settings settings;

        public AccountViewModel()
        {
            settings = new Simperium.Settings();
            settings.USER_AGENT = "WPLib/0.1";
            if (isoSettings.Contains(SETTINGS_APP_ID))
                settings.APP_ID = (isoSettings[SETTINGS_APP_ID] as string);
        }

        public string AppId
        {
            get
            {
                return settings.APP_ID;
            }

            set
            {
                if (value != settings.APP_ID)
                {
                    settings.APP_ID = value;
                    NotifyPropertyChanged("AppId");
                }
            }
        }

        public string ApiKey
        {
            get
            {
                if (String.IsNullOrWhiteSpace(api_key))
                    if (isoSettings.Contains(SETTINGS_ADMIN_KEY))
                        api_key = isoSettings[SETTINGS_ADMIN_KEY] as string;
                return api_key;
            }

            set
            {
                if (value != api_key)
                {
                    api_key = value;
                    NotifyPropertyChanged("ApiKey");
                }
            }
        }

        /*
        public string AdminKey
        {
            get
            {
                if (String.IsNullOrWhiteSpace(admin_key))
                    if (isoSettings.Contains(SETTINGS_ADMIN_KEY))
                        admin_key = isoSettings[SETTINGS_ADMIN_KEY] as string;
                return admin_key;
            }

            set
            {
                if (value != admin_key)
                {
                    admin_key = value;
                    NotifyPropertyChanged("AdminKey");
                }
            }
        }

         */
        internal void Save()
        {
            isoSettings[SETTINGS_APP_ID] = app_id;
            isoSettings[SETTINGS_API_KEY] = api_key;
            isoSettings[SETTINGS_ADMIN_KEY] = admin_key;
            isoSettings.Save();
        }

        internal void Reset()
        {
            AppId = "";
            Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
    
}
