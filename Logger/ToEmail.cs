using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    class ToEmail
    {
        //收件人
        static List<string> addressee = new List<string>() { "414957212@qq.com" };

        public static void SendMail(string title,string message) {

            try
            {
                EmailUtility.SendMailList(title, message, addressee);
            }
            catch (Exception)
            {

            }
        }
    }
}
 