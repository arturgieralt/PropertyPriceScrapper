namespace MailService
{
    public class MailSettings
    {
        public string MailServer { get; set; }
        public int MailPort { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }
        public string EmailKey { get; set; }
    }
}