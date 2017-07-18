using Common;
using Enyim.Caching;
using Enyim.Caching.Configuration;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cache
{
    public class MemCacheHelp : ICache
    {
        /// <summary>  
        /// 定义一个静态MemcachedClient客户端,它随类一起加载，所有对象共用  
        /// </summary>  
        private static MemcachedClient mclient;
        /// <summary>  
        /// 静态构造函数，初始化Memcached客户端  
        /// </summary>  
        static MemCacheHelp()
        {
            mclient = MemCached.getInstance();
        }       

        public T Get<T>(string key)
        {
            return mclient.Get<T>(key);
        }

        public List<T> List<T>(List<string> keys)
        {
            var list = new List<T>();
            foreach (var item in keys)
            {
                list.Add(this.Get<T>(item));
            }

            return list;
        }

        public void Set(string key, object o, int minutes)
        {
            mclient.Store(StoreMode.Add,key,o,TimeSpan.FromMinutes(minutes));
        }

        public void Remove(string key)
        {
            mclient.Remove(key);
        }

    }


    /// <summary>  
    /// MemcachedClient 配置类  
    /// </summary>  
    public sealed class MemCached
    {
        private static MemcachedClient MemClient;
        static readonly object padlock = new object();
        //线程安全的单例模式  
        public static MemcachedClient getInstance()
        {
            if (MemClient == null)
            {
                lock (padlock)
                {
                    if (MemClient == null)
                    {
                        MemClientInit();
                    }
                }
            }
            return MemClient;
        }
        static void MemClientInit()
        {
            try
            {
                ////初始化缓存  
                //MemcachedClientConfiguration memConfig = new MemcachedClientConfiguration();
                //// 配置文件 - ip  
                //memConfig.Servers.Add(StringUtility.CreateIPEndPoint("127.0.0.1:11211"));

                //// 配置文件 - 协议  
                //memConfig.Protocol = MemcachedProtocol.Binary;
                //// 配置文件-权限，如果使用了免密码功能，则无需设置userName和password  
                //memConfig.Authentication.Type = typeof(PlainTextAuthenticator);
                //memConfig.NodeLocator = typeof(DefaultNodeLocator);
                ////下面请根据实例的最大连接数进行设置  
                //memConfig.SocketPool.MinPoolSize = 10;
                //memConfig.SocketPool.MaxPoolSize = 200;
                //memConfig.SocketPool.ConnectionTimeout = new TimeSpan(0, 0, 10);
                //memConfig.SocketPool.DeadTimeout = new TimeSpan(0, 0, 30);
               

                var mcc = new MemcachedClientConfiguration();
                mcc.Servers.Add(StringUtility.CreateIPEndPoint("47.92.134.179:11211"));
                mcc.NodeLocator = typeof(DefaultNodeLocator);
                //mcc.KeyTransformer = typeof(SHA1KeyTransformer);
                //mcc.Transcoder = typeof(DefaultTranscoder);
                mcc.SocketPool.MinPoolSize = 10;
                mcc.SocketPool.MaxPoolSize = 100;
                mcc.SocketPool.ConnectionTimeout = new TimeSpan(0, 0, 10);
                mcc.SocketPool.DeadTimeout = new TimeSpan(0, 0, 30);
                MemClient = new MemcachedClient(mcc);
            }
            catch (Exception e)
            {
                var r = e.ToString();
                throw;
            }
        }
    }
}
