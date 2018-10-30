using SevenReservas.Models;
using Ophelia;
using Ophelia.Comun;
using Ophelia.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace SevenReservas.DAO
{
    public class DAOPqDinpq
    {
        OException BOException = new OException();

        public List<TOPqDinpq> GetPqDinpq(short emp_codi, int inp_cont)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT DIN_CONT, DIN_SEGT, DIN_FESE ");
                sql.Append(" FROM PQ_DINPQ ");
                sql.Append("  WHERE EMP_CODI = @P_EMP_CODI ");
                sql.Append("  AND INP_CONT = @P_INP_CONT ");
                sql.Append("  ORDER BY DIN_CONT DESC ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_INP_CONT", inp_cont));


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

        public List<TOPqDinpq> GetPqDinpqDetail(short emp_codi, int inp_cont, int din_cont)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT DIN_CONT, DIN_SEGT, DIN_FESE ");
                sql.Append(" FROM PQ_DINPQ ");
                sql.Append("  WHERE EMP_CODI = @P_EMP_CODI ");
                sql.Append("  AND INP_CONT = @P_INP_CONT ");
                sql.Append("  AND DIN_CONT = @P_DIN_CONT ");
                sql.Append("  ORDER BY DIN_CONT DESC ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_INP_CONT", inp_cont));
                listAux.Add(new Parameter("@P_DIN_CONT", din_cont));

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


        public Func<IDataReader, TOPqDinpq> Make = reader => new TOPqDinpq
        {
            Din_cont = reader["DIN_CONT"].AsInt(),
            Din_segt = reader["DIN_SEGT"].AsString(),
            Din_fese = reader["DIN_FESE"].AsDateTime(),
        };
    }
}