using Ophelia;
using Ophelia.Comun;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SevenReservas.BO
{
    public class EnviarCorreo
    {
        //protected OException BOException;
        private XML HParamXML;
        
        ///<summary>
        /// Constructor del Business Object del programa
        ///</summary>
        //public Email()
        //{
        //    BOException = new OException();
        // 
        //}


        public void SendMail(string codigo, string mail)
        {
            XML HParamXML = new XML();

            using (SmtpClient smtp = new SmtpClient())
            {
                MailMessage correo = new MailMessage();

                string user = HParamXML.ObtenerWebUser();
                string pass = HParamXML.ObtenerWebPassword();

                correo.From = new MailAddress(user, user, Encoding.UTF8);

                correo.To.Add(mail);

                correo.Subject = "Código registro ";
                correo.Body = "Su código de registro es: " + codigo;
                correo.IsBodyHtml = true;
                correo.BodyEncoding = Encoding.UTF8;
                correo.Priority = MailPriority.High;

                smtp.UseDefaultCredentials = false;

                smtp.Host = HParamXML.ObtenerWebMail();
                smtp.Port = Convert.ToInt32(HParamXML.ObtenerPuertoMail());



                //smtp.Credentials = new System.Net.NetworkCredential(user, pass);
                smtp.EnableSsl = true;

                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587;
                //smtp.Credentials = new System.Net.NetworkCredential("luisv@digitalware.com.co", "Vixxxx*");
                var ssl = ConfigurationManager.AppSettings["CorreoSsl"].ToString();
                if (ssl != null)
                    smtp.EnableSsl = bool.Parse(ssl);

                smtp.Credentials = new System.Net.NetworkCredential(user, pass);
                //smtp.EnableSsl = true;


                smtp.Send(correo);
            }
        }

    }
}
