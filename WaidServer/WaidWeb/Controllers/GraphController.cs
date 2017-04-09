using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using WaidWeb.Models;

namespace WaidWeb.Controllers
{
    [Authorize]
    public class GraphController : Controller
    {
        // GET: /Graph/Today
        public ActionResult Today()
        {
            SetUserCookie();

            return View();
        }

        // GET: /Graph/TodayInNumbers
        public ActionResult TodayInNumbers()
        {
            SetUserCookie();

            return View();
        }

        // GET: /Graph/AnotherDay
        public ActionResult AnotherDay()
        {
            SetUserCookie();

            return View();
        }

        // GET: /Graph/AnotherDayInNumbers
        public ActionResult AnotherDayInNumbers()
        {
            SetUserCookie();

            return View();
        }

        // GET: /Graph/Feedback
        public ActionResult Feedback()
        {
            SetUserCookie();

            ViewBag.Submitted = false;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitFeedback(string feedback)
        {
            ViewBag.Submitted = true;

            if (!string.IsNullOrWhiteSpace(feedback))
            {
                var fromAddress = new MailAddress("ml@gmail.com", "Mark");
                var toAddress = new MailAddress("ml@gmail.com", "Mark");
                string fromPassword = ConfigurationManager.AppSettings["MailPassword"];
                string subject = "Feedback from - " + User.Identity.Name;
                string body = feedback;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }

            return View("Feedback");
        }

        private void SetUserCookie()
        {
            string userName = User.Identity.Name;

            using (var db = new UsersContext())
            {
                UserProfile user =
                    db.UserProfiles.Single(u => u.UserName.ToLower() == userName.ToLower());
                ViewBag.UploadId = user.UploadId;

                UserIdRepository.Set(userName, user.UploadId, Response);
            }
        }

        // GET: /Graph/DownloadClient
        public ActionResult DownloadClient()
        {
            Guid userId;
            if (UserIdRepository.TryGetUserId(out userId))
            {
                ViewBag.UploadId = userId;
            }

            return View();
        }
    }
}