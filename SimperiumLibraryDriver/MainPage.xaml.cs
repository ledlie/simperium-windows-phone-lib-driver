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
    public partial class MainPage : PhoneApplicationPage
    {
        Simperium.Settings settings;

        public MainPage()
        {
            InitializeComponent();
            settings = new Simperium.Settings();
            settings.APP_ID = "cylinder-manager-e30";
            settings.API_KEY = "d6c6b4b7aa0f4162a04f23ebd34c6d2e";
            settings.USER_AGENT = "WPLib/0.1";

            UserAuth userAuth = new UserAuth(settings);
            userAuth.AuthorizeUserCompleted += new EventHandler<UserAuthEventArgs<AuthorizeResult>>(userAuth_AuthorizeUserCompleted);
            userAuth.Authorize("ledlie@csail.mit.edu", "yorker");
            for (int i = 0; i < 600; i++)
                App.Log.Append("Sent authentication " + i);
        }

        void userAuth_AuthorizeUserCompleted(object sender, UserAuthEventArgs<AuthorizeResult> e)
        {
            App.Log.Append("Auth completed");
            Console.WriteLine(e);

            Simperium.Bucket bucket = new Bucket("samplebucket", e.Result.access_token, settings);
            bucket.GetCompleted += new EventHandler<BucketEventArgs<GetResult>>(bucket_GetCompleted);
            bucket.Get("newitem");
        }

        void bucket_GetCompleted(object sender, BucketEventArgs<GetResult> e)
        {
            Console.WriteLine(e);
            Console.WriteLine(e.Result.data);
        }

        private void LogMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LogPage.xaml", UriKind.Relative));
        }

        private void AccountMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AccountPage.xaml", UriKind.Relative));
        }

        private void UserAuthMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/UserAuthPage.xaml", UriKind.Relative));
        }

        private void GetButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}