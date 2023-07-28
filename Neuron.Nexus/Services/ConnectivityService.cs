using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neuron.Nexus.Services
{
    public interface IConnectivityService
    {
        bool IsConnected();
    }
    public class ConnectivityService : IConnectivityService
    {
        public bool IsConnected()
        {
            bool isConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;
            if (isConnected)
            {
                return true;
            }

            return false;
        }
    }
}
