using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;
using System.Net.NetworkInformation;

namespace Demo.PL.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var clinet = new SmtpClient("smtp.gmail.com", 587);
            clinet.EnableSsl = true;
            clinet.Credentials = new NetworkCredential("sarahEldaly230@gmail.com", "qzpxydiojpnoctej");
            clinet.Send("saraEldaly230@gmail.com ", email.To, email.Subject, email.Body);
        }
    }
}
