using biz.rebel_wings.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.rebel_wings.Services.Email
{
    public interface IEmailService
    {
        string SendEmail(EmailModel email);
        string SendEmailAttach(EmailModelAttach email);
    }
}
