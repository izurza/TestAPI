//using System.Net.Mail;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using TestAPI.Models;

namespace TestAPI.Funciones
{
    public class MailRender
    {
        public string RenderBody(Guid customerId)
        {
            return $"Hello customer {customerId}!!!";
        }
    }

    public class MailSender
    {
        //private readonly EmailOptions _options;

        //public MailSender(IOptions<EmailOptions> options)
        //{
        //        _options = options.Value;
        //}
        //private readonly IConfiguration _configuration;
        /*public MailSender(IConfiguration configuration) 
        { 
            _configuration = configuration;
        }*/

        private readonly IOptions<EmailOptions> _options;
        private readonly IConfiguration _configuration;
        private readonly MailRender _mailRender;
        public MailSender(IOptions<EmailOptions> options, IConfiguration configuration, MailRender mailRender)
        {
            _options = options;
            _configuration = configuration;
            _mailRender = mailRender;
        }


        public void SendEmailWithMailKitWithOptions()
        {
            var mailMessage = new MimeMessage();

            mailMessage.From.Add(new MailboxAddress(_options.Value.Name, _options.Value.Email));

            mailMessage.To.Add(new MailboxAddress(_options.Value.Name, _options.Value.Email));

            mailMessage.Subject = "Nueva tarea Asignada";

            mailMessage.Body = new TextPart("plain")

            {

                Text = _mailRender.RenderBody(Guid.NewGuid())
            };

            using var smtpClient = new SmtpClient();
            smtpClient.Connect(_options.Value.Host, _options.Value.Port, _options.Value.SSL);
            smtpClient.Authenticate(_options.Value.Email, _options.Value.Password);
            smtpClient.Send(mailMessage);
            smtpClient.Disconnect(true);
        }
        public void SendEmailWithMailKitWithConfiguration()
        {
            var mailMessage = new MimeMessage();

            mailMessage.From.Add(new MailboxAddress(_configuration["EmailConf:Name"], _configuration["EmailConf:Email"]));

            mailMessage.To.Add(new MailboxAddress(_configuration["EmailConf:Name"], _configuration["EmailConf:Email"]));

            mailMessage.Subject = "subject";

            mailMessage.Body = new TextPart("plain")

            {

                Text = _mailRender.RenderBody(Guid.NewGuid())
            };

            using var smtpClient = new SmtpClient();
            smtpClient.Connect(_configuration["EmailConf:Host"], int.Parse(_configuration["EmailConf:Port"]), bool.Parse(_configuration["EmailConf:SSL"]));
            smtpClient.Authenticate(_configuration["EmailConf:Email"], _configuration["EmailConf:Password"]);
            smtpClient.Send(mailMessage);
            smtpClient.Disconnect(true);
        }


        /*public void SendEmail(TodoItem todoItem)
        {
            MailMessage message = new MailMessage();

            message.From = new MailAddress("i.izurza@consulpyme.com", "Desde App", System.Text.Encoding.UTF8);
            message.To.Add("i.izurza@consulpyme.com");
            message.Subject = "Notificación de nueva Actividad ToDo";
            message.Body = string.Format("Buenos dias,\n\n" +
                "Se le notifica que se le ha asignado una nueva tarea ->{0}", todoItem.Name);
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();
            smtp.UseDefaultCredentials = false;
            smtp.Host = "mail.consulpyme.com";
            smtp.Port = 465;            
            smtp.Credentials = new NetworkCredential("i.izurza@consulpyme.com", "TirelessTracker3/2");

            
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };
            
            smtp.EnableSsl = true;
            Console.WriteLine("antes de send");
            smtp.Send(message);
            Console.WriteLine("despues de send");

            //message.From = new MailAddress(_options.Email, "Desde App", System.Text.Encoding.UTF8);
            //message.To.Add(_options.Email);
            //message.Subject = "Notificación de nueva Actividad ToDo";
            //message.Body = string.Format("Buenos dias,\n\n" +
            //    "Se le notifica que se le ha asignado una nueva tarea ->{0}", todoItem.Name);
            //message.IsBodyHtml = true;
            //message.Priority = MailPriority.High;

            //SmtpClient smtp = new SmtpClient();
            //smtp.UseDefaultCredentials = false;
            //smtp.Host = _options.Host;
            //smtp.Port = _options.Port;
            //smtp.Credentials = new NetworkCredential(_options.Email, _options.Password);

            //Console.WriteLine("hola");
            //ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            //{
            //    return true;
            //};
            //smtp.EnableSsl = _options.SSL;
            //smtp.Send(message);


        }*/
    }
}
