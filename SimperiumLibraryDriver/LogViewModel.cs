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
using System.Collections.Generic;
using System.ComponentModel;

namespace SimperiumLibraryDriver
{
    public class LogViewModel : INotifyPropertyChanged
    {
        private List<string> logLines;
        private string logText;
        private int MAX_LOG_LINES = 500;

        public LogViewModel()
        {
            logLines = new List<string>();
        }

        public string Log
        {
            get
            {
                return logText;
            }
        }

        public void Append(string logLine)
        {
            logLines.Add(logLine);
            if (logLines.Count > MAX_LOG_LINES)
            {
                logLines.RemoveRange(0, 10); // assumes MAX_LOG_LINES > 10
                logText = "";
                foreach (string line in logLines)
                    logText += line + Environment.NewLine;
            }
            else
            {
                logText += logLine + Environment.NewLine;
            }

            NotifyPropertyChanged("Log");
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
