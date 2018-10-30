using SevenReservas.Models;
using Ophelia;
using Ophelia.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ophelia.Comun;
using SevenReservas.Tools;
using Ophelia.Seven;

namespace SevenReservas.DAO
{
    public class DAOSoSocio
    {

        OException BOException = new OException();

        public TOSoSocio GetSoSocio(short emp_codi, string sbe_ncar,string soc_pass)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT SO_SBENE.SBE_NOMB, SO_SBENE.SBE_APEL, SO_SBENE.SBE_TELE, ");
                sql.Append(" SO_SBENE.SBE_PASS, SO_SBENE.SBE_FEXP, SO_SBENE.SBE_PASS, SO_SBENE.SBE_CONT, ");
                sql.Append(" SO_SBENE.SOC_FOTO, SO_SBENE.SBE_NCAR, SO_SBENE.SOC_CONT , SO_SOCIO.MAC_NUME, SO_SBENE.SBE_CODI, ");

                sql.Append(" SO_SBENE.SBE_MAIL, SO_SBENE.SBE_NCEL, SO_SBENE.SBE_DIRE,SO_SOCIO.SOC_NCAR, GN_EMPRE.EMP_TELE, GN_EMPRE.EMP_NITE ");

                sql.Append(" FROM SO_SOCIO, SO_SBENE, GN_EMPRE ");
                sql.Append(" WHERE SO_SOCIO.EMP_CODI = SO_SBENE.EMP_CODI ");
                sql.Append(" AND SO_SOCIO.SOC_CONT = SO_SBENE.SOC_CONT ");
                sql.Append(" AND SO_SOCIO.EMP_CODI = GN_EMPRE.EMP_CODI ");
                sql.Append(" AND SO_SOCIO.EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND SO_SBENE.SBE_NCAR = @P_SBE_NCAR ");
                sql.Append(" AND SO_SOCIO.SOC_ESTA = 'A' ");
                sql.Append(" AND SO_SBENE.SBE_ESTA = 'A' ");
                sql.Append(" AND SO_SBENE.SBE_PASS = @SBE_PASS ");
                //sql.Append(" AND SO_SBENE.SBE_CONT = 1 ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_SBE_NCAR", sbe_ncar));
                listAux.Add(new Parameter("@SBE_PASS", soc_pass));

                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.Read(pTOContext, sql.ToString(), Make, oParameter);
                return objeto;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }


        public List<TOSoSocio> GetSoSbene(short emp_codi, string sbe_ncar)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT BENEFICI.SBE_NOCO, BENEFICI.SBE_NCAR, BENEFICI.SBE_CONT, BENEFICI.SBE_NCAR, ");
                sql.Append(" BENEFICI.SBE_NOMB, BENEFICI.SBE_APEL, BENEFICI.SBE_TELE, '' SBE_PASS, BENEFICI.SOC_FOTO, ");
                sql.Append(" BENEFICI.SBE_FEXP, BENEFICI.SBE_NCAR, SO_SBENE.SBE_CONT, SO_SBENE.SOC_CONT, SO_SOCIO.MAC_NUME, ");
                sql.Append(" BENEFICI.SBE_MAIL, BENEFICI.SBE_NCEL, BENEFICI.SBE_DIRE, SO_SOCIO.SOC_NCAR, GN_EMPRE.EMP_TELE, GN_EMPRE.EMP_NITE, SO_SBENE.SBE_CODI ");
                sql.Append(" FROM SO_SOCIO, SO_SBENE, SO_SBENE BENEFICI, GN_EMPRE ");
                sql.Append(" WHERE SO_SOCIO.EMP_CODI = SO_SBENE.EMP_CODI ");
                sql.Append(" AND SO_SOCIO.SOC_CONT = SO_SBENE.SOC_CONT ");
                sql.Append(" AND SO_SOCIO.EMP_CODI = GN_EMPRE.EMP_CODI ");
                sql.Append(" AND SO_SOCIO.EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND SO_SBENE.SBE_NCAR = @P_SBE_NCAR ");
                sql.Append(" AND SO_SOCIO.SOC_ESTA = 'A' ");
                sql.Append(" AND SO_SBENE.SBE_ESTA = 'A' ");

                sql.Append(" AND SO_SBENE.EMP_CODI = BENEFICI.EMP_CODI ");
                sql.Append(" AND SO_SBENE.SOC_CONT = BENEFICI.SOC_CONT ");

