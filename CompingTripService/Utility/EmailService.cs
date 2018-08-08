using System.Net;
using System.Net.Mail;

namespace CampingTripService.Utility
{
    public class EmailService
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
        public EmailService(NetworkCredential networkCredential)
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
        /// <param name="tripPlace">Verifiaction key</param>
        public void Send(string to, string trip)
        {
            // constructing message
            var mail = new MailMessage
            {
                From = new MailAddress("no-reply.recipeverify@gmail.com"),
                Subject = "Kanc",
                Body = $"Your campaign data is incorrect. \n {trip}"
            };

            mail.To.Add(to);

            // sending message
            this._smtpClient.Send(mail);
        }
    }
}
