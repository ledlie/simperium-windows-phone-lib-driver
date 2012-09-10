using System;
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