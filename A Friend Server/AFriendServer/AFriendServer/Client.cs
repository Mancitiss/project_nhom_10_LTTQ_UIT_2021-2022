using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AFriendServer
{
    internal class Client
    {
        internal TcpClient client;
        internal SslStream stream;
        internal bool is_locked;
        internal int loaded;
        internal int loopnum;
        internal string sessionKey;

        internal Client() { }

    }
}
