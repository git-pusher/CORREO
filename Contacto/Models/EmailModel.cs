using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Contacto.Models
{
    public class EmailModel
    {
        [Required, Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required, Display(Name = "Ingrese su correo electrónico"), EmailAddress]
        public string From { get; set; }

        [Required, Display(Name = "Asunto")]
        public string Subject { get; set; }

        [Required, Display(Name = "Escriba aquí su mensaje")]
        public string Message { get; set; }
    }
}
