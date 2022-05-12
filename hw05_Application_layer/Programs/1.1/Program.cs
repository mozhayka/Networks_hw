using System;
using System.Net.Mail;

namespace _1._1
{
    class Program
    {
        static void Send(string from, string to, string subject, string message, bool isBodyHtml,
            string server, int port, string username, string password)
        {
            try
            {
                MailMessage mail = new();
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.IsBodyHtml = isBodyHtml;
                mail.Body = message;

                using SmtpClient smtp = new(server, port);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.Credentials = new System.Net.NetworkCredential(username, password);

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        static void SendMail(string to = "mozhay239@mail.ru")
        {
            string from = "st075839@student.spbu.ru";
            Console.WriteLine($"Type password for {from}");
            string pass = Console.ReadLine();
            string server = "mail.spbu.ru";
            int port = 25;

            Send(from, to, "Test", "Hello world!", 
                false, server, port, 
                from, pass);
        }

        static void Main(string[] args)
        {
            SendMail();
        }
    }
}
