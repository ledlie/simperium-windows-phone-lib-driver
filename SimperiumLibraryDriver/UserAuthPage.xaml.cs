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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Simperium;

namespace SimperiumLibraryDriver
{
    public partial class UserAuthPage : PhoneApplicationPage
    {
        public UserAuthPage()
        {
            InitializeComponent();
            ContentPanel.DataContext = App.Credentials;
        }

        private void AuthorizeButton_Click(object sender, RoutedEventArgs e)
        {
            App.UserAuth.AuthorizeUserCompleted += new EventHandler<UserAuthEventArgs<AuthorizeResult>>(AuthorizeUserCompleted);
            App.UserAuth.Authorize(App.Credentials.Username, App.Credentials.Password);
            output("Sent authentication for " + App.Credentials.Username);

        }

        void AuthorizeUserCompleted(object sender, UserAuthEventArgs<AuthorizeResult> e)
        {
            if (e.Error != null)
                output(e.Error.Message);
            else
            {
                App.Credentials.AccessToken = e.Result.access_token;
                output("Auth completed OK, token=" + App.Credentials.AccessToken);
            }
        }


        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void output(string logLine)
        {
            LogText.Text = logLine;
            App.Log.Append(logLine);
        }
    }
}