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
    public class DAOPqInpqr
    {
        OException BOException = new OException();

        public List<TOPqInpqr> GetPqInpqr(short emp_codi, int soc_cont, int sbe_cont)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT INP_CONT, INP_ESTA, INP_MPQR, INP_FEVE ");
                sql.Append(" FROM PQ_INPQR ");
                sql.Append(" WHERE EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND SOC_CONT = @P_SOC_CONT ");
                sql.Append(" AND SBE_CONT = @P_SBE_CONT ");
                sql.Append(" AND SBE_CONT = @P_SBE_CONT ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_SOC_CONT", soc_cont));
                listAux.Add(new Parameter("@P_SBE_CONT", sbe_cont));

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

        public Func<IDataReader, TOPqInpqr> Make = reader => new TOPqInpqr
        {
            inp_cont = reader["INP_CONT"].AsInt(),
            inp_esta = reader["INP_ESTA"].AsString(),
            inp_mpqr = reader["INP_MPQR"].AsString(),
            inp_feve = reader["INP_FEVE"].AsDateTime(),
            desplegar = "C",
            open = false
        };
    }
}