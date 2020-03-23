/// <summary>
/// The network helper development.
/// </summary>

namespace VoiceModChat.Helpers
{
    using System;
    using System.Net;
    using System.Net.NetworkInformation;

    /// <summary>
    /// Class which has helper methods related to the network.
    /// </summary>
    public static class NetworkHelper
    {
        /// <summary>
        /// Checks if the given port is available.
        /// </summary>
        /// <param name="port">The port to be checked.</param>
        /// <returns><c>true</c> if the port is available, <c>false</c> otherwise.</returns>
        public static bool CheckIfPortAvailable(uint port)
        {
            bool isAvailable = true;

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpListeners();

            foreach (IPEndPoint tcpi in tcpConnInfoArray)
            { 
                if (tcpi.Port == port)
                {
                    isAvailable = false;
                    break;
                }
            }

            return isAvailable;
        }
    }
}
