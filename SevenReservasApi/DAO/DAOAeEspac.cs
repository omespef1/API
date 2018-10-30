using SevenReservas.Models;
using SevenReservas.Tools;
using Ophelia;
using Ophelia.Comun;
using Ophelia.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SevenReservas.DAO
{
    public class DAOAeClase
    {
        OException BOException = new OException();

        public List<TOAeClase> GetAeClase(short emp_codi)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT EMP_CODI,CLA_CONT,CLA_CODI,CLA_NOMB,CLA_CLTI,AUD_ESTA,AUD_UFAC,CLA_TICA,");
                sql.Append(" AUD_USUA, LIP_CONT, LIP_CONB, CLA_FOTO, CLA_FCHR, CLA_DESC ");
                sql.Append(" FROM AE_CLASE");
                sql.Append(" WHERE EMP_CODI = @P_EMP_CODI");
                sql.Append(" AND AE_CLASE.CLA_CODI > '0' ");
                sql.Append(" AND AE_CLASE.CLA_MAPP = 'S'");
                sql.Append(" AND EXISTS(SELECT 1 FROM AE_ESPAC ");
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_CLASE.EMP_CODI ");
                sql.Append(" AND AE_ESPAC.CLA_CONT = AE_CLASE.CLA_CONT ");
                sql.Append(" AND AE_ESPAC.ESP_ESTA = 'A') ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));

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

        internal TOAeClase GetAeClaseByClaCont(int emp_codi, int cla_cont)
        {
            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT EMP_CODI,CLA_CONT,CLA_CODI,CLA_NOMB,CLA_CLTI,AUD_ESTA,AUD_UFAC,CLA_TICA,CLA_DESC, ");
                sql.Append(" AUD_USUA, LIP_CONT, LIP_CONB, CLA_FOTO, CLA_FCHR ");
                sql.Append(" FROM AE_CLASE");
                sql.Append(" WHERE EMP_CODI = @P_EMP_CODI");
                sql.Append(" AND CLA_CONT = @P_CLA_CONT");

                Parameter[] listAux = {
                    new Parameter("@P_EMP_CODI", emp_codi),
                    new Parameter("@P_CLA_CONT", cla_cont),
                };
                OTOContext pTOContext = new OTOContext();
                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.Read(pTOContext, sql.ToString(), Make, listAux);
                return objeto;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public Func<IDataReader, TOAeClase> Make = reader => new TOAeClase
        {
            Cla_clti = reader["CLA_CLTI"].AsString(),
            Cla_codi = reader["CLA_CODI"].AsString(),
            Cla_cont = reader["CLA_CONT"].AsInt(),
            Cla_nomb = reader["CLA_NOMB"].AsString(),
            Emp_codi = reader["EMP_CODI"].AsInt16(),
            Lip_cont = reader["LIP_CONT"].AsInt(),
            Cla_foto = reader["CLA_FOTO"].AsFoto(),
            Cla_Fchr = reader["CLA_FCHR"] == DBNull.Value ? (DateTime?)null : reader["CLA_FCHR"].AsDateTime(),
            Cla_Tica = reader["CLA_TICA"] == DBNull.Value ? (int?)null : reader["CLA_TICA"].AsInt(),
            Cla_desc = reader["CLA_DESC"].ToString()
        };
    }
}