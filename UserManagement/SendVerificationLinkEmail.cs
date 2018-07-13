using System;
using System.Net;
using System.Net.Mail;

namespace UserManagement
{
    public class SendVerificationLinkEmail
    {
        public static void SendEmail() { }

        public static void CreateBccTestMessage(string server, string activationcode, string email, string scheme, string host, string port)
        {
            var varifyUrl = scheme + "://" + host + ":" + port + "/JobSeeker/ActivateAccount/" + activationcode;

            MailAddress from = new MailAddress("shtatev4991@gmail.com", "Kanch Admin");
            MailAddress to = new MailAddress(email, "Dear Traveler");
            MailMessage message = new MailMessage(from, to)
            {
                Subject = "Your account is successfull created.",
                Body = @"<br/><br/>We are excited to tell you that your account is" +
        " successfully created. Please click on the below link to verify your account" +
        " <br/><br/><a href='" + varifyUrl + "'>" + varifyUrl + "</a> "
            };

            MailAddress bcc = new MailAddress("manager1@contoso.com");
            message.Bcc.Add(bcc);
            SmtpClient client = new SmtpClient(server)
            {
                Credentials = CredentialCache.DefaultNetworkCredentials
            };

            Console.WriteLine("Sending an e-mail message to {0} and {1}.",
                to.DisplayName, message.Bcc.ToString());

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateBccTestMessage(): {0}",
                            ex.ToString());
            }
        }
    }
}
