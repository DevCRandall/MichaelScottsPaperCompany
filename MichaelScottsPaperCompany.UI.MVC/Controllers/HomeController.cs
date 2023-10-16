using MichaelScottsPaperCompany.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MimeKit;
using MailKit.Net.Smtp;
using MichaelScottsPaperCompany.Models;

namespace MichaelScottsPaperCompany.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _Configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _Configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Send email if valid
                string message = $"You have received a new email from your site's contact form!<br/>" +
                $"Sender: {model.FirstName} {model.LastName}<br/>Email: {model.Email}<br/>Subject: {model.Subject}<br/>" +
                $"Message: {model.Message}";

                var Msg = new MimeMessage();
                Msg.From.Add(new MailboxAddress("No Reply", _Configuration.GetValue<string>("Credentials:Email:User")));

                Msg.To.Add(new MailboxAddress("Personal", _Configuration.GetValue<string>("Credentials:Email:Recipient")));

                Msg.Subject = model.Subject;
                Msg.Body = new TextPart("HTML") { Text = message };

                Msg.Priority = MessagePriority.Urgent;

                Msg.ReplyTo.Add(new MailboxAddress(model.FirstName + " " + model.LastName, model.Email));

                using (var Client = new SmtpClient())
                {
                    Client.Connect(_Configuration.GetValue<string>("Credentials:Email:Client"));

                    Client.Authenticate(_Configuration.GetValue<string>("Credentials:Email:User"),
                                        _Configuration.GetValue<string>("Credentials:Email:Password"));

                    // It's possible the mail server could be down when the user tries to contact us,
                    // so we can encapsulate our code to send the message in a try/catch.
                    try
                    {
                        Client.Send(Msg);
                    }
                    catch (Exception ex)
                    {

                        // If there is an issue we can store an error message
                        // in a ViewBag variable to be displayed in the View.
                        ViewBag.ErrorMessage = $"There was an error processing your request." +
                            $"Please try again later.<br/>" +
                            $"Error Message: {ex.StackTrace}";

                        // Return the user to the View with their form info intact.
                        return View(model);
                    }
                }


                return RedirectToAction(nameof(ThankYou));

            }

            return View(model);
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}