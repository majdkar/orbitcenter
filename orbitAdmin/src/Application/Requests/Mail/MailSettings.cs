﻿
namespace SchoolV01.Application.Requests.Mail
{
    public class MailSettings
    {
        public string DisplayName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }       
        public bool UseStartTls { get; set; }
    }
}
