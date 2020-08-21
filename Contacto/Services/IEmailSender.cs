using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contacto.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string name, string email, string subject, string message);
    }
}
