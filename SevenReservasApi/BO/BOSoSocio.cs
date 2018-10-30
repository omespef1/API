using SevenReservas.DAO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenReservas.BO
{
    public class BOSoSocio
    {
        DAOSoSocio daoSoSocio = new DAOSoSocio();
       

        public TOSoSocio GetSoSocio(string sbe_ncar, int sbe_cont,short emp_codi)
        {
            TOSoSocio to = new TOSoSocio();

            var socios = daoSoSocio.GetSoSbene(emp_codi, sbe_ncar);

            if (sbe_cont == 1 && socios.Count > 1)
            {
                var socio = socios.First(x => x.Sbe_cont == 1);
                to = socio;
                //to.Soc_nomb = socio.Soc_nomb;
                //to.Soc_apel = socio.Soc_apel;
                //to.Soc_tele = socio.Soc_tele;
                //to.Sbe_fexp = socio.Sbe_fexp;
                //to.Mac_nume = socio.Mac_nume;
                //to.Soc_foto = socio.Soc_foto;
                //to.Sbe_ncel = socio.Sbe_ncel;
                //to.Sbe_dire = socio.Sbe_dire;
                //to.Sbe_mail = socio.Sbe_mail;

                to.beneficiarios.AddRange(socios.Where(x => x.Sbe_cont != 1));
            }
            else
            {
                var socio = socios.First(x => x.Sbe_cont == sbe_cont);
                to = socio;
                //to.Soc_nomb = socio.Soc_nomb;
                //to.Soc_apel = socio.Soc_apel;
                //to.Soc_tele = socio.Soc_tele;
                //to.Sbe_fexp = socio.Sbe_fexp;
                //to.Mac_nume = socio.Mac_nume;
                //to.Soc_foto = socio.Soc_foto;
                //to.Sbe_ncel = socio.Sbe_ncel;
                //to.Sbe_dire = socio.Sbe_dire;
                //to.Sbe_mail = socio.Sbe_mail;
            }

            to.Soc_pass = "";

            return to;
        }

        public TOSoSocio GetSoSocio(string Sbe_ncar, string soc_pass,short emp_codi)
        {
            var socio = daoSoSocio.GetSoSocio(emp_codi, Sbe_ncar,soc_pass);            
            return socio;
        }

        public TOTransaction< TOSoRsoci> GetSoRsoci(TOSoRsoci toSoRsoci)
        {
            try
            {                
                var retorno = daoSoSocio.GetSoRsoci(toSoRsoci);
                if (retorno == null)                                   
                    return new TOTransaction<TOSoRsoci>() { ObjTransaction = null, Retorno = 1, TxtError = "Datos incorrectos. Verifique por favor" };
                int r = 0;
                if (string.IsNullOrEmpty(toSoRsoci.Sbe_pass))
                {
                    Random generator = new Random();
                    r = generator.Next(0, 1000000);
                    toSoRsoci.Soc_cing = r;
                    toSoRsoci.Soc_cfec = DateTime.Now.AddHours(2);
                }
                else
                {
                    if ( DateTime.Now> toSoRsoci.Soc_cfec)
                        throw new Exception("El código de seguridad ha expirado.");
                }
                //Si el socio ya esta registrado por el app 
                //genera el mensaje que ya esta registrado 
                //por el app             
                toSoRsoci.Soc_cont = retorno.Soc_cont;
                toSoRsoci.Sbe_cont = retorno.Sbe_cont;
                var updsoci = daoSoSocio.UpdSoRsoci(toSoRsoci);
                if (updsoci != 0 && r != 0)
                {
                    //Ophelia.Email e = new Ophelia.Email();
                    //Ophelia.TOEmail te = new Ophelia.TOEmail();
                    //
                    //te.ema_asun = "Codigo de registro";
                    //te.ema_dest = toSoRsoci.Sbe_mail;
                    //te.ema_nomb = "luis";
                    //te.ema_remi = "luisv@digialware.com.co";
                    //te.ema_assl = true;
                    //
                    //te.ema_desc = "Su código de registro es: " + r.ToString();
                    //te.ema_iden = "luisv@digialware.com.co";
                    //
                    //e.EnviarEmail(te);

                    EnviarCorreo c = new EnviarCorreo();
                    c.SendMail(toSoRsoci.Soc_cing.ToString(), toSoRsoci.Sbe_mail);
                }
                //envia correo electronico
                return new TOTransaction<TOSoRsoci>() { ObjTransaction = toSoRsoci, TxtError = "", Retorno = 0 };
            }
            catch (Exception ex)
            {
                return new TOTransaction<TOSoRsoci>() { ObjTransaction = null, TxtError = ex.Message, Retorno = 1 };
            }
           
        }

        public int GetSoRsoci(int soc_cont, int sbe_cont, int soc_cing,short emp_codi)
        {
            int retorno = 1;

            TOSoRsoci toSoRsoci = new TOSoRsoci();
            toSoRsoci.Emp_codi = emp_codi;
            toSoRsoci.Soc_cont = soc_cont;
            toSoRsoci.Sbe_cont = sbe_cont;
            toSoRsoci.Soc_cing = soc_cing;

            var rsoci = daoSoSocio.GetSoRsociValido(toSoRsoci);

            if (rsoci != null)
            {
                retorno = 0;

                if (rsoci.Soc_cont == 0)
                    return retorno = 1;

                DateTime ahora = DateTime.Now;
                if (rsoci.Soc_cfec < ahora)
                    return retorno = 1;
            }
           

            return retorno;
        }

        public int UpdSoSocip(TOSoSocio toSoSocio)
        {
            int retorno = 0;
           
            toSoSocio.Soc_pass = Ophelia.Seven.Encrypta.EncriptarClave(toSoSocio.Soc_pass);
            
            if (toSoSocio.Soc_cont == 0)
                retorno = 1;

            var updpass = daoSoSocio.UpdSoSocip(toSoSocio);

            if (updpass > 0)
                retorno = 0;


            return retorno;
        }

        public int UpdSoSocio(TOSoSocio toSoSocio)
        {
          

            var updsocio = daoSoSocio.UpdSoSocio(toSoSocio);

            return updsocio;
        }


        public int UpdSoSocio(short emp_codi, string mac_nume)
        {
            return 0;
        }
    }
}
