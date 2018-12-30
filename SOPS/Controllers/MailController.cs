using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web;
using System.Web.Caching;
using System.Web.Http;

namespace SOPS.Controllers
{
    [RoutePrefix("api/Mail")]
    //[Authorize(Roles = "Administrator")]
    public class MailController : ApiController
    {
        private ApplicationDbContext db = null;
        private SmtpClient smtpClient = new SmtpClient("ssl0.ovh.net", 587);
        private static CacheItemRemovedCallback OnCacheRemove = null;
        private static bool IsSendingActivated = false;

        public MailController()
        {
            smtpClient.Credentials = new System.Net.NetworkCredential("sops@antoniuk.pl", "Trudne@haslo");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            OnCacheRemove = CacheItemRemoved;
        }

        // POST: api/Mail/activate
        /// <summary>
        /// wlacz wysylanie maili
        /// niebezpieczne
        /// </summary>
        [Route("activate")]
        public void PostScheduleSending()
        {
            IsSendingActivated = true;
            AddTask("send_mail", 20);
        }

        // POST: api/Mail/deactivate
        /// <summary>
        /// wylacz wylsanie maili
        /// niebezpieczne
        /// </summary>
        [Route("deactivate")]
        public async void PostStopSendingAsync()
        {
            //IsSendingActivated = false;
            var dateTime = DateTime.Now;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("sops@antoniuk.pl", "SOPS");
            mail.To.Add(new MailAddress("sops@antoniuk.pl"));
            //mail.To.Add(new MailAddress("michasacuer3@gmail.com"));
            //mail.To.Add(new MailAddress("pawel@antoniuk.pl"));
            //mail.To.Add(new MailAddress("skrzynkanof@gmail.com"));
            //mail.To.Add(new MailAddress("joliniak@vp.pl"));

            mail.Subject = "Okresowy raport";
            mail.Body = "W załączniku znajduje się automatycznie wygenerowany raport dotyczący działalności firmy, której otrzymujący wiadomość jest pracownikiem.\n\n" +
                "Mail został napisany i wysłany automatycznie. Proszę na niego nie odpowiadać.\n\n" +
                "Pozdrawiamy,\n" +
                "©System Rejestracji Produktów Sporzywczych";

            var controller = new DocumentController();
            controller.ControllerContext = ControllerContext;

            Stream stream = await controller.GetReport(59).Result.Content.ReadAsStreamAsync();
            var attachmentName = "Raport(" + dateTime.ToString("dd.MM.yy") + ").pdf";
            mail.Attachments.Add(new Attachment(stream, attachmentName, "application/pdf"));

            smtpClient.Send(mail);
        }

        private void AddTask(string name, int seconds)
        {
            HttpRuntime.Cache.Insert(name, seconds, null, DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, OnCacheRemove);
        }

        private void CacheItemRemoved(string k, object v, CacheItemRemovedReason r)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("sops@antoniuk.pl", "SOPS");

            db = new ApplicationDbContext();
            var userEmployees = db.Users.Where(u => db.Employees.Any(e => e.UserId == u.Id));
            /*
            foreach (var userEmployee in userEmployees)
            {
                if (userEmployee.EmailConfirmed)
                {
                    mail.To.Add(new MailAddress(userEmployee.Email));
                }
                //mail.To.Add(new MailAddress(userEmployee.Email));
            }
            */
            mail.To.Add(new MailAddress("sops@antoniuk.pl"));
            //("michasacuer3@gmail.com"));
            //("skrzynkanof@gmail.com"));

            /*
            mail.Subject = "Temat";
            mail.Body = "Ala ma dłonie";
            mail.Attachments.Add()
            */

            smtpClient.Send(mail);

            if (IsSendingActivated)
            {
                AddTask(k, Convert.ToInt32(v));
            }
        }
    }
}
