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
        [Route("activate")]
        public void PostScheduleSending()
        {
            IsSendingActivated = true;
            AddTask("send_mail", 20);
        }

        // POST: api/Mail/deactivate
        [Route("deactivate")]
        public async void PostStopSendingAsync()
        {
            IsSendingActivated = false;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("sops@antoniuk.pl", "SOPS");

            mail.To.Add(new MailAddress("sops@antoniuk.pl"));
            
            mail.Subject = "Temat";
            mail.Body = "Ala ma dłonie";

            var controller = new DocumentController();
            controller.ControllerContext = ControllerContext;

            Stream stream = await controller.GetReport(71).Content.ReadAsStreamAsync();
            mail.Attachments.Add(new Attachment(stream, "Raport.pdf", null));

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
            foreach (var userEmployee in userEmployees)
            {
                if (userEmployee.EmailConfirmed)
                {
                    //mail.To.Add(new MailAddress(userEmployee.Email));
                }
                // mail.To.Add(new MailAddress(userEmployee.Email));
            }
            mail.To.Add(new MailAddress("sops@antoniuk.pl"));
            //("michasacuer3@gmail.com"));
            //("skrzynkanof@gmail.com"));

            mail.Subject = "Temat";
            mail.Body = "Ala ma dłonie";
            // mail.Attachments.Add()

            smtpClient.Send(mail);

            if (IsSendingActivated)
            {
                AddTask(k, Convert.ToInt32(v));
            }
        }
    }
}
