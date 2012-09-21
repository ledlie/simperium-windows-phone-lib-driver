/*
 * Copyright 2012 Jonathan Ledlie
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
 * implied.
 *
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

﻿using System;
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

        IsolatedStorageSettings isoSettings;
        Simperium.Settings settings;

        public AccountViewModel(Simperium.Settings _settings)
        {
            isoSettings = IsolatedStorageSettings.ApplicationSettings;
            settings = _settings;
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
                return settings.API_KEY;
            }

            set
            {
                if (value != settings.API_KEY)
                {
                    settings.API_KEY = value;
                    NotifyPropertyChanged("ApiKey");
                }
            }
        }

        public string AdminKey
        {
            get
            {
                return settings.ADMIN_KEY;
            }

            set
            {
                if (value != settings.ADMIN_KEY)
                {
                    settings.ADMIN_KEY = value;
                    NotifyPropertyChanged("AdminKey");
                }
            }
        }

        internal void Save()
        {
            isoSettings[SETTINGS_APP_ID] = settings.APP_ID;
            isoSettings[SETTINGS_API_KEY] = settings.API_KEY;
            isoSettings[SETTINGS_ADMIN_KEY] = settings.ADMIN_KEY;
            isoSettings.Save();
        }

        internal void Reset()
        {
            AppId = "";
            ApiKey = "";
            AdminKey = "";
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
