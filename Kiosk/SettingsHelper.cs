﻿// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: http://www.microsoft.com/cognitive
// 
// Microsoft Cognitive Services Github:
// https://github.com/Microsoft/Cognitive
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using IntelligentKioskSample.Views;
using System;
using System.ComponentModel;
using System.IO;
using Windows.Storage;

namespace IntelligentKioskSample
{
    internal class SettingsHelper : INotifyPropertyChanged
    {
        public event EventHandler SettingsChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private static SettingsHelper instance;

        static SettingsHelper()
        {
            instance = new SettingsHelper();
        }

        public void Initialize()
        {
            LoadRoamingSettings();
            Windows.Storage.ApplicationData.Current.DataChanged += RoamingDataChanged;
        }

        private void RoamingDataChanged(ApplicationData sender, object args)
        {
            LoadRoamingSettings();
            instance.OnSettingsChanged();
        }

        private void OnSettingsChanged()
        {
            if (instance.SettingsChanged != null)
            {
                instance.SettingsChanged(instance, EventArgs.Empty);
            }
        }

        private async void OnSettingChanged(string propertyName, object value)
        {
            if (propertyName == "MallKioskDemoCustomSettings")
            {
                // save to file as the content is too big to be saved as a string-like setting
                StorageFile file = await ApplicationData.Current.RoamingFolder.CreateFileAsync(
                    "MallKioskDemoCustomSettings.xml",
                    CreationCollisionOption.ReplaceExisting);

                using (Stream stream = await file.OpenStreamForWriteAsync())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        await writer.WriteAsync(value.ToString());
                    }
                }
            }
            else
            {
                ApplicationData.Current.RoamingSettings.Values[propertyName] = value;
            }

