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
using System.Windows.Navigation;
using System.IO.IsolatedStorage;

namespace SimperiumLibraryDriver
{
    public partial class MainPage : PhoneApplicationPage
    {
        JsonConverter<BasicJsonObject> basicJsonObjectConverter;

        // Handle on json object that we are going to send to the server.
        // A Get will replace this object with a newly deserialized one
        BasicJsonObject data;

        // to save the bucket name and object id for convenience
        IsolatedStorageSettings isoSettings;
        const string SETTINGS_BUCKET_NAME = "bucketName";
        const string SETTINGS_OBJECT_ID = "objectId";
        const string SETTINGS_CCID = "ccid";
        const string SETTINGS_CLIENT_ID = "client_id";


        public MainPage()
        {
            InitializeComponent();

            isoSettings = IsolatedStorageSettings.ApplicationSettings;

            basicJsonObjectConverter = new JsonConverter<BasicJsonObject>();
            // TODO Remove
            App.Settings.APP_ID = "cylinder-manager-e30";
            App.Settings.API_KEY = "d6c6b4b7aa0f4162a04f23ebd34c6d2e";
            App.Settings.ADMIN_KEY = "e4b4709e31924777a4521df5fbf57692";

            data = new BasicJsonObject();
            // use two-way binding to set BasicjsonObject's key/value pairs
            JsonObjectGrid.DataContext = data;

            if (isoSettings.Contains(SETTINGS_BUCKET_NAME))
                BucketName_Box.Text = isoSettings[SETTINGS_BUCKET_NAME] as string;
            if (isoSettings.Contains(SETTINGS_OBJECT_ID))
                ObjectId_Box.Text = isoSettings[SETTINGS_OBJECT_ID] as string;
            if (isoSettings.Contains(SETTINGS_CCID))
                CCID_Box.Text = isoSettings[SETTINGS_CCID] as string;
            if (isoSettings.Contains(SETTINGS_CLIENT_ID))
                ClientId_Box.Text = isoSettings[SETTINGS_CLIENT_ID] as string;
        }


        private void GetButton_Click(object sender, RoutedEventArgs e)
        {
            if (!checkBucket() || !checkObjectId())
                return;
            App.Bucket.GetCompleted += new EventHandler<BucketEventArgs<GetResult>>(GetCompleted);
            App.Bucket.Get(ObjectId_Box.Text);
            output("Sent get request");
        }

        void GetCompleted(object sender, BucketEventArgs<GetResult> e)
        {
            if (e.Error != null)
            {
                output("Get Error " + e.Error.Message);
                return;
            }
            output("Received get response OK: " + e.Result.data);
            BasicJsonObject result = basicJsonObjectConverter.ConvertJsonToObject(e.Result.data);
            if (result == null)
            {
                output("Failed to deserialize json response");
                return;
            }
            
            data = result;
            JsonObjectGrid.DataContext = data;
            Version_Box.Text = "" + e.Result.version;
        }

        void NewIdButton_Click(object sender, RoutedEventArgs e)
        {
            ObjectId_Box.Text = Simperium.Bucket.GenerateId();
            Version_Box.Text = "";
        }

        void SetButton_Click(object sender, RoutedEventArgs e)
        {
            if (!checkBucket() || !checkObjectId())
                return;
            App.Bucket.SetCompleted += new EventHandler<BucketEventArgs<SetResult>>(SetCompleted);

            // serialize the key/values from the UI
            string json = basicJsonObjectConverter.ConvertObjectToJson(data);

            // extract the version and ccid in case they have been set
            int version = -1;
            if (!getNumber(Version_Box.Text, out version, "version"))
                return;
            int ccid = -1;
            if (!getNumber(CCID_Box.Text, out ccid, "CCID"))
                return;

            // we just take all of the values, in case any of them are set
            App.Bucket.Set(ObjectId_Box.Text, json, version,
                (bool)Response_CheckBox.IsChecked.Value, (bool)Replace_CheckBox.IsChecked.Value,
                ClientId_Box.Text, ccid);
            output("Sent set request");
        }

        void SetCompleted(object sender, BucketEventArgs<SetResult> e)
        {
            if (e.Error != null)
            {
                output("Set Error " + e.Error.ToString());
                return;
            }
            output("Received set response OK: " + e.Result.data);

            // if we asked for a response, e.Result.data contains the current object
            // So we just assign it
            if (!String.IsNullOrWhiteSpace(e.Result.data))
            {
                BasicJsonObject result = basicJsonObjectConverter.ConvertJsonToObject(e.Result.data);
                if (result == null)
                {
                    output("Failed to deserialize json response");
                    return;
                }
                data = result;
                JsonObjectGrid.DataContext = data;
            }
            Version_Box.Text = "" + e.Result.version;
            // TODO do anything with clientid and ccid ?
        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (!checkBucket() || !checkObjectId())
                return;

            // extract the version and ccid in case they have been set
            int version = -1;
            if (!getNumber(Version_Box.Text, out version, "version"))
                return;
            int ccid = -1;
            if (!getNumber(CCID_Box.Text, out ccid, "CCID"))
                return;


            App.Bucket.DeleteCompleted += new EventHandler<BucketEventArgs<DeleteResult>>(DeleteCompleted);
            App.Bucket.Delete(ObjectId_Box.Text, version, ClientId_Box.Text, ccid);
            output("Sent delete request");
        }


        void DeleteCompleted(object sender, BucketEventArgs<DeleteResult> e)
        {
            if (e.Error != null)
                output("Delete Error " + e.Error.Message);
            else
            {
                output("Received delete response OK");
                Version_Box.Text = "" + e.Result.version;
            }
        }



        private void IndexButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ChangesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Menu navigation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void UserAuthMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/UserAuthPage.xaml", UriKind.Relative));
        }

        private void LogMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/LogPage.xaml", UriKind.Relative));
        }

        private void AccountMenuItem_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AccountPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Helper functions
        /// </summary>

        private void output(string logLine)
        {
            LogText.Text = logLine;
            App.Log.Append(logLine);
        }

        private bool checkBucket()
        {
            if (String.IsNullOrWhiteSpace(BucketName_Box.Text))
            {
                output("Empty bucket name");
                return false;
            }
            if (String.IsNullOrWhiteSpace(App.Credentials.AccessToken))
            {
                output("Empty access token");
                return false;
            }
            if (App.Bucket == null || App.Bucket.Name != BucketName_Box.Text)
                App.Bucket = new Bucket(BucketName_Box.Text, App.Credentials.AccessToken, App.Settings);
            return true;
        }

        private bool checkObjectId()
        {
            if (String.IsNullOrWhiteSpace(ObjectId_Box.Text))
            {
                output("Empty object id");
                return false;
            }
            return true;
        }


        private bool getNumber(string numText, out int num, string boxName)
        {
            if (!String.IsNullOrWhiteSpace(numText))
            {
                bool ok = Int32.TryParse(numText, out num);
                if (!ok)
                {
                    output("Could not parse " +boxName+ " number");
                }
                return ok;
            }
            num = -1;
            return true;
        }


        private void BucketName_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
             isoSettings[SETTINGS_BUCKET_NAME] = BucketName_Box.Text;
             isoSettings.Save();
        }

        private void ObjectId_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            isoSettings[SETTINGS_OBJECT_ID] = ObjectId_Box.Text;
            isoSettings.Save();
        }

        private void CCID_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            isoSettings[SETTINGS_CCID] = CCID_Box.Text;
            isoSettings.Save();
        }

        private void ClientId_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            isoSettings[SETTINGS_CLIENT_ID] = ClientId_Box.Text;
            isoSettings.Save();
        }
    }
}