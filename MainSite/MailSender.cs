using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace MainSite
{
    public static class MailSender
    {
        public static void SendMailNotification(DateTime startDate, TimeSpan duration, string userName, string userPhon, List<string> selectedServicesName)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("oli_882011@mail.ru");
#if DEBUG
            mailMsg.To.Add(new MailAddress("astmusresist@gmail.com"));
#else
            mailMsg.To.Add(new MailAddress("olgas882013@gmail.com"));            
#endif
            mailMsg.IsBodyHtml = false;
            mailMsg.Subject = "Запись на " + startDate.ToString();
            mailMsg.Body = userName + " " + Environment.NewLine + String.Join(",", selectedServicesName) + Environment.NewLine + startDate.ToString("dd.MM.yyyy HH:mm") + " тел: " + userPhon;

            SmtpClient client = new SmtpClient("smtp.mail.ru", 25);
            client.Credentials = new System.Net.NetworkCredential() { UserName = "oli_882011@mail.ru", Password = "rusaya8" };
            client.EnableSsl = true;

            client.Send(mailMsg);
        }

        public static void SendDeletedMailNotification(NailDate nailDate)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("oli_882011@mail.ru");
#if DEBUG
            mailMsg.To.Add(new MailAddress("astmusresist@gmail.com"));
#else
            mailMsg.To.Add(new MailAddress("olgas882013@gmail.com"));            
#endif
            mailMsg.IsBodyHtml = false;
            mailMsg.Subject = "Запись на " + nailDate.StartTime.ToString("yyyy.MM.dd HH:mm") + " отменилась";
            mailMsg.Body = nailDate.ToString();

            SmtpClient client = new SmtpClient("smtp.mail.ru", 25);
            client.Credentials = new System.Net.NetworkCredential() { UserName = "oli_882011@mail.ru", Password = "rusaya8" };
            client.EnableSsl = true;

            client.Send(mailMsg);
        }

        public static void SendUpdatedMailNotification(NailDate updatedNailDate)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.From = new MailAddress("oli_882011@mail.ru");
#if DEBUG
            mailMsg.To.Add(new MailAddress("astmusresist@gmail.com"));
#else
            mailMsg.To.Add(new MailAddress("olgas882013@gmail.com"));            
#endif
            mailMsg.IsBodyHtml = false;
            mailMsg.Subject = "Запись на" + updatedNailDate .StartTime + "изменена";
            string body = updatedNailDate.ToString();
            SmtpClient client = new SmtpClient("smtp.mail.ru", 25);
            client.Credentials = new System.Net.NetworkCredential() { UserName = "oli_882011@mail.ru", Password = "rusaya8" };
            client.EnableSsl = true;

            client.Send(mailMsg);
        }
    }
}