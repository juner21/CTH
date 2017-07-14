using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EmailUtility
    {
        private static string fromUser = "414957212@qq.com";

        private static string fromUserPwd = "hanfei0410";


        public static void SendMail(string title ,string message,string toUser)
        {

            try
            {
                var email = new MailMessage
                {
                    From = new MailAddress(fromUser),
                    To = { new MailAddress(fromUser) }
                };

                email.CC.Add(new MailAddress(toUser));

                email.Subject = title;

                email.IsBodyHtml = true;

                var msg = new StringBuilder();
                msg.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                msg.Append("<br><br>");
                msg.Append(message);

                email.Body = msg.ToString();

                var smtpClient = new SmtpClient("smtp.exmail.qq.com")
                {
                    Credentials = new System.Net.NetworkCredential(fromUser, fromUserPwd)

                };

                smtpClient.Send(email);
            }
            catch (Exception)
            {

            }
        }


        public static void SendMailList(string title, string message, List<string> toUserList)
        {

            try
            {
                var email = new MailMessage
                {
                    From = new MailAddress(fromUser),
                    To = { new MailAddress(fromUser) }
                };

                toUserList.ForEach(e => email.CC.Add(new MailAddress(e)));

                email.Subject = title;

                email.IsBodyHtml = true;

                var msg = new StringBuilder();
                msg.Append(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
                msg.Append("<br><br>");
                msg.Append(message);

                email.Body = msg.ToString();

                var smtpClient = new SmtpClient("smtp.exmail.qq.com")
                {
                    Credentials = new System.Net.NetworkCredential(fromUser, fromUserPwd)

                };

                smtpClient.Send(email);
            }
            catch (Exception)
            {

            }
        }
    }
}
