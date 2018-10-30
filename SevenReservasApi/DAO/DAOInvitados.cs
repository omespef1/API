using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using DTOEntities;
using Ophelia;
using Ophelia.DataBase;
using Ophelia.Comun;

namespace SevenReservas.DAO
{
    public class DAOInvitados
    {
        OException BOException = new OException();

        internal List<DTOEntities.TOInvitado> ListInvitados(DTOEntities.TOInvitado invitado)
        {
            List<Parameter> listAux = new List<Parameter>();
            try
            {
                var ahora = DateTime.Now.Date; //.AddDays(-20);
                //ahora = ahora.AddDays(2);
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT SO_INVIT.INV_NOMB, SO_INVIT.INV_APEL, SO_INVIT.INV_FENT ");
                sql.Append(" FROM SO_INGRE, SO_INVIT                                        ");
                sql.Append(" WHERE SO_INGRE.EMP_CODI = SO_INVIT.EMP_CODI                    ");
                sql.Append(" AND SO_INGRE.ING_CONT = SO_INVIT.ING_CONT                      ");
                sql.Append(" AND SO_INGRE.EMP_CODI = @P_EMP_CODI                            ");
                //sql.Append(" AND SO_INGRE.SBE_CONT = @P_SBE_CONT                            ");
                sql.Append(" AND SO_INGRE.ING_IDEN = @P_SBE_CODI                            ");
                sql.Append(" AND CAST(SO_INVIT.INV_FENT AS DATE) >= @P_FEC_INI              ");
                sql.Append(" ORDER BY 3 ASC                                                 ");

                listAux.Add(new Parameter("@P_EMP_CODI", invitado.Emp_Codi));
                //listAux.Add(new Parameter("@P_SBE_CONT", invitado.Sbe_Cont));
                listAux.Add(new Parameter("@P_FEC_INI", ahora));
                listAux.Add(new Parameter("@P_SBE_CODI", invitado.Sbe_codi));

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

        public Func<IDataReader, TOInvitado> Make = reader => new TOInvitado
        {
            Nombre = reader["INV_NOMB"].AsString(),
            Apellido = reader["INV_APEL"].AsString(),
            Fecha = reader["INV_FENT"].AsDateTime()
        };
    }
}