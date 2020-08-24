using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Contacto.Services
{
    public class AuthMessageSender : IEmailSender
    {
        public AuthMessageSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public EmailSettings _emailSettings { get; }

        public Task SendEmailAsync(string name, string email, string subject, string message)
        {
            try
            {
                Execute(name, email, subject, message).Wait();
                return Task.FromResult(0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task Execute(string name, string email, string subject, string message)
        {
            try
            {
                string toEmail = string.IsNullOrEmpty(email) ? _emailSettings.ToEmail : email;
        

                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.UsernameEmail, "COSHIPIGUA")
                };

                mail.To.Add(new MailAddress(toEmail));
                //mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

                mail.Subject = "Hola, " + name;

               //mail.Body = "<table style='max - width: 600px; padding: 10px; margin: 0 auto; border - collapse: collapse; '><tr><td style='background - color: #55b2c1; text-align: left; padding: 0'><a href='https://www.facebook.com/Coshipigua/'></a></td></tr><tr><td style='padding: 0'> <img style='padding: 0; display: block' src='https://i.postimg.cc/G3PKR6wZ/Coshipigua.jpg' width='100%'> <a href='https://www.facebook.com/Coshipigua/'></td></tr><tr><td style='background-color: #ecf0f1'><div style='color: #34495e; margin: 4% 10% 2%; text-align: justify;font-family: sans-serif'><h2 style='color: #e67e22; margin: 0 0 7px'>Hola," + name + " </h2><p style='margin: 2px; font-size: 15px'>Nos alegra recibir un mensaje tuyo, en breve resolveremos tu duda.</p><div style='width: 100%; text-align: center; margin-top: 40px'><a style='text-decoration: none; border-radius: 5px; padding: 11px 23px; color: white; background-color: #3498db' href='https://www.instagram.com/Coshipigua'>Ir a la página</a></div><p style='color: #b3b3b3; font-size: 12px; text-align: center;margin: 30px 0 0'>Coshipigua 2020</p></div></td></tr></table>";

                string Body = "<table style='max - width: 600px; padding: 10px; margin: 0 auto; border - collapse: collapse; '><tr><td style='background - color: #55b2c1; text-align: left; padding: 0'></td></tr><tr><td style='padding: 0'><img style='padding: 0; display: block' src='cid:imageId' width='100%'></td></tr><tr><td style='background-color: #ecf0f1'><div style='color: #34495e; margin: 4% 10% 2%; text-align: justify;font-family: sans-serif'><h2 style='color: #e67e22; margin: 0 0 7px'>Hola, " + name + " </h2><p style='margin: 2px; font-size: 15px'>Nos alegra recibir un mensaje tuyo, en breve resolveremos tu duda.</p><div style='width: 100%; text-align: center; margin-top: 40px'><a style='text-decoration: none; border-radius: 5px; padding: 11px 23px; color: white; background-color: #3498db' href='https://www.instagram.com/Coshipigua'>Ir a la página</a></div><p style='color: #b3b3b3; font-size: 12px; text-align: center;margin: 30px 0 0'>Coshipigua 2020</p></div></td></tr><tr><td style='background - color: #55b2c1; text-align: left; padding: 0'></td></tr><tr><td style='padding: 0'><img style='padding: 0; display: block' src='cid:imageId' width='100%'></td></tr></table>";

                //string Body = "<h2>Hola, " + name + ", mira ésta gran oferta:</h2>" + "</br>" + "<img src='cid:imageId' />";
         

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");

                LinkedResource header = new LinkedResource(@"C:\Users\ADMIN\Documents\Visual Studio 2019\Projects\Contact\Contacto\Contacto\wwwroot\img\header.jpg", "image/jpg");

                header.ContentId = "imageId";

                header.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

                htmlView.LinkedResources.Add(header);



                mail.AlternateViews.Add(htmlView);

                mail.IsBodyHtml = true;

                mail.Priority = MailPriority.High;

                //otras opciones
                //mail.Attachments.Add(new Attachment(@"C:\Users\ADMIN\Documents\Visual Studio 2019\Projects\Contact\Contacto\Contacto\wwwroot\img\laptopVenta.jpg"));

                //Y lo enviamos a través del servidor SMTP...
                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
