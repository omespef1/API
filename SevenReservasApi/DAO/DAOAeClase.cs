using SevenReservas.Models;
using Ophelia;
using Ophelia.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Ophelia.Comun;
using SevenReservas.Tools;

namespace SevenReservas.DAO
{
    public class DAOAeEspac
    {
        OException BOException = new OException();

        public List<TOAeEspac> GetAeEspac(short emp_codi,int cla_cont)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT EMP_CODI, ESP_CONT, ESP_CODI, ESP_NOMB, CLA_CONT, TIP_CONT, ESP_DESC, BOD_CODI, ESP_ESTA, AUD_ESTA,");
                sql.Append(" AUD_UFAC, AUD_USUA, ESP_AREA, ESP_CAPA, ESP_PADR, ESP_IMAG, BOD_CONT, CAS_CONT, ESP_MDIT,ESP_IMAG FROM AE_ESPAC");
                sql.Append(" WHERE EMP_CODI = @P_EMP_CODI");
                sql.Append(" AND AE_ESPAC.CLA_CONT = @P_CLA_CONT");
                sql.Append(" AND AE_ESPAC.ESP_CONT > 0");
                sql.Append(" AND AE_ESPAC.ESP_ESTA = 'A' ");
                sql.Append(" AND AE_ESPAC.ESP_MAPP = 'S' ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_CLA_CONT", cla_cont));

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

        public Func<IDataReader, TOAeEspac> Make = reader => new TOAeEspac
        {
            Emp_codi = reader["EMP_CODI"].AsInt16(),
            Cla_cont = reader["CLA_CONT"].AsInt(),
            Esp_codi = reader["ESP_CODI"].AsString(),
            Esp_cont = reader["ESP_CONT"].AsInt(),
            Esp_desc = reader["ESP_DESC"].AsString(),
            Esp_nomb = reader["ESP_NOMB"].AsString(),
            Tip_cont = reader["TIP_CONT"].AsInt(),
            Esp_imag = reader["ESP_IMAG"].AsFoto(),
            Esp_mdit = reader["ESP_MDIT"].AsString()
            
        };
    }
}