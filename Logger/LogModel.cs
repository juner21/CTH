using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Logger
{
    public class LogModel
    {
        static LogModel() {

            foreach (var item in Enum.GetValues(typeof(LogPath)))
            {
                var r = typeof(LogPath).GetField(item.ToString()).GetCustomAttribute<DescriptionAttribute>().Description;
                PathStrDic.Add(item.GetHashCode(), r);
            }

            var appDomain = System.AppDomain.CurrentDomain.BaseDirectory + "Log/";
            PathStrDic[LogPath.AppDomain.GetHashCode()] = appDomain;
        }


        public LogModel(string title, string message, WaringLevelEnum waringLevel, LogPath logPath = LogPath.Default)
        {
            this.Title = title;
            this.Message = message;
            this.WaringLevel = waringLevel;
            this.WriteDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            this.IsWriteCache = true;
            this.LogPathStr = PathStrDic[logPath.GetHashCode()];
            this.LogPath = logPath;
        }

        private static Dictionary<int, string> PathStrDic = new Dictionary<int, string>();

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否写入缓存，默认为true（false，直接写入txt）
        /// </summary>
        public bool IsWriteCache { get; set; }

        /// <summary>
        /// 写入的时间
        /// </summary>
        protected string WriteDateTime{get;set;}

        /// <summary>
        /// 参数字典
        /// </summary>
        public Dictionary<string, string> ParamsDic { get; set; }
        
        /// <summary>
        /// 报警等级
        /// </summary>
        public WaringLevelEnum WaringLevel { get; set; }

        /// <summary>
        /// 预留字段
        /// </summary>
        public string Ex { get; set; }

        /// <summary>
        /// Log存放路径
        /// </summary>
        public string LogPathStr { get; set; }

        public LogPath LogPath { get; set; }
    }

    /// <summary>
    /// 每个枚举会对应开辟一个新的StringBuilder缓存，请勿乱加
    /// </summary>
    public enum LogPath {

        [Description("")]
        AppDomain = -1,
        [Description("D://Log//")]
        Default = 0,
        [Description("C://Log//")]
        ToC = 1,

    }
}
