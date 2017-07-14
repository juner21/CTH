using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    public class StringUtility
    {
        public static bool IsEmail(string content)
        {
            return Regex.IsMatch(content, @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        }

        public static bool IsTelephone(string telephone)
        {
            return Regex.IsMatch(telephone, @"^[1][3-8]\d{9}$");

        }

        public static bool IsInt(string content)
        {
            return Regex.IsMatch(content, @"^-?\d+$");
        }

        /// <summary>
        /// 正数
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static bool IsDouble(string content)
        {
            return Regex.IsMatch(content, @"^([1-9]\d*)(\.\d*)?$");
        }

        /// <summary>
        /// 电话号码验证（包括 移动和固定电话）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsPhone(string str)
        {
            //((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)
            return Regex.IsMatch(str, @"(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}");

        }

        public static bool IsGuid(string str)
        {
            return Regex.IsMatch(str, @"^[0-9a-f]{8}(-[0-9a-f]{4}){3}-[0-9a-f]{12}$", RegexOptions.IgnoreCase);

        }

        public static string CutString(string str, int len)
        {
            if (string.IsNullOrEmpty(str) || len <= 0)
            {
                return string.Empty;
            }

            int l = str.Length;

            #region 计算长度
            int clen = 0;
            while (clen < len && clen < l)
            {
                //每遇到一个中文，则将目标长度减一。
                if ((int)str[clen] > 128) { len--; }
                clen++;
            }
            #endregion

            if (clen < l)
            {
                return str.Substring(0, clen) + "...";
            }
            else
            {
                return str;
            }
        }

        public static string RemoveHTML(string str)
        {
            string regexstr = "<[^>]*>";
            return Regex.Replace(str, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

        public static string NoHTML(string Htmlstring) //去除HTML标记  
        {
            //删除脚本  
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML  
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = Htmlstring.Trim();
            return Htmlstring;
        }

        public static DateTime GetStartTime(string startTime)
        {
            if (string.IsNullOrEmpty(startTime))
                return DateTime.MinValue;
            var st = DateTime.MinValue;
            DateTime.TryParse(startTime + " 00:00:00", out st);
            return st;
        }
        public static DateTime GetEndTime(string endTime)
        {
            if (string.IsNullOrEmpty(endTime))
                return DateTime.MinValue;
            var et = DateTime.MinValue;
            DateTime.TryParse(endTime + " 23:59:59", out et);
            return et;
        }

        public static string NewGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Trim().ToLower();
        }

        public static string Left(string str, int len, string end = "")
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.Length > len)
            {
                return str.Substring(0, len) + end;
            }
            return str;
        }

        public static double[] DoubleArrayParse(string text)
        {
            var tags = new List<double>();
            if (!string.IsNullOrEmpty(text))
            {
                foreach (var t in text.Split(',', '，', ' ').Where(n => !string.IsNullOrWhiteSpace(n)).Select(n => n.Trim()))
                {
                    var tt = double.MinValue;
                    double.TryParse(t, out tt);
                    if (tt != double.MinValue)
                        tags.Add(tt);
                }
            }

            return tags.ToArray();
        }

        public static int[] IntArrayParse(string text)
        {
            var tags = new List<int>();
            if (!string.IsNullOrEmpty(text))
            {
                foreach (var t in text.Split(',', '，', ' ').Where(n => !string.IsNullOrWhiteSpace(n)).Select(n => n.Trim()))
                {
                    var tt = int.MinValue;
                    int.TryParse(t, out tt);
                    if (tt != int.MinValue)
                        tags.Add(tt);
                }
            }

            return tags.ToArray();
        }

        public static List<string> StringArrayParse(string text)
        {
            var tags = new List<string>();
            if (!string.IsNullOrEmpty(text))
            {
                foreach (var t in text.Split(',', '，', ' ').Where(n => !string.IsNullOrWhiteSpace(n)).Select(n => n.Trim()))
                {

                    if (!string.IsNullOrEmpty(t))
                        tags.Add(t.Trim());
                }
            }

            return tags;
        }

        public static List<ulong> LongArrayParse(string text)
        {
            var tags = new List<ulong>();
            if (!string.IsNullOrEmpty(text))
            {
                foreach (var t in text.Split(',', '，', ' ').Where(n => !string.IsNullOrWhiteSpace(n)).Select(n => n.Trim()))
                {

                    if (!string.IsNullOrEmpty(t))
                        tags.Add(ulong.Parse(t));
                }
            }

            return tags;
        }

        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }



        /// <summary>
        /// 获取指定日期，在为一年中为第几周
        /// </summary>
        /// <param name="dt">指定时间</param>
        /// <reutrn>返回第几周</reutrn>
        public static int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            return weekOfYear;
        }

        /// <summary>
        /// Unicode解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FromUnicodeString(string str)
        {
            //最直接的方法Regex.Unescape(str);
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                string[] strlist = str.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < strlist.Length; i++)
                    {
                        int charCode = Convert.ToInt32(strlist[i], 16);
                        strResult.Append((char)charCode);
                    }
                }
                catch (FormatException ex)
                {
                    return Regex.Unescape(str);
                }
            }
            return strResult.ToString();
        }

        /// <summary>
        /// 随机大小写字母加数字
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandom(int length)
        {
            var constant = new List<string>()
            {
                "0","1","2","3","4","5","6","7","8","9", "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
            };

            var newRandom = new StringBuilder(62);
            var rd = new Random();
            for (var i = 0; i < length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        public static string GetMd5(string str)
        {
            MD5 md = MD5CryptoServiceProvider.Create();
            byte[] hash;

            UTF8Encoding enc = new UTF8Encoding();

            byte[] buffer = enc.GetBytes(str);

            hash = md.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 创建IP
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static IPEndPoint CreateIPEndPoint(string endPoint)
        {
            string[] ep = endPoint.Split(':');
            if (ep.Length != 2) throw new FormatException("Invalid endpoint format");
            IPAddress ip;
            if (!IPAddress.TryParse(ep[0], out ip))
            {
                throw new FormatException("Invalid ip-adress");
            }
            int port;
            if (!int.TryParse(ep[1], NumberStyles.None, NumberFormatInfo.CurrentInfo, out port))
            {
                throw new FormatException("Invalid port");
            }
            return new IPEndPoint(ip, port);
        }
    }
}
