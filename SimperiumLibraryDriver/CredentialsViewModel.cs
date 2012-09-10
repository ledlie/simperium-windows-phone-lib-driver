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
    public class CredentialsViewModel : INotifyPropertyChanged
    {
        private string username;
        private string password;
        private string access_token;

        const string SETTINGS_USERNAME = "username";
        const string SETTINGS_PASSWORD = "password";
        const string SETTINGS_ACCESS_TOKEN = "access_token";

        IsolatedStorageSettings isoSettings = IsolatedStorageSettings.ApplicationSettings;

        public CredentialsViewModel()
        {
            
        }

        public string Username
        {
            get
            {
                if (String.IsNullOrWhiteSpace(username))
                    if (isoSettings.Contains(SETTINGS_USERNAME))
                        username = isoSettings[SETTINGS_USERNAME] as string;
                return username;
            }

            set {
                if (value != username) {
                    username = value;
                    isoSettings[SETTINGS_USERNAME] = username;
                    isoSettings.Save();
                    NotifyPropertyChanged("Username");
                }
            }
        }

        public string Password
        {
            get
            {
                if (String.IsNullOrWhiteSpace(password))
                    if (isoSettings.Contains(SETTINGS_PASSWORD))
                        password = isoSettings[SETTINGS_PASSWORD] as string;
                return password;
            }

            set
            {
                if (value != password)
                {
                    password = value;
                    isoSettings[SETTINGS_PASSWORD] = password;
                    isoSettings.Save();
                    NotifyPropertyChanged("Password");
                }
            }
        }

        public string AccessToken
        {
            get
            {
                if (String.IsNullOrWhiteSpace(access_token))
                    if (isoSettings.Contains(SETTINGS_ACCESS_TOKEN))
                        access_token = isoSettings[SETTINGS_ACCESS_TOKEN] as string;
                return access_token;
            }

            set
            {
                if (value != access_token)
                {
                    access_token = value;
                    isoSettings[SETTINGS_ACCESS_TOKEN] = access_token;
                    isoSettings.Save();
                    NotifyPropertyChanged("AccessToken");
                }
            }
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
