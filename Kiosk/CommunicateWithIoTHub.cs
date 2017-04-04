﻿using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentKioskSample
{
    public class CommunicateWithIoTHub
    {
        DeviceClient deviceClient;
        string iotHubUri = "PartnersHub.azure-devices.net";
        string deviceKey = "qcE/mE8D63k0w4o0+muPXNk9Fzlwhvyo8EtVPLZ14m0=";

        public void SendEmotions(IEnumerable<KeyValuePair<string, float>> EmotionsScores, String Gender)
        {
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("RBPi3", deviceKey), TransportType.Http1);
            SendEmotionsToCloudMessagesAsync(EmotionsScores, Gender);
        }

        private async void SendEmotionsToCloudMessagesAsync(IEnumerable<KeyValuePair<string, float>> EmotionsScores, string Gender)
        {
            List<KeyValuePair<string, float>> emotions = new List<KeyValuePair<string, float>>();
            string happiness = "";
            string disgust = "";
            string fear = "";
            string anger = "";
            string neutral = "";
            string surprise = "";
            string contempt = "";
            string sadness = "";

            foreach (var item in EmotionsScores)
            {
                if (item.Key == "Happiness")
                    happiness = item.Value.ToString("0.000");
                else if (item.Key == "Disgust")
                    disgust = item.Value.ToString("0.000");
                else if (item.Key == "Fear")
                    fear = item.Value.ToString("0.000");
                else if (item.Key == "Anger")
                    anger = item.Value.ToString("0.000");
                else if (item.Key == "Sadness")
                    sadness = item.Value.ToString("0.000");
                else if (item.Key == "Neutral")
                    neutral = item.Value.ToString("0.000");
                else if (item.Key == "Surprise")
                    surprise = item.Value.ToString("0.000");
                else if (item.Key == "Contempt")
                    contempt = item.Value.ToString("0.000");
            }

            float score = 0;
            string emotion = "";

            foreach (var item in EmotionsScores)
            {
                if (item.Value > score)
                {
                    emotion = item.Key;
                    score = item.Value;
                }
            }

            var telemetryDataPoint = new
            {
                Gender = Gender,
                Happiness = happiness,
                Disgust = disgust,
                Fear = fear,
                Anger = anger,
                Sadness = sadness,
                Neutral = neutral,
                Surprise = surprise,
                Contempt = contempt,
                Highest = emotion
            };

            var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            //string messageString = "Emotions Detected";
            //var message = new Message(Encoding.ASCII.GetBytes(messageString));

            //foreach (var item in EmotionsScores)
            //{
            //    message.Properties.Add(item.Key, item.Value.ToString("0.000"));
            //}

            await deviceClient.SendEventAsync(message);
        }

    }
}
