using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.rebel_wings.Models.Email
{
    public class EmailModel
    {
        public string To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; }
        public int id_app { get; set; }
    }
    public class EmailModelAttach
    {
        public string To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public bool IsBodyHtml { get; set; }
        public int id_app { get; set; }
        public List<Files> File { get; set; }
    }

    public class Files
    {
        public string attach { get; set; }
    }

    public class EmailSettings
    {
        public string PrimaryDomain { get; set; }
        public string MailName { get; set; }
        public int PrimaryPort { get; set; }
        public string UsernameEmail { get; set; }
        public string UsernamePassword { get; set; }
        public bool DevTest { get; set; }
        public string MailTest { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public bool EnableSsl { get; set; }

    }
}
