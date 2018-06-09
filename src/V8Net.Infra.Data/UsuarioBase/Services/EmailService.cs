using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using V8Net.Domain.UsuarioBaseContext.Services;

namespace V8Net.Infra.Data.UsuarioBase.Services
{
    public class EmailService : IEmailService
    {
        public void RecuperarSenha(string email, string senha)
        {
            var objEmail = new MailMessage { From = new MailAddress("<< EMAIL >>") };
            objEmail.To.Add(email);
            objEmail.Priority = MailPriority.High;
            objEmail.IsBodyHtml = true;
            objEmail.Subject = "V8NET Ltda (Senha do sistema)";
            objEmail.Body = $"Você solicitou uma nova senha. \n" +
                            $"Senha: { senha }. \n" +
                            $"Lembramos que esta senha é válida por 24 hrs e, caso você não a altere, deverá de solicitar uma nova.";
            objEmail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
            objEmail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
            using (var objSmtp = new SmtpClient())
            {
                objSmtp.Host = "<< smtp.HOST.com.br >>";
                objSmtp.Port = 587;
                objSmtp.Credentials = new NetworkCredential("<< EMAIL >>", "<< SENHA >>");
                try
                {
                    objSmtp.Send(objEmail);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

    }
}
