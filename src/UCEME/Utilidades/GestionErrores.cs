namespace UCEME.Utilidades
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Text;

    public static class GestionErrores
    {
        public static void RegistroError(string msg, string pagina, string proceso)
        {
            var strOutputFileContents = DateTime.Now + " : " + pagina + " - " + proceso + " - " + msg;
            try
            {
                var fecha = DateTime.Now;
                var dia = string.Format("{0}-{1}-{2}", fecha.Day, fecha.Month, fecha.Year);

                var strFile = Path.GetFullPath(@"c:\temp\Log\Incidenciasgd-" + dia + ".txt");

                if (!File.Exists(strFile))
                {
                    using (var fs = File.Create(strFile)) { }
                }

                var w = File.AppendText(strFile);
                Log(strOutputFileContents, w);
                w.Close();
            }
            catch (Exception e)
            {
                var sError = e.Message;
            }
        }

        public static void RegistroErrorEmail(string msg, string pagina, string proceso)
        {
            var strOutputFileContents = DateTime.Now + " : " + pagina + " - " + proceso + " - " + msg;

            var emailMessage = new StringBuilder();

            emailMessage.Append(strOutputFileContents);

            var emailfrom = System.Configuration.ConfigurationManager.AppSettings["email_from"];
            var email = new MailMessage
            {
                From = new MailAddress(emailfrom),
                Subject = "Uceme Application error",
                Body = emailMessage.ToString(),
                IsBodyHtml = true
            };

            email.To.Add(new MailAddress("juliocejudo@endocrinologia-madrid.com"));

            var credUser = System.Configuration.ConfigurationManager.AppSettings["credential_user"];
            var credPassw = System.Configuration.ConfigurationManager.AppSettings["credential_password"];
            var smtpServer = new SmtpClient
            {
                Host = System.Configuration.ConfigurationManager.AppSettings["host_SMTP"],
                Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["port_SMTP"]),
                Credentials = new NetworkCredential(credUser, credPassw),
                EnableSsl = true
            };

            smtpServer.Send(email);
        }

        private static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now,
                DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
            w.Flush();
        }
    }
}