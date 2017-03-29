using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace CreateDeviceIdentity
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "HostName=PartnersHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=1DihL7Od8KQY6yOlBc/w5lqm8pnuMJUkZzs96ge+L6s=";

        static void Main(string[] args)
        {
        }
    }
}
