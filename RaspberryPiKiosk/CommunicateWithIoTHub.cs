using Microsoft.Azure.Devices.Client;
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


        public void SendEmotions(IEnumerable<KeyValuePair<string, float>> EmotionsScores)
        {
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey("RBPi3", deviceKey), TransportType.Http1);
            SendDeviceToCloudMessagesAsync(EmotionsScores);
        }

        private async void SendDeviceToCloudMessagesAsync(IEnumerable<KeyValuePair<string, float>> EmotionsScores)
        {
            string messageString = "Emotions Detected";
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            foreach (var item in EmotionsScores)
            {
                message.Properties.Add(item.Key, item.Value.ToString("0.000"));
            }

            await deviceClient.SendEventAsync(message);
        }
    }
}
