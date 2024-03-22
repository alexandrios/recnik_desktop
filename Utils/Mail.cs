using System;
using System.Net;
using System.IO;
using System.Net.Mail;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using MimeKit;

namespace SRWords
{
    public static class Mail
    {
        public static void SendMail()
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("alex27@mail.ru", "Recnik admin");

            // кому отправляем
            //MailAddress to = new MailAddress("alexandrios273@gmail.com");
            //MailAddress to = new MailAddress("peju@mail.ru");
            MailAddress to = new MailAddress("alex27@mail.ru");

            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to)
            {
                // тема письма
                Subject = "Тест",

                // текст письма
                Body = "<h2>Письмо-тест работы smtp-клиента</h2><p>Здравствуйте! Ваша заявка принята.",

                // письмо представляет код html
                IsBodyHtml = true
            };

            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.mail.ru", 25) //587) //25)
            {
                // логин и пароль (пароль для внешнего приложения "Send mails from recnik desktop")
                Credentials = new NetworkCredential("alex27@mail.ru", "SJJsu1ASYU0irwitfirh"),
                EnableSsl = true
            };

            smtp.Send(m);
            Console.Read();
        }
    }
}