                //sql.Append(" AND SO_SBENE.SBE_CONT = 1 ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_SBE_NCAR", sbe_ncar));

                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.ReadList(pTOContext, sql.ToString(), Make, oParameter);
                return objeto;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }
        public Func<IDataReader, TOSoSocio> Make = reader => new TOSoSocio
        {
            Soc_nomb = reader["SBE_NOMB"].AsString(),
            Soc_apel = reader["SBE_APEL"].AsString(),
            Soc_tele = reader["SBE_TELE"].AsString(),
            Soc_pass = reader["SBE_PASS"].AsString(),
            Soc_foto = reader["SOC_FOTO"].AsFoto(),
            Sbe_fexp = reader["SBE_FEXP"].AsDateTime(),
            Mac_nume = reader["SBE_NCAR"].AsString(),
            Sbe_cont = reader["SBE_CONT"].AsInt(),
            Soc_cont = reader["SOC_CONT"].AsInt(),
            Mac_nume1 = reader["MAC_NUME"].AsString(),
            Soc_ncar = reader["SOC_NCAR"].AsString(),
            Sbe_mail = reader["SBE_MAIL"].AsString(),
            Sbe_ncel = reader["SBE_NCEL"].AsString(),
            Sbe_dire = reader["SBE_DIRE"].AsString(),
            Emp_tele = reader["EMP_TELE"].AsString(),
            Emp_nite = reader["EMP_NITE"].AsString(),
            Sbe_codi = reader["SBE_CODI"].AsString()
        };


        public TOSoRsoci GetSoRsoci(TOSoRsoci toSoRsoci)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT SO_SOCIO.SOC_CONT, SO_SBENE.SBE_CONT, SO_SBENE.SBE_CFEC, SO_SBENE.SBE_PASS ");
                sql.Append(" FROM SO_SOCIO, SO_SBENE ");
                sql.Append(" WHERE SO_SOCIO.EMP_CODI = SO_SBENE.EMP_CODI ");
                sql.Append(" AND SO_SOCIO.SOC_CONT = SO_SBENE.SOC_CONT ");
                sql.Append(" AND SO_SOCIO.EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND SO_SOCIO.SOC_ESTA = 'A' ");
                sql.Append(" AND SO_SBENE.SBE_ESTA = 'A' ");
                //sql.Append(" AND SO_SBENE.SBE_CONT = 1 ");

                sql.Append(" AND SO_SBENE.SBE_NCAR = @P_SBE_NCAR ");
                sql.Append(" AND SO_SBENE.SBE_MAIL = @P_SBE_MAIL ");
                sql.Append(" AND SO_SBENE.SBE_NCEL = @P_SBE_NCEL ");


                listAux.Add(new Parameter("@P_EMP_CODI", toSoRsoci.Emp_codi));
                listAux.Add(new Parameter("@P_SBE_NCAR", toSoRsoci.Sbe_ncar));
                listAux.Add(new Parameter("@P_SBE_MAIL", toSoRsoci.Sbe_mail));
                listAux.Add(new Parameter("@P_SBE_NCEL", toSoRsoci.Sbe_ncel));

                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.Read(pTOContext, sql.ToString(), MakeRsoci, oParameter);
                return objeto;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public Func<IDataReader, TOSoRsoci> MakeRsoci = reader => new TOSoRsoci
        {
            // Emp_codi { get; set; }
            //  Mac_nume { get; set; }
            //  Sbe_mail { get; set; }
            //  Sbe_ncel { get; set; }
            Soc_cont = reader["SOC_CONT"].AsInt(),
            Sbe_cont = reader["SBE_CONT"].AsInt(),
            //Soc_cing { get; set; }
            Soc_cfec = reader["SBE_CFEC"].AsDateTime(),
            Sbe_pass = reader["SBE_PASS"].AsString(),
        };

        public TOSoRsoci GetSoRsociValido(TOSoRsoci toSoRsoci)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();
                
                sql.Append(" SELECT SO_SBENE.SOC_CONT, SO_SBENE.SBE_CONT, SO_SBENE.SBE_CFEC ");
                sql.Append(" FROM SO_SOCIO, SO_SBENE ");
                sql.Append(" WHERE SO_SOCIO.EMP_CODI = SO_SBENE.EMP_CODI ");
                sql.Append(" AND SO_SOCIO.SOC_CONT = SO_SBENE.SOC_CONT ");
                sql.Append(" AND SO_SOCIO.EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND SO_SOCIO.SOC_ESTA = 'A' ");
                sql.Append(" AND SO_SBENE.SBE_ESTA = 'A' ");

                sql.Append(" AND SO_SBENE.SOC_CONT = @P_SOC_CONT ");
                sql.Append(" AND SO_SBENE.SBE_CONT = @P_SBE_CONT ");
                sql.Append(" AND SO_SBENE.SBE_CING = @P_SBE_CING ");


                listAux.Add(new Parameter("@P_EMP_CODI", toSoRsoci.Emp_codi));
                listAux.Add(new Parameter("@P_SOC_CONT", toSoRsoci.Soc_cont));
                listAux.Add(new Parameter("@P_SBE_CONT", toSoRsoci.Sbe_cont));
                listAux.Add(new Parameter("@P_SBE_CING", toSoRsoci.Soc_cing));

                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.Read(pTOContext, sql.ToString(), MakeRsoci, oParameter);
                return objeto;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }


        public int UpdSoRsoci(TOSoRsoci toSoRsoci)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" UPDATE SO_SBENE ");
                sql.Append(" SET  ");
                //Actualiza el código de verificación y la fecha máxima de uso del código si el usuario está intentando actualizar los datos
                if (string.IsNullOrEmpty(toSoRsoci.Sbe_pass))
                {
                    sql.Append(" SBE_CING = @P_SBE_CING, ");
                    sql.Append(" SBE_CFEC = @P_SBE_CFEC ");
                    listAux.Add(new Parameter("@P_SBE_CING", toSoRsoci.Soc_cing));
                    listAux.Add(new Parameter("@P_SBE_CFEC", toSoRsoci.Soc_cfec));
                }
                //Actualiza el password si el usuario ya recibió el codigo de seguridad y confirmó su constraseña
                if (!string.IsNullOrEmpty(toSoRsoci.Sbe_pass))
                {
                    sql.Append(" SBE_PASS = @P_SBE_PASS ");
                    listAux.Add(new Parameter("@P_SBE_PASS", Encrypta.EncriptarClave(toSoRsoci.Sbe_pass)));
                }
               
                sql.Append(" WHERE EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND SBE_NCAR = @P_SBE_NCAR ");
                sql.Append(" AND SOC_CONT = @P_SOC_CONT ");
                sql.Append(" AND SBE_CONT = @P_SBE_CONT ");

             
                listAux.Add(new Parameter("@P_EMP_CODI", toSoRsoci.Emp_codi));
                listAux.Add(new Parameter("@P_SBE_NCAR", toSoRsoci.Sbe_ncar));
                listAux.Add(new Parameter("@P_SOC_CONT", toSoRsoci.Soc_cont));
                listAux.Add(new Parameter("@P_SBE_CONT", toSoRsoci.Sbe_cont));
              

                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.Update(pTOContext, sql.ToString(), oParameter);
                return objeto.AsInt();
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return -1;
            }
        }


        public int UpdSoSocip(TOSoSocio toSoSocio)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" UPDATE SO_SBENE ");
                sql.Append(" SET SBE_PASS = @P_SBE_PASS ");
                sql.Append(" WHERE EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND SOC_CONT = @P_SOC_CONT ");
                sql.Append(" AND SBE_CONT = @P_SBE_CONT ");

                listAux.Add(new Parameter("@P_SBE_PASS", toSoSocio.Soc_pass));
                listAux.Add(new Parameter("@P_EMP_CODI", toSoSocio.Emp_codi));
                listAux.Add(new Parameter("@P_SOC_CONT", toSoSocio.Soc_cont));
                listAux.Add(new Parameter("@P_SBE_CONT", toSoSocio.Sbe_cont));


                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.Update(pTOContext, sql.ToString(), oParameter);
                return objeto.AsInt();
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return -1;
            }
        }



        public int UpdSoSocio(TOSoSocio toSoSocio)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" UPDATE SO_SBENE SET ");
                sql.Append(" SBE_NCEL = @P_SBE_NCEL, ");
                sql.Append(" SBE_DIRE = @P_SBE_DIRE, ");
                sql.Append(" SBE_TELE = @P_SBE_TELE, ");
                sql.Append(" SBE_MAIL = @P_SBE_MAIL ");

                sql.Append(" WHERE EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND SOC_CONT = @P_SOC_CONT ");
                sql.Append(" AND SBE_CONT = @P_SBE_CONT ");

                listAux.Add(new Parameter("@P_SBE_NCEL", toSoSocio.Sbe_ncel));
                listAux.Add(new Parameter("@P_SBE_DIRE", toSoSocio.Sbe_dire));
                listAux.Add(new Parameter("@P_SBE_TELE", toSoSocio.Soc_tele));
                listAux.Add(new Parameter("@P_SBE_MAIL", toSoSocio.Sbe_mail));
                listAux.Add(new Parameter("@P_EMP_CODI", toSoSocio.Emp_codi));
                listAux.Add(new Parameter("@P_SOC_CONT", toSoSocio.Soc_cont));
                listAux.Add(new Parameter("@P_SBE_CONT", toSoSocio.Sbe_cont));


                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.Update(pTOContext, sql.ToString(), oParameter);
                return objeto.AsInt();
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return -1;
            }
        }





    }
}
