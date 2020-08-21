
using Contacto.Models;
using Contacto.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

using System.Net.Mail;

namespace Contacto.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;
        public EmailController(IEmailSender emailSender, IHostingEnvironment env)
        {
            _emailSender = emailSender;
        }
        public IActionResult SendEmail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SendEmail(EmailModel email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    TesteEnvioEmail(email.Name, email.From, email.Subject, email.Message).GetAwaiter();
                    return RedirectToAction("EmailSent");
                }
                catch (Exception)
                {
                    return RedirectToAction("EmailNotSent");
                }
            }
            return View(email);
        }
        public async Task TesteEnvioEmail(string name, string email, string subject, string message)
        {
            try
            {
                //email destino, assunto do email, mensagem a enviar
                await _emailSender.SendEmailAsync(name, email, subject, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult EmailSent()
        {
            return View();
        }
        public ActionResult EmailError()
        {
            return View();
        }
    }
}