            instance.OnSettingsChanged();
            instance.OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (instance.PropertyChanged != null)
            {
                instance.PropertyChanged(instance, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static SettingsHelper Instance
        {
            get
            {
                return instance;
            }
        }

        private async void LoadRoamingSettings()
        {
            object value = ApplicationData.Current.RoamingSettings.Values["DeviceName"];
            if (value != null)
            {
                this.DeviceName = value.ToString();
            }
            value = ApplicationData.Current.RoamingSettings.Values["DeviceKey"];
            if (value != null)
            {
                this.DeviceKey = value.ToString();
            }
            //object value = ApplicationData.Current.RoamingSettings.Values["FaceApiKey"];
            //if (value != null)
            //{
            //  this.FaceApiKey = value.ToString();
            this.FaceApiKey = "635db0a6bc26463d81e4c3ce32471cb9";
            //}

           // value = ApplicationData.Current.RoamingSettings.Values["EmotionApiKey"];
            //if (value != null)
           // {
                this.EmotionApiKey = "3d40a27a2ba34ee1a75a8d0ce39fadb8";
           // }

            //value = ApplicationData.Current.RoamingSettings.Values["VisionApiKey"];
           // if (value != null)
           // {
                this.VisionApiKey = "baf75a45e02f48c79e6e1b7939b951a2";
           // }

          //  value = ApplicationData.Current.RoamingSettings.Values["BingSearchApiKey"];
          //  if (value != null)
            //{
                this.BingSearchApiKey = "8ef2eb01495a47da9e04fb311b104d4d";
           // }

            //value = ApplicationData.Current.RoamingSettings.Values["BingAutoSuggestionApiKey"];
            //if (value != null)
            //{
                this.BingAutoSuggestionApiKey = "983e20f6dd144f18ac1da1de28c3978d";
            //  }

             value   = ApplicationData.Current.RoamingSettings.Values["WorkspaceKey"];
            if (value != null)
            {
                this.WorkspaceKey = value.ToString();
            }

            //value = ApplicationData.Current.RoamingSettings.Values["TextAnalyticsKey"];
            //if (value != null)
            //{
                this.TextAnalyticsKey = "57ec0d34687949d190b95cab19194293";
           // }

            value = ApplicationData.Current.RoamingSettings.Values["CameraName"];
            if (value != null)
            {
                this.CameraName = value.ToString();
            }

            value = ApplicationData.Current.RoamingSettings.Values["MinDetectableFaceCoveragePercentage"];
            if (value != null)
            {
                uint size;
                if (uint.TryParse(value.ToString(), out size))
                {
                    this.MinDetectableFaceCoveragePercentage = size;
                }
            }

            value = ApplicationData.Current.RoamingSettings.Values["ShowDebugInfo"];
            if (value != null)
            {
                bool booleanValue;
                if (bool.TryParse(value.ToString(), out booleanValue))
                {
                    this.ShowDebugInfo = booleanValue;
                }
            }

            value = ApplicationData.Current.RoamingSettings.Values["DriverMonitoringSleepingThreshold"];
            if (value != null)
            {
                double threshold;
                if (double.TryParse(value.ToString(), out threshold))
                {
                    this.DriverMonitoringSleepingThreshold = threshold;
                }
            }

            value = ApplicationData.Current.RoamingSettings.Values["DriverMonitoringYawningThreshold"];
            if (value != null)
            {
                double threshold;
                if (double.TryParse(value.ToString(), out threshold))
                {
                    this.DriverMonitoringYawningThreshold = threshold;
                }
            }

            // load mall kiosk demo custom settings from file as the content is too big to be saved as a string-like setting
            try
            {
                using (Stream stream = await ApplicationData.Current.RoamingFolder.OpenStreamForReadAsync("MallKioskDemoCustomSettings.xml"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        this.MallKioskDemoCustomSettings = await reader.ReadToEndAsync();
                    }
                }
            }
            catch (Exception)
            {
                this.RestoreMallKioskSettingsToDefaultFile();
            }
        }

        public void RestoreMallKioskSettingsToDefaultFile()
        {
            this.MallKioskDemoCustomSettings = File.ReadAllText("Views\\MallKioskDemoConfig\\MallKioskDemoSettings.xml");
        }

        public void RestoreAllSettings()
        {
            ApplicationData.Current.RoamingSettings.Values.Clear();
        }

        private string faceApiKey = string.Empty;
        public string FaceApiKey
        {
            get { return this.faceApiKey; }
            set
            {
                this.faceApiKey = value;
                this.OnSettingChanged("FaceApiKey", value);
            }
        }


        private string emotionApiKey = string.Empty;
        public string EmotionApiKey
        {
            get { return this.emotionApiKey; }
            set
            {
                this.emotionApiKey = value;
                this.OnSettingChanged("EmotionApiKey", value);
            }
        }

        private string visionApiKey = string.Empty;
        public string VisionApiKey
        {
            get { return this.visionApiKey; }
            set
            {
                this.visionApiKey = value;
                this.OnSettingChanged("VisionApiKey", value);
            }
        }

        private string bingSearchApiKey = string.Empty;
        public string BingSearchApiKey
        {
            get { return this.bingSearchApiKey; }
            set
            {
                this.bingSearchApiKey = value;
                this.OnSettingChanged("BingSearchApiKey", value);
            }
        }

        private string bingAutoSuggestionSearchApiKey = string.Empty;
        public string BingAutoSuggestionApiKey
        {
            get { return this.bingAutoSuggestionSearchApiKey; }
            set
            {
                this.bingAutoSuggestionSearchApiKey = value;
                this.OnSettingChanged("BingAutoSuggestionApiKey", value);
            }
        }

        private string workspaceKey = string.Empty;
        public string WorkspaceKey
        {
            get { return workspaceKey; }
            set
            {
                this.workspaceKey = value;
                this.OnSettingChanged("WorkspaceKey", value);
            }
        }

        private string mallKioskDemoCustomSettings = string.Empty;
        public string MallKioskDemoCustomSettings
        {
            get { return this.mallKioskDemoCustomSettings; }
            set
            {
                this.mallKioskDemoCustomSettings = value;
                this.OnSettingChanged("MallKioskDemoCustomSettings", value);
            }
        }

        private string textAnalyticsKey = string.Empty;
        public string TextAnalyticsKey
        {
            get { return textAnalyticsKey; }
            set
            {
                this.textAnalyticsKey = value;
                this.OnSettingChanged("TextAnalyticsKey", value);
            }
        }

        private string cameraName = string.Empty;
        public string CameraName
        {
            get { return cameraName; }
            set
            {
                this.cameraName = value;
                this.OnSettingChanged("CameraName", value);
            }
        }

        private uint minDetectableFaceCoveragePercentage = 7;
        public uint MinDetectableFaceCoveragePercentage
        {
            get { return this.minDetectableFaceCoveragePercentage; }
            set
            {
                this.minDetectableFaceCoveragePercentage = value;
                this.OnSettingChanged("MinDetectableFaceCoveragePercentage", value);
            }
        }

        private bool showDebugInfo = false;
        public bool ShowDebugInfo
        {
            get { return showDebugInfo; }
            set
            {
                this.showDebugInfo = value;
                this.OnSettingChanged("ShowDebugInfo", value);
            }
        }

        private double driverMonitoringSleepingThreshold = RealtimeDriverMonitoring.DefaultSleepingApertureThreshold;
        public double DriverMonitoringSleepingThreshold
        {
            get { return this.driverMonitoringSleepingThreshold; }
            set
            {
                this.driverMonitoringSleepingThreshold = value;
                this.OnSettingChanged("DriverMonitoringSleepingThreshold", value);
            }
        }

        private double driverMonitoringYawningThreshold = RealtimeDriverMonitoring.DefaultYawningApertureThreshold;
        public double DriverMonitoringYawningThreshold
        {
            get { return this.driverMonitoringYawningThreshold; }
            set
            {
                this.driverMonitoringYawningThreshold = value;
                this.OnSettingChanged("DriverMonitoringYawningThreshold", value);
            }
        }


        private string deviceName = string.Empty;
        public string DeviceName {
            get { return deviceName; }
            set
            {
                this.deviceName = value;
                this.OnSettingChanged("DeviceName", value);
            }
        }

        private string deviceKey = string.Empty;
        public string DeviceKey
        {
            get { return deviceKey; }
            set
            {
                this.deviceKey = value;
                this.OnSettingChanged("DeviceKey", value);
            }
        }
    }
}