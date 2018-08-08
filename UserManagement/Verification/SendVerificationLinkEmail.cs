using System.Net;
using System.Net.Mail;

namespace UserManagement.Verification
{
    public class SendVerificationCodeEmail
    {
        private NetworkCredential _networkCredential;

        /// <summary>
        /// SMTP client
        /// </summary>
        private SmtpClient _smtpClient;

        /// <summary>
        /// Creates new instance of MailService
        /// </summary>
        /// <param name="networkCredential">Network credentials</param>
        public SendVerificationCodeEmail(NetworkCredential networkCredential)
        {
            // constructing 
            this._networkCredential = networkCredential;

            this._smtpClient = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = networkCredential,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        /// <summary>
        /// Sends verification key to the given mail address,
        /// </summary>
        /// <param name="to">Mail address</param>
        /// <param name="verifyKey">Verifiaction key</param>
        public void Send(string to, string verifyKey)
        {
            // constructing message
            var mail = new MailMessage
            {
                From = new MailAddress("no-reply.recipeverify@gmail.com"),
                Subject = "Recipe User Verify Service",
                Body = verifyKey
            };

            mail.To.Add(to);

            // sending message
            this._smtpClient.Send(mail);
        }
    }
}
