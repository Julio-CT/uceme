namespace UCEME.Utilidades
{
    using System;
    using System.Diagnostics;
    using System.Net;
    using System.Net.Mail;
    using System.Text;

    public class Notificaciones
    {
        public static bool SendPasswordRetrieval(string emailAddress, string token)
        {
            try
            {
                var emailMessage = new StringBuilder();

                emailMessage.Append("<br />");
                emailMessage.Append("Hola,");
                emailMessage.Append("Ha pedido que se recupere su contaseña.");
                emailMessage.Append("<br />");
                emailMessage.Append("Por favor, haga click en el enlace para resetarla: <br />");
                emailMessage.Append("<br />");
                var urlNuevaPass = string.Format("{0}/Seguridad/Validate?email={1}&token={2}", System.Configuration.ConfigurationManager.AppSettings["url_actual"], emailAddress, token);
                emailMessage.Append(string.Format("<a href='{0}'>Pulse aquí</a>", urlNuevaPass));
                emailMessage.Append("<br />");
                emailMessage.Append("Por favor, no responda a este correo, si no ha pedido la recuperacion de la contaseña, ignorelo. <br />");
                emailMessage.Append("<br />");

                var subject = "Recuperacion Password UCEME";
                SendEmail(emailAddress, emailMessage, subject);

                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(string.Format("Failure to send email to {0}. Error {1}.", emailAddress, e.Message));
                return false;
            }
        }

        public static bool SendNewUserCreated(string emailAddress, string token)
        {
            try
            {
                var emailMessage = new StringBuilder();

                emailMessage.Append("<br />");
                emailMessage.Append("Hola,");
                emailMessage.Append("Has creado una nueva cuenta en UCEME.");
                emailMessage.Append("<br />");
                emailMessage.Append("Por favor, haz click en el enlace inferior para cambiar tu contraseña: <br />");
                emailMessage.Append("<br />");
                var urlNuevaPass = string.Format("{0}/Seguridad/Validate?email={1}&token={2}", System.Configuration.ConfigurationManager.AppSettings["url_actual"], emailAddress, token);
                emailMessage.Append(string.Format("<a href='{0}'>Pulsa aquí</a>", urlNuevaPass));
                emailMessage.Append("<br />");

                var subject = "Recuperacion Password UCEME";
                SendEmail(emailAddress, emailMessage, subject);

                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(string.Format("Failure to send email to {0}. Error {1}.", emailAddress, e.Message));
                return false;
            }
        }

        public static bool EnviarCorreoContacto(string emailAddress, string nombre, string texto)
        {
            try
            {
                var emailto = System.Configuration.ConfigurationManager.AppSettings["credential_user"];
                var emailMessage = new StringBuilder();

                emailMessage.Append("<br />");
                emailMessage.Append(nombre + " ha rellenado el formulario de contacto de UCEME: ");
                emailMessage.Append("<br />");
                emailMessage.Append("<br />");
                emailMessage.Append(texto);

                var email = new MailMessage();
                var emailfrom = System.Configuration.ConfigurationManager.AppSettings["email_from"];
                email.From = new MailAddress(emailfrom);
                email.To.Add(new MailAddress(emailto));
                email.Subject = "Formulario de contacto enviado a UCEME";
                email.Body = emailMessage.ToString();
                email.IsBodyHtml = true;

                var subject = "Formulario de contacto enviado a UCEME";
                SendEmail(emailto, emailMessage, subject);

                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(string.Format("Failure to send email to {0}. Error {1}.", emailAddress, e.Message));
                return false;
            }
        }

        public static bool ModificarCitasMedicos(Uceme.Model.Models.Cita cita)
        {
            string emailAddress = null;
            try
            {
                var nombre = cita.nombre;
                var dia = Utilidades.DiasHoras.EuropeanDay(cita.dia);
                var hora = Utilidades.DiasHoras.TimeToString(cita.hora);
                if (cita.email != null)
                {
                    emailAddress = cita.email;
                }

                var telefono = cita.telefono;
                var hospital = cita.Turno.DatosProfesionales.nombre;
                var emailto = System.Configuration.ConfigurationManager.AppSettings["credential_user"];

                var emailMessage = new StringBuilder();

                emailMessage.Append("<br />");
                emailMessage.Append("Ha eliminado o modificado un turno de UCEME: ");
                emailMessage.Append("<br />");
                emailMessage.Append("El paciente " + nombre + " tenia una cita el dia " + dia + " a las " + hora + " en el hospital " + hospital);
                emailMessage.Append("<br />");
                emailMessage.Append("Para darle nueva cita puede contactar por telefono: " + telefono);
                emailMessage.Append("<br />");
                if (emailAddress != null)
                {
                    emailMessage.Append("O bien por email: " + emailAddress);
                }

                var subject = "Necesidad de reagendar cita";
                SendEmail(emailto, emailMessage, subject);

                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(string.Format("Failure to send email to {0}. Error {1}.", emailAddress, e.Message));
                return false;
            }
        }

        public static bool NotificarCitasMedicos(Uceme.Model.Models.Cita cita, string observaciones)
        {
            var emailAddress = System.Configuration.ConfigurationManager.AppSettings["credential_user"];
            try
            {
                var nombre = cita.nombre;
                var dia = Utilidades.DiasHoras.EuropeanDay(cita.dia);
                var hora = Utilidades.DiasHoras.TimeToString(cita.hora);
                var hospital = cita.Turno.DatosProfesionales.nombre;
                var telefono = cita.telefono;

                var emailMessage = new StringBuilder();

                emailMessage.Append("<br />");
                emailMessage.Append("Hay una nueva cita de UCEME: ");
                emailMessage.Append("<br />");
                emailMessage.Append("El paciente " + nombre + " tiene una cita el dia " + dia + " a las " + hora + " en el hospital " + hospital);
                emailMessage.Append("<br />");
                emailMessage.Append("Su telefono es : " + telefono);
                emailMessage.Append("<br />");
                if (cita.email != null)
                {
                    emailMessage.Append("y su email: " + cita.email);
                }
                else
                {
                    emailMessage.Append("no dejo email de contacto");
                }

                if (observaciones != "")
                {
                    emailMessage.Append("<br />");
                    emailMessage.Append("Adjunto las siguientes observaciones : " + observaciones);
                }

                var subject = "Nueva cita UCEME";
                SendEmail(emailAddress, emailMessage, subject);

                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(string.Format("Failure to send email to {0}. Error {1}.", emailAddress, e.Message));
                return false;
            }
        }

        public static bool NotificarCitasUsuario(Uceme.Model.Models.Cita cita, string observaciones)
        {
            var emailAddress = cita.email;
            try
            {
                var nombre = cita.nombre;
                var dia = Utilidades.DiasHoras.EuropeanDay(cita.dia);
                var hora = Utilidades.DiasHoras.TimeToString(cita.hora);
                var hospital = cita.Turno.DatosProfesionales.nombre;
                var telefono = cita.telefono;

                var emailMessage = new StringBuilder();

                emailMessage.Append("<br />");
                emailMessage.Append("Tiene una nueva cita de UCEME: ");
                emailMessage.Append("<br />");
                emailMessage.Append("El paciente " + nombre + " tiene una cita el dia " + dia + " a las " + hora + " en el hospital " + hospital);
                emailMessage.Append("<br />");
                emailMessage.Append("Su telefono es : " + telefono);
                emailMessage.Append("<br />");
                emailMessage.Append("y su email: " + emailAddress);
                if (observaciones != "")
                {
                    emailMessage.Append("<br />");
                    emailMessage.Append("Adjunto las siguientes observaciones : " + observaciones);
                }

                emailMessage.Append("<br />");
                emailMessage.Append("Por favor, no responda a este mensaje.");
                emailMessage.Append("Muchas gracias.");

                var subject = "Nueva cita UCEME";
                SendEmail(emailAddress, emailMessage, subject);

                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(string.Format("Failure to send email to {0}. Error {1}.", emailAddress, e.Message));
                return false;
            }
        }

        private static void SendEmail(string emailto, StringBuilder emailMessage, string subject)
        {
            var email = new MailMessage();
            var emailfrom = System.Configuration.ConfigurationManager.AppSettings["email_from"];
            email.From = new MailAddress(emailfrom);
            email.To.Add(new MailAddress(emailto));
            email.Subject = subject;
            email.Body = emailMessage.ToString();
            email.IsBodyHtml = true;

            using (var smtpServer = new SmtpClient())
            {
                var host = System.Configuration.ConfigurationManager.AppSettings["host_SMTP"];
                smtpServer.Host = host;

                var port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["port_SMTP"]);
                smtpServer.Port = port;

                //hasta configurar la cuanta de envio, supongo que con las credenciales de esta, será valida
                var credUser = System.Configuration.ConfigurationManager.AppSettings["credential_user"];
                var credPassw = System.Configuration.ConfigurationManager.AppSettings["credential_password"];
                smtpServer.Credentials = new NetworkCredential(credUser, credPassw);
                smtpServer.EnableSsl = true;
                smtpServer.Send(email);
            }
        }
    }
}