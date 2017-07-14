using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ServerSetting
    {

        private static string serverState = "local";

        //private static string ConnState = "server"; 


        public static string ServerState {
            get {
                return serverState;
            }
        }

        static ServerSetting()
        {

            switch (ServerSetting.ServerState)
            {
                case "local":
                    break;
                case "toServer":
                    break;
                case "online":
                    break;
            }
        }
    }
}
