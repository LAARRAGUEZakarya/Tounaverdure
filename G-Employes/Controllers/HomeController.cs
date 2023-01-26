using G_Employes.Areas.Identity.Data;

using GestionEmployes.Models;
using GestionEmployes.Models.G_Stock;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using G_Employes.Models;
using System.Net.Mail;
using System.Net;

namespace G_Employes.Controllers
{
 [AllowAnonymous]
    public class HomeController : Controller
    {
       
 
        public IActionResult Index()
        {


            //serach for user sign in:



            return View();
        }

        public IActionResult services()
        {
            return View();
        }
        public IActionResult service(string detail)
        {
            return View();
        }
        public IActionResult service2(string detail)
        {
            return View();
        }
        public IActionResult service3(string detail)
        {
            return View();
        }


        public IActionResult contact()
        {
            return View();
        }


        [HttpPost]  
        public IActionResult contact(string name, string email, string subject, string message)
        {
            SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new NetworkCredential("tuna-vr@outlook.com", "testtuna123@");
            MailMessage msg = new MailMessage("tuna-vr@outlook.com", "tuna-vr@outlook.com");
            msg.Subject = subject;
            msg.IsBodyHtml = true;

            msg.Body = "From: " + name + "<br>E-mail: " + email + "</br>Message : " + message  ;
            smtp.Send(msg);

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
