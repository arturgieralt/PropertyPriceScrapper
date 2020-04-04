using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace MailService
{
    public class MailSender
    {
        private readonly MailSettings _settings;

        public MailSender(MailSettings settings)
        {
            _settings = settings;
        }

        public async Task SendNotificationAsync(string subject, string messageBody)
        {
            var message = new MimeMessage ();
			message.From.Add (new MailboxAddress (_settings.SenderEmail));
			message.To.Add (new MailboxAddress (_settings.ReceiverEmail));
			message.Subject = subject;

			message.Body = new TextPart ("plain") {
				Text = messageBody
			};

			using (var client = new SmtpClient ()) {
				client.Connect (_settings.MailServer, _settings.MailPort, _settings.EnableSsl);
				client.Authenticate (_settings.Login, _settings.Password);

				await client.SendAsync (message);
				
				client.Disconnect (true);
			}
        }
    }
}
