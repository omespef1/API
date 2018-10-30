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
    public class DAOProducto
    {
        OException BOException = new OException();

        public List<TOProducto> GetProductos(short emp_codi, int Cla_cont)
        {
       
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();
                
                sql.Append(" SELECT IN_PRODU.PRO_CONT, IN_PRODU.PRO_CODI,                                                   ");
                sql.Append(" IN_PRODU.PRO_NOMB,CAST(IN_BMPRO.BMP_PROD AS VARBINARY (MAX)) BMP_PROD,IN_PRODU.PRO_DMIN,AE_ESPAC.ESP_MDIT             ");
                sql.Append(" FROM AE_ESPAC,AE_DESPA,AE_CLASE,IN_PRODU                                                       ");
                sql.Append(" LEFT OUTER JOIN IN_BMPRO ON(IN_PRODU.EMP_CODI = IN_BMPRO.EMP_CODI                              ");
                sql.Append(" AND IN_PRODU.PRO_CONT = IN_BMPRO.PRO_CONT)                                                     ");
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_CLASE.EMP_CODI                                                    ");
                sql.Append(" AND AE_ESPAC.CLA_CONT = AE_CLASE.CLA_CONT                                                      ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI                                                      ");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT                                                      ");
                sql.Append(" AND AE_DESPA.EMP_CODI = IN_PRODU.EMP_CODI                                                      ");
                sql.Append(" AND AE_DESPA.PRO_CONT = IN_PRODU.PRO_CONT                                                      ");
                sql.Append(" AND AE_ESPAC.ESP_ESTA = 'A'                                                                    ");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT                                                            ");
                sql.Append(" AND IN_PRODU.PRO_DMIN > 0                                                                      ");
                sql.Append(" AND AE_DESPA.EMP_CODI = @P_EMP_CODI                                                            ");

                sql.Append(" AND (AE_DESPA.DES_VISU = 'P' OR AE_DESPA.DES_VISU = 'A')                                       ");
                sql.Append(" AND AE_CLASE.CLA_MAPP = 'S' ");

                sql.Append(" GROUP BY IN_PRODU.PRO_CONT, IN_PRODU.PRO_CODI,                                                 ");
                sql.Append(" IN_PRODU.PRO_NOMB,CAST(IN_BMPRO.BMP_PROD AS VARBINARY (MAX)),IN_PRODU.PRO_DMIN,AE_ESPAC.ESP_MDIT                       ");
                sql.Append(" ORDER BY IN_PRODU.PRO_CONT ");
                //sql.Append(" SELECT IN_PRODU.PRO_CONT, IN_PRODU.PRO_CODI,");
                //sql.Append(" IN_PRODU.PRO_NOMB,IN_BMPRO.BMP_PROD,IN_PRODU.PRO_DMIN FROM PS_DLIPR,PS_LIPRE,AE_CLASE,IN_PRODU");
                //sql.Append(" LEFT OUTER JOIN IN_BMPRO ON(IN_PRODU.EMP_CODI = IN_BMPRO.EMP_CODI");
                //sql.Append(" AND IN_PRODU.PRO_CONT = IN_BMPRO.PRO_CONT)");
                //sql.Append(" WHERE PS_LIPRE.EMP_CODI = AE_CLASE.EMP_CODI");
                //sql.Append(" AND PS_LIPRE.LIP_CONT = AE_CLASE.LIP_CONT");
                //sql.Append(" AND PS_DLIPR.EMP_CODI = PS_LIPRE.EMP_CODI");
                //sql.Append(" AND PS_DLIPR.LIP_CONT = PS_LIPRE.LIP_CONT");
                //sql.Append(" AND PS_DLIPR.EMP_CODI = IN_PRODU.EMP_CODI");
                //sql.Append(" AND PS_DLIPR.PRO_CONT = IN_PRODU.PRO_CONT");
                //sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT");
                //sql.Append(" AND IN_PRODU.PRO_DMIN > 0");
                //sql.Append(" AND PS_DLIPR.EMP_CODI = @P_EMP_CODI");



                listAux.Add(new Parameter("@P_CLA_CONT", Cla_cont));
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

        internal TOProducto GetProducto(short emp_codi, int pro_cont)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT PRO_CONT, PRO_CODI, PRO_NOMB,PRO_DMIN FROM  IN_PRODU");
                sql.Append(" WHERE IN_PRODU.PRO_CONT = @P_PRO_CONT");
                sql.Append(" AND IN_PRODU.EMP_CODI = @P_EMP_CODI");

                listAux.Add(new Parameter("@P_PRO_CONT", pro_cont));
                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));

                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.Read(pTOContext, sql.ToString(), MakeProducto, oParameter);
                return objeto;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }
        public Func<IDataReader, TOProducto> MakeProducto = reader => new TOProducto
        {
            Pro_codi = reader["PRO_CODI"].AsString(),
            Pro_cont = reader["PRO_CONT"].AsInt(),
            Pro_nomb = reader["PRO_NOMB"].AsString(),
            Pro_dmin = reader["PRO_DMIN"].AsInt()




        };

        public Func<IDataReader, TOProducto> Make = reader => new TOProducto
        {
            Pro_codi = reader["PRO_CODI"].AsString(),
            Pro_cont = reader["PRO_CONT"].AsInt(),
            Pro_nomb = reader["PRO_NOMB"].AsString(),
            Bmp_prod = reader["BMP_PROD"].AsFoto(),
            Pro_dmin = reader["PRO_DMIN"].AsInt(),
            esp_mdit = reader["ESP_MDIT"].AsString(),
        //    esp_cont = reader["ESP_CONT"].AsInt(),
        //    esp_nomb = reader["ESP_NOMB"].AsString(),
        };
    }
}