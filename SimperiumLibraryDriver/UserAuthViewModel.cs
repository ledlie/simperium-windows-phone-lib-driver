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
    public class UserAuthViewModel : INotifyPropertyChanged
    {
        private string username;
        private string password;

        const string SETTINGS_USERNAME = "username";
        const string SETTINGS_PASSWORD = "password";

        IsolatedStorageSettings isoSettings = IsolatedStorageSettings.ApplicationSettings;

        public UserAuthViewModel()
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
