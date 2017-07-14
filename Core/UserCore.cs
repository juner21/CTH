using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using InterfaceContract;

namespace Core
{
    public class UserCore
    {

        public User Get()
        {
            return ContainerDocker.Container.GetExportedValue<IUserData>().Get();
        }


        public bool Login(string userName, string pwd)
        {
            return ContainerDocker.Container.GetExportedValue<IUserData>().Login(userName, pwd);
        }


        private UserCore() { }
        private static UserCore instance;
        private static object _lock = new object();

        public static UserCore GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new UserCore();
                        }
                    }
                }
                return instance;
            }
        }

    }

}
