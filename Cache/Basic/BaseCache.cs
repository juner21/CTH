using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class BaseCache
    {
        public static ICache CacheHelp { get; private set; }

        protected BaseCache() { }

        static BaseCache()
        {
            CacheHelp = new MemCacheHelp();
        }


        protected int LONG = 60 * 24;
        protected int NORMAL = 60;
        protected int SHORT = 30;
        protected int TINY = 5;

    }

    public interface ICache
    {

        T Get<T>(string key);

        List<T> List<T>(List<string> keys);

        void Set(string key, object o, int minutes);

        void Remove(string key);
    }
    
}
