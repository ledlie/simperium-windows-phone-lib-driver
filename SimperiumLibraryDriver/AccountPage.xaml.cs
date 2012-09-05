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

namespace SimperiumLibraryDriver
{
    public partial class AccountPage : PhoneApplicationPage
    {
        public AccountPage()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            App.Account.Save();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            App.Account.Reset();
        }
    }
}