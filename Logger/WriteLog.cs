using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Common;
using System.Reflection;
using System.ComponentModel;

namespace Logger
{
    public static class WriteLog
    {
        private static Dictionary<int,StringBuilder> sbDic = null;

        private static DateTime lastTime = DateTime.Now;

        private static object _lock = new object();

        static WriteLog() {

            foreach (var item in Enum.GetValues(typeof(LogPath)))
            {
                var r = typeof(LogPath).GetField(item.ToString()).GetCustomAttribute<DescriptionAttribute>().Description;

                sbDic.Add(item.GetHashCode(), new StringBuilder());
            }

        }

        public static void Write(LogModel logModel) {
            
            WriteCache(logModel);
        }

        private static void WriteTxt(string message, string path)
        {
            string fileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

            string pathAll = path + fileName;

            try
            {
                if (!File.Exists(pathAll))
                {
                    Directory.CreateDirectory(path);
                }

                System.IO.File.AppendAllText(pathAll, message);

            }
            catch (Exception e)
            {
                ToEmail.SendMail("[Log日志写入错误]",e.ToString());
                throw;
            }
        }

        private static void WriteCache(LogModel logModel)
        {
            try
            {
                var sb = sbDic[logModel.LogPath.GetHashCode()];

                sb.Append(JsonUtility.Serialize(logModel) +",");

                if (logModel.WaringLevel.GetHashCode()>1)
                {
                    ToEmail.SendMail(logModel.Title, logModel.Message);
                }

                if (sb.Length > 10000 || DateTime.Now > lastTime.AddMinutes(10) || !logModel.IsWriteCache)
                {
                    lock (_lock)
                    {
                        WriteTxt(sb.ToString(), logModel.LogPathStr);
                        sb.Clear();
                    }
                }

                lastTime = DateTime.Now;
            }
            catch (Exception e)
            {
                ToEmail.SendMail("[Log日志写入错误]", e.ToString());
                throw;
            }

           
        }
        

    }
}
