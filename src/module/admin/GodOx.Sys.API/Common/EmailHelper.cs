using MailKit.Net.Smtp;
using MimeKit;
using System;
namespace GodOx.Sys.API.Common
{

    public class EmailHelper
    {
        /// <summary>
        /// 发送人
        /// </summary>
        public static string FromName { get; set; }

        /// <summary>
        /// 发送邮件地址
        /// </summary>
        public static string FromAddress { get; set; }
        /// <summary>
        /// 授权码
        /// </summary>
        public static string AuthCode { get; set; }

        /// <summary>
        /// 邮件发送
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        /// <param name="toName"></param>
        /// <param name="toAddress"></param>
        public static void SendEmail(string subject, string content, string toName, string toAddress)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(FromName, FromAddress));
                if (!string.IsNullOrEmpty(toName) && !string.IsNullOrEmpty(toAddress))
                {
                    message.To.Add(new MailboxAddress(toName, toAddress));
                }
                else
                {
                    message.To.Add(new MailboxAddress("mhg", "mahonggang8888@126.com"));
                }
                message.Subject = subject;
                message.Body = new TextPart("plain")
                {
                    Text = content
                };
                using (var client = new SmtpClient())
                {
                    //client.QueryCapabilitiesAfterAuthenticating = false;
                    client.Connect("smtp.qq.com", 587, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    // Note: since we don't have an OAuth2 token, disable 	
                    // the XOAUTH2 authentication mechanism.     
                    client.Authenticate(FromAddress, AuthCode);
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                // LogHelper.Default.ProcessError("邮件发送", ex.Message);
            }
        }
    }
}
