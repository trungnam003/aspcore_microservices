using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services.Email
{
    public class MailRequest
    {
        [EmailAddress]
        public string From { get; set; }

        [EmailAddress]
        public string To { get; set; }

        public IEnumerable<string> ToAddAdress { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public IFormFileCollection Attachments { get; set; }
    }
}
