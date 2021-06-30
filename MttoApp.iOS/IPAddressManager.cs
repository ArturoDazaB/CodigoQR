using MttoApp.Servicios;
using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;

[assembly: Xamarin.Forms.Dependency(typeof(MttoApp.iOS.IPAddressManager))]

namespace MttoApp.iOS
{
    internal class IPAddressManager : IIPAddressManager
    {
        public string GetIPAddress()
        {
            try
            {
                foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                       netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                    {
                        foreach (var addressinfo in netInterface.GetIPProperties().UnicastAddresses)
                        {
                            if (addressinfo.Address.AddressFamily == AddressFamily.InterNetwork)
                            {
                                return addressinfo.Address.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is NetworkInformationException)
            {
                return "ERROR";
            }

            return "";
        }
    }
}