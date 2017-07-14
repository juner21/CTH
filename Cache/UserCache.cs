using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Model;

namespace Cache
{
    public class UserCache :BaseCache
    {
        public string Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            var r = CacheHelp.Get<string>(key);

            if (r == null) {
                r = UserCore.GetInstance.Get().UserName;
                CacheHelp.Set(key, r, base.TINY);
            }                

            return r;
        }


        public void Set(string key ,object value,int minutes) {

            CacheHelp.Set(key, value, minutes);
        }

        public bool Login(string userName, string pwd)
        {
            return UserCore.GetInstance.Login(userName,pwd);
        }


        private UserCache() { }
        private static UserCache instance;
        private static object _lock = new object();

        public static UserCache GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new UserCache();
                        }
                    }
                }
                return instance;
            }
        }
    }
}
