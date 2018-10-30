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

namespace SevenReservas.DAO
{
    public class DAOAgenda
    {
        OException BOException = new OException();

        public void GetFechaMinMax(short emp_codi, int year, int month, int day, int esp_cont, out DateTime fechaIni, out DateTime fechaFin)
        {
            List<Parameter> listAux = new List<Parameter>();
            fechaIni = new DateTime();
            fechaFin = new DateTime();


            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT MIN(DHO_HORI) DHO_HORI,MAX(DHO_HORF) DHO_HORF  FROM AE_HORAR,AE_DHORA");
                sql.Append(" WHERE AE_HORAR.EMP_CODI = AE_DHORA.EMP_CODI");
                sql.Append(" AND AE_HORAR.HOR_CONT = AE_DHORA.HOR_CONT");
                sql.Append(" AND AE_HORAR.EMP_CODI = @P_EMP_CODI");
                sql.Append(" AND AE_HORAR.HOR_ESTA = 'A'");
                sql.Append(" AND YEAR(DHO_FECH) = @P_YEAR");
                sql.Append(" AND MONTH(DHO_FECH) = @P_MONTH");
                sql.Append(" AND DAY(DHO_FECH) = @P_DAY");
                sql.Append(" AND AE_HORAR.ESP_CONT = @P_ESP_CONT");



                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_ESP_CONT", esp_cont));
                listAux.Add(new Parameter("@P_YEAR", year));
                listAux.Add(new Parameter("@P_MONTH", month));
                listAux.Add(new Parameter("@P_DAY", day));

                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.Read(pTOContext, sql.ToString(), MakeFechaMinMax, oParameter);

                fechaIni = objeto.FechaInicio;
                fechaFin = objeto.FechaFin;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);

            }
        }

        public Func<IDataReader, TODisponible> MakeFechaMinMax = reader => new TODisponible
        {

            FechaFin = reader["DHO_HORF"].AsDateTime(),
            FechaInicio = reader["DHO_HORI"].AsDateTime()
            //esp_cont = reader["ESP_CONT"].AsInt16()
        };

        public List<TOOcupado> GetOcupados(short emp_codi, int esp_cont, int year, int month, int day)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT AE_ESPAC.ESP_CONT, AE_ESPAC.ESP_CODI, AE_ESPAC.ESP_NOMB, AE_ESPAC.ESP_ESTA,");
                sql.Append(" AE_RESER.RES_CONT, AE_RESER.RES_FINI, AE_RESER.RES_FINA, AE_RESER.RES_ESTA,");
                sql.Append(" AE_RESER.RES_COLO, AE_RESER.RES_EVEN, AE_RESER.RES_OPCI, AE_RESER.RES_TIPO,");
                sql.Append(" (SELECT TOP 1 SUBSTRING(DOB_OBSE, 1, 255) FROM AE_DOBSE WHERE EMP_CODI = AE_RESER.EMP_CODI AND RES_CONT = AE_RESER.RES_CONT) RES_DESC");
                sql.Append(" FROM AE_ESPAC,AE_RESER");
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_RESER.EMP_CODI");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_RESER.ESP_CONT");
                sql.Append(" AND AE_ESPAC.EMP_CODI = @P_EMP_CODI");
                sql.Append(" AND AE_ESPAC.ESP_CONT = @P_ESP_CONT");
                sql.Append(" AND YEAR(AE_RESER.RES_FINI) = @P_YEAR");
                sql.Append(" AND MONTH(AE_RESER.RES_FINI) = @P_MONTH");
                sql.Append(" AND DAY(AE_RESER.RES_FINI) = @P_DAY");
                sql.Append(" AND AE_RESER.RES_ESTA IN('R', 'C', 'U', 'E')");
                sql.Append(" AND AE_RESER.RES_TIPO = 'U'");
                sql.Append(" UNION");
                sql.Append(" SELECT AE_ESPAC.ESP_CONT, AE_ESPAC.ESP_CODI, AE_ESPAC.ESP_NOMB, AE_ESPAC.ESP_ESTA,");
                sql.Append(" AE_RESER.RES_CONT, AE_RESER.RES_FINI, AE_RESER.RES_FINA, AE_RESER.RES_ESTA,");
                sql.Append(" AE_RESER.RES_COLO, AE_RESER.RES_EVEN, AE_RESER.RES_OPCI, AE_RESER.RES_TIPO,");
                sql.Append(" 'No Disponible' RES_DESC");
                sql.Append(" FROM AE_ESPAC, AE_RESER");
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_RESER.EMP_CODI");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_RESER.ESP_CONT");
                sql.Append(" AND AE_ESPAC.EMP_CODI = @P_EMP_CODI");
                sql.Append(" AND AE_ESPAC.ESP_CONT = @P_ESP_CONT");
                sql.Append(" AND YEAR(AE_RESER.RES_FINI) = @P_YEAR");
                sql.Append(" AND MONTH(AE_RESER.RES_FINI) = @P_MONTH");
                sql.Append(" AND DAY(AE_RESER.RES_FINI) = @P_DAY");
                sql.Append(" AND AE_RESER.RES_ESTA = 'C'");
                sql.Append(" AND AE_RESER.RES_TIPO = 'I'");
                sql.Append(" UNION");
                sql.Append(" SELECT AE_ESPAC.ESP_CONT, AE_ESPAC.ESP_CODI, AE_ESPAC.ESP_NOMB, AE_ESPAC.ESP_ESTA,");
                sql.Append(" NULL, AE_MANTE.MAN_FINI, AE_MANTE.MAN_FFIN, 'M' RES_ESTA,");
                sql.Append(" 0 RES_CONT, 0 RES_EVEN, 2 RES_OPCI, 'U' RES_TIPO,");
                sql.Append(" 'Mantenimiento' RES_DESC");
                sql.Append(" FROM AE_ESPAC, AE_MANTE");
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_MANTE.EMP_CODI");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_MANTE.ESP_CONT");
                sql.Append(" AND AE_ESPAC.EMP_CODI = @P_EMP_CODI");
                sql.Append(" AND AE_ESPAC.ESP_CONT = @P_ESP_CONT");
                sql.Append(" AND YEAR(AE_MANTE.MAN_FINI) = @P_YEAR");
                sql.Append(" AND MONTH(AE_MANTE.MAN_FINI) = @P_MONTH");
                sql.Append(" AND DAY(AE_MANTE.MAN_FINI) = @P_DAY");
                sql.Append(" ORDER BY 6");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_ESP_CONT", esp_cont));
                listAux.Add(new Parameter("@P_YEAR", year));
                listAux.Add(new Parameter("@P_MONTH", month));
                listAux.Add(new Parameter("@P_DAY", day));

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

        public List<TODisponible> GetHorariosTercero(int year, int month, int day, int cla_cont, int pro_cont, int ter_codi, string hor_disp, string Op_Disp)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT DISTINCT DHO_HORI, DHO_HORF                                                  ");
                sql.Append(" FROM AE_HORAR, AE_DHORA, AE_CLASE, AE_ESPAC,AE_DESPA,AE_ESPTE                       ");
                sql.Append(" WHERE AE_HORAR.EMP_CODI = AE_DHORA.EMP_CODI                                         ");
                sql.Append(" AND AE_HORAR.HOR_CONT = AE_DHORA.HOR_CONT AND AE_CLASE.EMP_CODI = AE_ESPAC.EMP_CODI ");
                sql.Append(" AND AE_CLASE.CLA_CONT = AE_ESPAC.CLA_CONT AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI AND AE_HORAR.EMP_CODI = AE_ESPTE.EMP_CODI ");
                sql.Append(" AND AE_HORAR.TER_CODI = AE_ESPTE.TER_CODI AND AE_ESPAC.ESP_CONT = AE_ESPTE.ESP_CONT ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_ESPTE.EMP_CODI AND AE_HORAR.HOR_ESTA = 'A'               ");
                sql.Append(" AND YEAR(DHO_FECH) = @P_YEAR AND MONTH(DHO_FECH) = @P_MONTH                         ");
                sql.Append(" AND DAY(DHO_FECH) = @P_DAY AND AE_CLASE.CLA_CONT = @P_CLA_CONT                      ");
                sql.Append(" AND AE_DESPA.PRO_CONT = @P_PRO_CONT                                                 ");
                if (Op_Disp != "F")
                    sql.Append(" AND AE_ESPTE.TER_CODI = @P_TER_CODI                                             ");
                sql.Append(" AND AE_HORAR.HOR_DISP = @P_DISP                                                     ");

                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter("@P_YEAR", year));
                parameters.Add(new Parameter("@P_MONTH", month));
                parameters.Add(new Parameter("@P_DAY", day));
                parameters.Add(new Parameter("@P_CLA_CONT", cla_cont));
                parameters.Add(new Parameter("@P_PRO_CONT", pro_cont));
                if (Op_Disp != "F")
                    parameters.Add(new Parameter("@P_TER_CODI", ter_codi));
                parameters.Add(new Parameter("@P_DISP", hor_disp));

                OTOContext pTOContext = new OTOContext();
                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.ReadList(pTOContext, sql.ToString(), HorariosTercero, parameters.ToArray());
                return objeto;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        public Func<IDataReader, TODisponible> HorariosTercero = reader => new TODisponible
        {
            FechaInicio = reader["DHO_HORI"].AsDateTime(),
            FechaFin = reader["DHO_HORF"].AsDateTime()
        };
        public List<TONoHorDisponible> GetHorariosTerceroNoDisp(int year, int month, int day, int cla_cont, int pro_cont, int ter_codi, string hor_disp, string Op_Disp, DateTime fini , DateTime ffin)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT DISTINCT AE_ESPTE.TER_CODI                                                      ");
                sql.Append(" FROM AE_HORAR, AE_DHORA, AE_CLASE, AE_ESPAC,AE_DESPA,AE_ESPTE                       ");
                sql.Append(" WHERE AE_HORAR.EMP_CODI = AE_DHORA.EMP_CODI                                         ");
                sql.Append(" AND AE_HORAR.HOR_CONT = AE_DHORA.HOR_CONT AND AE_CLASE.EMP_CODI = AE_ESPAC.EMP_CODI ");
                sql.Append(" AND AE_CLASE.CLA_CONT = AE_ESPAC.CLA_CONT AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI AND AE_HORAR.EMP_CODI = AE_ESPTE.EMP_CODI ");
                sql.Append(" AND AE_HORAR.TER_CODI = AE_ESPTE.TER_CODI AND AE_ESPAC.ESP_CONT = AE_ESPTE.ESP_CONT ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_ESPTE.EMP_CODI AND AE_HORAR.HOR_ESTA = 'A'               ");
                sql.Append(" AND YEAR(DHO_FECH) = @P_YEAR AND MONTH(DHO_FECH) = @P_MONTH                         ");
                sql.Append(" AND DAY(DHO_FECH) = @P_DAY AND AE_CLASE.CLA_CONT = @P_CLA_CONT                      ");
                sql.Append(" AND AE_DESPA.PRO_CONT = @P_PRO_CONT                                                 ");              
               sql.Append(" AND AE_ESPTE.TER_CODI = @P_TER_CODI                                             ");
                sql.Append(" AND AE_HORAR.HOR_DISP = @P_DISP                                                     ");
                sql.Append(" AND  @FechaIni < DHO_HORF  AND @FechaFin > DHO_HORI    ");
                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter("@P_YEAR", year));
                parameters.Add(new Parameter("@P_MONTH", month));
                parameters.Add(new Parameter("@P_DAY", day));
                parameters.Add(new Parameter("@P_CLA_CONT", cla_cont));
                parameters.Add(new Parameter("@P_PRO_CONT", pro_cont));
                parameters.Add(new Parameter("@FechaIni", fini));
                parameters.Add(new Parameter("@FechaFin", ffin));             
                parameters.Add(new Parameter("@P_TER_CODI", ter_codi));
                parameters.Add(new Parameter("@P_DISP", hor_disp));

                OTOContext pTOContext = new OTOContext();
                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.ReadList(pTOContext, sql.ToString(), HorariosTerceroNoDisp, parameters.ToArray());
                return objeto;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }
        public Func<IDataReader, TONoHorDisponible> HorariosTerceroNoDisp = reader => new TONoHorDisponible
        {
            TER_CODI = reader["TER_CODI"].AsDecimal(),
          
        };
        public int GetEspSinReservaTercero(int emp_codi, int year, int month, int day, int cla_cont, int pro_cont, DateTime fecha_ini, DateTime fecha_fin, int ter_codi)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                /****SOLUCION RESERVAS REMONTADAS APLICAR AERESER.exe****/

                sql.Append(" SELECT AE_ESPAC.ESP_CONT                                                                    ");
                sql.Append(" FROM AE_HORAR, AE_DHORA, AE_CLASE, AE_ESPAC,AE_DESPA                                        ");
                sql.Append("     ,AE_ESPTE                                                                               ");
                sql.Append(" WHERE AE_HORAR.EMP_CODI = AE_DHORA.EMP_CODI                                                 ");
                sql.Append(" AND AE_HORAR.HOR_CONT = AE_DHORA.HOR_CONT                                                   ");
                sql.Append(" AND AE_CLASE.EMP_CODI = AE_ESPAC.EMP_CODI                                                   ");
                sql.Append(" AND AE_CLASE.CLA_CONT = AE_ESPAC.CLA_CONT                                                   ");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT                                                   ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI                                                   ");
                sql.Append(" AND AE_HORAR.EMP_CODI = AE_ESPAC.EMP_CODI                                                   ");
                sql.Append(" AND AE_HORAR.ESP_CONT = AE_ESPAC.ESP_CONT                                                   ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_ESPTE.EMP_CODI                                                   ");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_ESPTE.ESP_CONT                                                   ");
                sql.Append(" AND NOT EXISTS(SELECT  AE_RESER.ESP_CONT FROM AE_RESER                                      ");
                sql.Append("                 WHERE AE_RESER.EMP_CODI = AE_ESPAC.EMP_CODI                                 ");
                sql.Append("                     AND AE_RESER.ESP_CONT = AE_ESPAC.ESP_CONT                               ");
                sql.Append("                     AND AE_RESER.RES_ESTA IN('C', 'U', 'R', 'P')                            ");
                sql.Append("                     AND(@DHO_HORI < AE_RESER.RES_FINA AND @DHO_HORF > AE_RESER.RES_FINI)    ");
                sql.Append("                 )                                                                           ");
                sql.Append(" AND AE_HORAR.HOR_ESTA = 'A'                                                                 ");
                sql.Append(" AND YEAR(DHO_FECH) = @ANOP                                                                  ");
                sql.Append(" AND MONTH(DHO_FECH) = @MESP                                                                 ");
                sql.Append(" AND DAY(DHO_FECH) = @DIAP                                                                   ");
                sql.Append(" AND AE_CLASE.CLA_CONT = @CLA_CONT                                                           ");
                sql.Append(" AND AE_DESPA.PRO_CONT = @PRO_CONT                                                           ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = @EMP_CODI                                                           ");
                sql.Append(" AND @DHO_HORI BETWEEN AE_DHORA.DHO_HORI AND AE_DHORA.DHO_HORF                               ");
                sql.Append(" AND @DHO_HORF BETWEEN AE_DHORA.DHO_HORI AND AE_DHORA.DHO_HORF                               ");
                sql.Append(" AND AE_ESPTE.TER_CODI = @TER_CODI                                                           ");
                sql.Append(" GROUP BY AE_ESPAC.ESP_CONT, AE_DHORA.DHO_HORI, AE_DHORA.DHO_HORF                            ");

                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter("@DHO_HORI", fecha_ini));
                parameters.Add(new Parameter("@DHO_HORF", fecha_fin));
                parameters.Add(new Parameter("@ANOP", year));
                parameters.Add(new Parameter("@MESP", month));
                parameters.Add(new Parameter("@DIAP", day));
                parameters.Add(new Parameter("@CLA_CONT", cla_cont));
                parameters.Add(new Parameter("@PRO_CONT", pro_cont));
                parameters.Add(new Parameter("@EMP_CODI", emp_codi));
                parameters.Add(new Parameter("@TER_CODI", ter_codi));

                OTOContext pTOContext = new OTOContext();
                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.GetScalar(pTOContext, sql.ToString(), parameters.ToArray());
                return objeto.AsInt();
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return 0;
            }
        }



        public int GetEspSinReserva(int emp_codi, int year, int month, int day, int cla_cont, int pro_cont, DateTime fecha_ini, DateTime fecha_fin)
        {
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT AE_ESPAC.ESP_CONT ");
                sql.Append(" FROM AE_HORAR, AE_DHORA, AE_CLASE, AE_ESPAC,AE_DESPA ");
                sql.Append(" WHERE AE_HORAR.EMP_CODI = AE_DHORA.EMP_CODI ");
                sql.Append(" AND AE_HORAR.HOR_CONT = AE_DHORA.HOR_CONT ");
                sql.Append(" AND AE_CLASE.EMP_CODI = AE_ESPAC.EMP_CODI ");
                sql.Append(" AND AE_CLASE.CLA_CONT = AE_ESPAC.CLA_CONT ");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI ");
                sql.Append(" AND AE_HORAR.EMP_CODI = AE_ESPAC.EMP_CODI ");
                sql.Append(" AND AE_HORAR.ESP_CONT = AE_ESPAC.ESP_CONT ");
                sql.Append(" AND NOT EXISTS(SELECT  AE_RESER.ESP_CONT FROM AE_RESER ");
                sql.Append("                 WHERE AE_RESER.EMP_CODI = AE_ESPAC.EMP_CODI ");
                sql.Append("                            AND AE_RESER.ESP_CONT = AE_ESPAC.ESP_CONT ");
                sql.Append("                            AND AE_RESER.RES_ESTA IN('C', 'U', 'R', 'P') ");
                sql.Append("                            AND (@P1 < AE_RESER.RES_FINA AND @P2 > AE_RESER.RES_FINA OR @P1 < AE_RESER.RES_FINI AND @P2 > AE_RESER.RES_FINI) ");


                //AND((@P_RES_FINI < AE_RESER.RES_FINA AND @P_RES_FINA >= AE_RESER.RES_FINA) OR(@P_RES_FINI <= AE_RESER.RES_FINI AND @P_RES_FINA > AE_RESER.RES_FINI)))

                //sql.Append("                            AND  DATEADD(MINUTE, 1, @P1)  BETWEEN AE_RESER.RES_FINI AND AE_RESER.RES_FINA ");
                //sql.Append("                            AND  DATEADD(MINUTE, -1, @P2)  BETWEEN AE_RESER.RES_FINI AND AE_RESER.RES_FINA ");
                sql.Append("                 ) ");
                sql.Append(" AND AE_HORAR.HOR_ESTA = 'A' ");
                sql.Append(" AND YEAR(DHO_FECH) = @P3 ");
                sql.Append(" AND MONTH(DHO_FECH) = @P4 ");
                sql.Append(" AND DAY(DHO_FECH) = @P5 ");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P6 ");
                sql.Append(" AND AE_DESPA.PRO_CONT = @P7 ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = @P8 ");
                sql.Append(" AND  DATEADD(MINUTE, 1, @P1)  BETWEEN AE_DHORA.DHO_HORI AND AE_DHORA.DHO_HORF ");
                sql.Append(" AND  DATEADD(MINUTE, -1, @P2)  BETWEEN AE_DHORA.DHO_HORI AND AE_DHORA.DHO_HORF ");
                sql.Append(" GROUP BY AE_ESPAC.ESP_CONT, AE_DHORA.DHO_HORI, AE_DHORA.DHO_HORF ");

                List<Parameter> parameters = new List<Parameter>();
                parameters.Add(new Parameter("@P1", fecha_ini));
                parameters.Add(new Parameter("@P2", fecha_fin));
                parameters.Add(new Parameter("@P3", year));
                parameters.Add(new Parameter("@P4", month));
                parameters.Add(new Parameter("@P5", day));
                parameters.Add(new Parameter("@P6", cla_cont));
                parameters.Add(new Parameter("@P7", pro_cont));
                parameters.Add(new Parameter("@P8", emp_codi));

                OTOContext pTOContext = new OTOContext();
                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.GetScalar(pTOContext, sql.ToString(), parameters.ToArray());
                return objeto.AsInt();
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return 0;
            }
        }

        public Func<IDataReader, TOOcupado> Make = reader => new TOOcupado
        {
            Esp_esta = reader["RES_ESTA"].AsString(),
            Esp_codi = reader["ESP_CODI"].AsString(),
            Esp_cont = reader["ESP_CONT"].AsInt(),
            Esp_nomb = reader["ESP_NOMB"].AsString(),
            Res_colo = reader["RES_COLO"].AsInt(),
            Res_cont = reader["RES_CONT"].AsInt(),
            Res_desc = reader["RES_DESC"].AsString(),
            Res_esta = reader["RES_ESTA"].AsString(),
            Res_even = reader["RES_EVEN"].AsInt(),
            Res_fina = reader["RES_FINA"].AsDateTime(),
            Res_fini = reader["RES_FINI"].AsDateTime(),
            Res_opci = reader["RES_OPCI"].AsInt(),
            Res_tipo = reader["RES_TIPO"].AsString()
        };


        public int GetCantidadTercerosEspacio(short emp_codi, int cla_cont) {
            List<Parameter> listAux = new List<Parameter>();
            try
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine(" SELECT COUNT(*) FROM (                           ");
	            sql.AppendLine("     SELECT GN_TERCE.TER_CODI                     ");
	            sql.AppendLine("     FROM AE_ESPTE, AE_ESPAC , GN_TERCE           ");
	            sql.AppendLine("     WHERE AE_ESPTE.EMP_CODI = AE_ESPAC.EMP_CODI  ");
	            sql.AppendLine("     AND AE_ESPTE.ESP_CONT = AE_ESPAC.ESP_CONT    ");
	            sql.AppendLine("     AND AE_ESPTE.TER_CODI = GN_TERCE.TER_CODI    ");
	            sql.AppendLine("     AND AE_ESPTE.EMP_CODI = @P_EMP_CODI          ");
	            sql.AppendLine("     AND AE_ESPAC.CLA_CONT = @P_CLA_CONT          ");
	            sql.AppendLine("     GROUP BY GN_TERCE.TER_CODI                   ");
                sql.AppendLine(" ) AS X                                           ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_CLA_CONT", cla_cont));
                
                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.GetScalar(pTOContext, sql.ToString(), oParameter);
                return objeto.AsInt();
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return 0;
            }
        }

        public int GetReservaTercerosEspacio(short emp_codi, int cla_cont, DateTime agt_fini, DateTime agt_ffin)
        {
            List<Parameter> listAux = new List<Parameter>();
            try
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine(" SELECT COUNT(*) AS QUANTITY FROM (                                                           ");
	            sql.AppendLine("     SELECT DISTINCT TER_CODI FROM AE_AGTER                                                   ");
	            sql.AppendLine("     WHERE AGT_ESTA IN('R', 'C', 'U')                                                         ");
	            sql.AppendLine("     AND EMP_CODI = @P_EMP_CODI                                                               ");
	            sql.AppendLine("     AND TER_CODI IN (                                                                        ");
		        sql.AppendLine("         SELECT TER_CODI                                                                      ");
		        sql.AppendLine("         FROM AE_ESPTE, AE_ESPAC                                                              ");
		        sql.AppendLine("         WHERE AE_ESPTE.EMP_CODI = AE_ESPAC.EMP_CODI                                          ");
		        sql.AppendLine("         AND AE_ESPTE.ESP_CONT = AE_ESPAC.ESP_CONT                                            ");
		        sql.AppendLine("         AND AE_ESPTE.EMP_CODI = @P_EMP_CODI                                                  ");
		        sql.AppendLine("         AND AE_ESPAC.CLA_CONT = @P_CLA_CONT                                                  ");
	            sql.AppendLine("     )                                                                                        ");
	            sql.AppendLine("     AND(                                                                                     ");
		        sql.AppendLine("         (@P_AGT_FINI < AGT_FFIN AND @P_AGT_FFIN >= AGT_FFIN)                                 ");
		        sql.AppendLine("         OR(@P_AGT_FINI <= AGT_FINI AND @P_AGT_FFIN > AGT_FINI)                               ");
	            sql.AppendLine("     )                                                                                        ");
                sql.AppendLine(" ) AS X                                                                                       ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_CLA_CONT", cla_cont));
                listAux.Add(new Parameter("@P_AGT_FINI", agt_fini));
                listAux.Add(new Parameter("@P_AGT_FFIN", agt_ffin));
                
                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.GetScalar(pTOContext, sql.ToString(), oParameter);
                return objeto.AsInt();
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return 0;
            }
        }


        public int GetReservaTercero(short emp_codi, int ter_codi, int pro_cont, DateTime agt_fini, DateTime agt_ffin)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();
                /*sql.Append(" SELECT AE_AGTER.RES_CONT ");
                sql.Append(" FROM AE_AGTER, AE_ESPAC, AE_DESPA, AE_RESER ");
                sql.Append(" WHERE AE_AGTER.RES_CONT = AE_RESER.RES_CONT AND AE_ESPAC.ESP_CONT = AE_RESER.ESP_CONT AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND AGT_ESTA IN('R', 'C', 'U') AND AE_AGTER.EMP_CODI = @P_EMP_CODI AND AE_AGTER.TER_CODI = @P_TER_CODI  AND @P_AGT_FINI >= AGT_FINI ");
                sql.Append(" AND @P_AGT_FINI < AGT_FFIN AND AE_DESPA.PRO_CONT = @P_PRO_CONT ");
                sql.Append(" UNION ALL ");
                sql.Append(" SELECT AE_AGTER.RES_CONT FROM AE_AGTER, AE_ESPAC, AE_DESPA, AE_RESER ");
                sql.Append(" WHERE AE_AGTER.RES_CONT = AE_RESER.RES_CONT AND AE_ESPAC.ESP_CONT = AE_RESER.ESP_CONT AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT ");
                sql.Append(" AND AGT_ESTA IN('R', 'C', 'U')  AND AE_AGTER.EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND AE_AGTER.TER_CODI = @P_TER_CODI  AND @P_AGT_FFIN > AGT_FINI AND @P_AGT_FFIN <= AGT_FFIN AND AE_DESPA.PRO_CONT = @P_PRO_CONT ");
                sql.Append(" UNION ALL ");
                sql.Append(" SELECT AE_AGTER.RES_CONT ");
                sql.Append(" FROM AE_AGTER, AE_ESPAC, AE_DESPA, AE_RESER ");
                sql.Append(" WHERE AE_AGTER.RES_CONT = AE_RESER.RES_CONT AND AE_ESPAC.ESP_CONT = AE_RESER.ESP_CONT AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT ");
                sql.Append(" AND AGT_ESTA IN('R', 'C', 'U')  AND AE_AGTER.EMP_CODI = @P_EMP_CODI  AND AE_AGTER.TER_CODI = @P_TER_CODI  AND AGT_FINI > @P_AGT_FINI ");
                sql.Append(" AND AGT_FFIN < @P_AGT_FFIN AND AE_DESPA.PRO_CONT = @P_PRO_CONT ");*/


                //INICIO
                /*sql.Append(" SELECT RES_CONT FROM AE_AGTER ");
                sql.Append(" WHERE AGT_ESTA IN('R', 'C', 'U') "); //--'); //reservada, confirmada y utilizada
                sql.Append(" AND EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND TER_CODI = @P_TER_CODI ");
                sql.Append(" AND @P_AGT_FINI >= AGT_FINI ");
                sql.Append(" AND @P_AGT_FINI < AGT_FFIN ");
                sql.Append(" UNION ALL");
                //FINAL
                sql.Append(" SELECT RES_CONT FROM AE_AGTER ");
                sql.Append(" WHERE AGT_ESTA IN('R', 'C', 'U') "); //--'); //reservada, confirmada y utilizada
                sql.Append(" AND EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND TER_CODI = @P_TER_CODI ");
                sql.Append(" AND @P_AGT_FFIN > AGT_FINI ");
                sql.Append(" AND @P_AGT_FFIN <= AGT_FFIN ");
                sql.Append(" UNION ALL ");
                //SUBRANGOS
                sql.Append(" SELECT RES_CONT FROM AE_AGTER ");
                sql.Append(" WHERE AGT_ESTA IN('R', 'C', 'U') "); //--'); //reservada, confirmada y utilizada
                sql.Append(" AND EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND TER_CODI = @P_TER_CODI ");
                sql.Append(" AND AGT_FINI > @P_AGT_FINI ");
                sql.Append(" AND AGT_FFIN < @P_AGT_FFIN ");*/

                sql.Append(" SELECT COUNT(RES_CONT) FROM AE_AGTER ");
                sql.Append(" WHERE AGT_ESTA IN('R', 'C', 'U') ");
                sql.Append(" AND EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND TER_CODI = @P_TER_CODI ");
                sql.Append(" AND((@P_AGT_FINI < AGT_FFIN AND @P_AGT_FFIN >= AGT_FFIN) OR(@P_AGT_FINI <= AGT_FINI AND @P_AGT_FFIN > AGT_FINI)) ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_TER_CODI", ter_codi));
                listAux.Add(new Parameter("@P_AGT_FINI", agt_fini));
                listAux.Add(new Parameter("@P_AGT_FFIN", agt_ffin));
                //listAux.Add(new Parameter("@P_PRO_CONT", pro_cont));


                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.GetScalar(pTOContext, sql.ToString(), oParameter);
                return objeto.AsInt();
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return 0;
            }
        }

        public int GetHorarioDisponible(short emp_codi, int ter_codi, DateTime agt_fini, DateTime agt_ffin)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT 1 FROM AE_HORAR, AE_DHORA ");
                sql.Append(" WHERE AE_HORAR.EMP_CODI = AE_DHORA.EMP_CODI ");
                sql.Append(" AND AE_HORAR.HOR_CONT = AE_DHORA.HOR_CONT ");
                sql.Append(" AND AE_HORAR.HOR_ESTA = 'A' "); //estado: activo
                sql.Append(" AND AE_HORAR.HOR_DISP = 'D' "); //'); //clase: disponibilidad
                sql.Append(" AND AE_HORAR.EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND AE_HORAR.TER_CODI = @P_TER_CODI ");
                sql.Append(" AND DHO_HORI <= @P_DHO_HORI ");
                sql.Append(" AND DHO_HORF >= @P_DHO_HORF ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_TER_CODI", ter_codi));
                listAux.Add(new Parameter("@P_DHO_HORI", agt_fini));
                listAux.Add(new Parameter("@P_DHO_HORF", agt_ffin));

                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.GetScalar(pTOContext, sql.ToString(), oParameter);
                return objeto.AsInt();
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return 0;
            }
        }


        public void GetFechaMinMaxDiponibilidad(short emp_codi, int year, int month, int day, int cla_cont, out DateTime fechaIni, out DateTime fechaFin, int pro_cont, string esp_mdit, float ter_codi, string Op_Disp)
        {
            List<Parameter> listAux = new List<Parameter>();
            fechaIni = new DateTime();
            fechaFin = new DateTime();
            try
            {
                StringBuilder sql = new StringBuilder();
                //sql.Append(" SELECT TOP 1 MIN(DHO_HORI) DHO_HORI,MAX(DHO_HORF) DHO_HORF, AE_ESPAC.ESP_CONT");
                sql.Append(" SELECT TOP 1 MIN(DHO_HORI) DHO_HORI,MAX(DHO_HORF) DHO_HORF");

                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" FROM AE_HORAR, AE_DHORA, AE_CLASE, AE_ESPAC,AE_DESPA,AE_ESPTE");
                }
                else
                {
                    sql.Append(" FROM AE_HORAR, AE_DHORA, AE_CLASE, AE_ESPAC,AE_DESPA");
                }
                sql.Append(" WHERE AE_HORAR.EMP_CODI = AE_DHORA.EMP_CODI");
                sql.Append(" AND AE_HORAR.HOR_CONT = AE_DHORA.HOR_CONT");
                sql.Append(" AND AE_CLASE.EMP_CODI = AE_ESPAC.EMP_CODI");
                sql.Append(" AND AE_CLASE.CLA_CONT = AE_ESPAC.CLA_CONT");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" AND AE_HORAR.EMP_CODI = AE_ESPTE.EMP_CODI");
                    sql.Append(" AND AE_HORAR.TER_CODI = AE_ESPTE.TER_CODI");
                    sql.Append(" AND AE_ESPAC.ESP_CONT = AE_ESPTE.ESP_CONT");
                    sql.Append(" AND AE_ESPAC.EMP_CODI = AE_ESPTE.EMP_CODI");
                }
                else
                {
                    sql.Append(" AND AE_HORAR.EMP_CODI = AE_ESPAC.EMP_CODI");
                    sql.Append(" AND AE_HORAR.ESP_CONT = AE_ESPAC.ESP_CONT");
                }
                sql.Append(" AND AE_HORAR.HOR_ESTA = 'A'");
                sql.Append(" AND YEAR(DHO_FECH) = @P_YEAR");
                sql.Append(" AND MONTH(DHO_FECH) = @P_MONTH");
                sql.Append(" AND DAY(DHO_FECH) = @P_DAY");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT");
                sql.Append(" AND AE_DESPA.PRO_CONT = @P_PRO_CONT");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" AND AE_ESPTE.TER_CODI = @P_TER_CODI");
                }
                sql.Append(" GROUP BY AE_ESPAC.ESP_CONT");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" ORDER BY NEWID()");
                }
                else
                {
                    StringBuilder sql1 = new StringBuilder();
                    sql1.Append("SELECT (SELECT MIN(DHO_HORI) FROM ( ");
                    sql1.Append(sql.ToString().Replace("TOP 1", ""));
                    sql1.Append(") AS X ) as DHO_HORI, (SELECT MAX(DHO_HORF) FROM (");
                    sql1.Append(sql.ToString().Replace("TOP 1", ""));
                    sql1.Append(") AS X) AS DHO_HORF");
                    sql = sql1;
                }

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_CLA_CONT", cla_cont));
                listAux.Add(new Parameter("@P_PRO_CONT", pro_cont));
                listAux.Add(new Parameter("@P_YEAR", year));
                listAux.Add(new Parameter("@P_MONTH", month));
                listAux.Add(new Parameter("@P_DAY", day));
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    listAux.Add(new Parameter("@P_TER_CODI", ter_codi));
                }
                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.Read(pTOContext, sql.ToString(), MakeFechaMinMax, oParameter);
                if (objeto == null)
                {
                    fechaIni = DateTime.Today;
                    fechaFin = DateTime.Today;
                }
                else
                {
                    fechaIni = objeto.FechaInicio;
                    fechaFin = objeto.FechaFin;
                }
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);

            }
        }

        public void GetEspContDisponible(short emp_codi, int year, int month, int day, int cla_cont, int pro_cont, out int esp_cont, DateTime res_fini, DateTime res_fina, string esp_mdit = "N", double ter_codi = 0)
        {
            esp_cont = 0;
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT TOP 1 AE_ESPAC.ESP_CONT ");
                /*if (esp_mdit == "S")
                {
                    sql.Append(" FROM AE_HORAR, AE_DHORA, AE_CLASE, AE_ESPAC,AE_DESPA,AE_ESPTE");
                }
                else
                {
                    sql.Append(" FROM AE_HORAR, AE_DHORA, AE_CLASE, AE_ESPAC,AE_DESPA");
                }*/
                if (esp_mdit == "S")
                {
                    sql.Append(" FROM AE_HORAR, AE_DHORA, AE_CLASE, AE_ESPAC,AE_DESPA,AE_ESPTE");
                }
                else
                {
                    sql.Append(" FROM AE_HORAR, AE_DHORA, AE_CLASE, AE_ESPAC,AE_DESPA");
                }

                sql.Append(" WHERE AE_HORAR.EMP_CODI = AE_DHORA.EMP_CODI");
                sql.Append(" AND AE_HORAR.HOR_CONT = AE_DHORA.HOR_CONT");
                sql.Append(" AND AE_CLASE.EMP_CODI = AE_ESPAC.EMP_CODI");
                sql.Append(" AND AE_CLASE.CLA_CONT = AE_ESPAC.CLA_CONT");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI");

                if (esp_mdit == "S")
                {
                    sql.Append(" AND AE_ESPTE.ESP_CONT = AE_ESPAC.ESP_CONT ");
                    sql.Append(" AND AE_ESPAC.EMP_CODI = AE_ESPTE.EMP_CODI ");
                }

                /*if (esp_mdit == "S")
                {
                    sql.Append(" AND AE_HORAR.EMP_CODI = AE_ESPTE.EMP_CODI");
                    sql.Append(" AND AE_HORAR.TER_CODI = AE_ESPTE.TER_CODI");
                    sql.Append(" AND AE_ESPAC.ESP_CONT = AE_ESPTE.ESP_CONT");
                    sql.Append(" AND AE_ESPAC.EMP_CODI = AE_ESPTE.EMP_CODI");
                }
                else
                {
                    sql.Append(" AND AE_HORAR.EMP_CODI = AE_ESPAC.EMP_CODI");
                    sql.Append(" AND AE_HORAR.ESP_CONT = AE_ESPAC.ESP_CONT");
                }*/
                sql.Append(" AND AE_HORAR.EMP_CODI = AE_ESPAC.EMP_CODI");
                sql.Append(" AND AE_HORAR.ESP_CONT = AE_ESPAC.ESP_CONT");

                sql.Append(" AND AE_HORAR.HOR_ESTA = 'A'");
                sql.Append(" AND YEAR(DHO_FECH) = @P_YEAR");
                sql.Append(" AND MONTH(DHO_FECH) = @P_MONTH");
                sql.Append(" AND DAY(DHO_FECH) = @P_DAY");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT");
                sql.Append(" AND AE_DESPA.PRO_CONT = @P_PRO_CONT");
                if (esp_mdit == "S")
                {
                    sql.Append(" AND AE_ESPTE.TER_CODI = @P_TER_CODI");
                }
                sql.Append(" AND AE_DHORA.DHO_HORI <= @P_RES_FINI AND AE_DHORA.DHO_HORF >= @P_RES_FINA ");


                List<Parameter> listAux = new List<Parameter>();
                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_CLA_CONT", cla_cont));
                listAux.Add(new Parameter("@P_PRO_CONT", pro_cont));
                listAux.Add(new Parameter("@P_YEAR", year));
                listAux.Add(new Parameter("@P_MONTH", month));
                listAux.Add(new Parameter("@P_DAY", day));
                listAux.Add(new Parameter("@P_RES_FINI", res_fini));
                listAux.Add(new Parameter("@P_RES_FINA", res_fina));
                if (esp_mdit == "S")
                {
                    listAux.Add(new Parameter("@P_TER_CODI", ter_codi));
                }

                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.GetScalar(pTOContext, sql.ToString(), oParameter);
                esp_cont = objeto.AsInt();
                //esp_cont = objeto.esp_cont;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        public List<TOOcupado> GetOcupadosDipo(short emp_codi, int Cla_cont, int year, int month, int day, string esp_mdit, int esp_cont, int pro_cont, string Op_Disp, float Ter_codi = 0)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT AE_ESPAC.ESP_CONT, AE_ESPAC.ESP_CODI, AE_ESPAC.ESP_NOMB, AE_ESPAC.ESP_ESTA,");
                sql.Append(" AE_RESER.RES_CONT, AE_RESER.RES_FINI, AE_RESER.RES_FINA, AE_RESER.RES_ESTA,");
                sql.Append(" AE_RESER.RES_COLO, AE_RESER.RES_EVEN, AE_RESER.RES_OPCI, AE_RESER.RES_TIPO,");
                sql.Append(" (SELECT TOP 1 SUBSTRING(DOB_OBSE, 1, 255) FROM AE_DOBSE WHERE EMP_CODI = AE_RESER.EMP_CODI AND RES_CONT = AE_RESER.RES_CONT) RES_DESC");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" FROM AE_ESPAC,AE_RESER,AE_CLASE,AE_ESPTE,AE_DESPA");
                }
                else
                {
                    sql.Append(" FROM AE_ESPAC,AE_RESER,AE_CLASE,AE_DESPA");
                }
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_RESER.EMP_CODI");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_RESER.ESP_CONT");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_CLASE.EMP_CODI");
                sql.Append(" AND AE_ESPAC.CLA_CONT = AE_CLASE.CLA_CONT");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    //sql.Append(" AND AE_RESER.TER_CODI = AE_ESPTE.TER_CODI ");
                    sql.Append(" AND AE_ESPAC.EMP_CODI = AE_ESPTE.EMP_CODI ");
                    sql.Append(" AND AE_ESPAC.ESP_CONT = AE_ESPTE.ESP_CONT ");
                }
                sql.Append(" AND AE_ESPAC.EMP_CODI = @P_EMP_CODI");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI ");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT");
                //sql.Append(" AND AE_ESPAC.ESP_CONT = @P_ESP_CONT");
                sql.Append(" AND YEAR(AE_RESER.RES_FINI) = @P_YEAR");
                sql.Append(" AND MONTH(AE_RESER.RES_FINI) = @P_MONTH");
                sql.Append(" AND DAY(AE_RESER.RES_FINI) = @P_DAY");
                sql.Append(" AND AE_RESER.RES_ESTA IN('R', 'C', 'U', 'E')");
                sql.Append(" AND AE_RESER.RES_TIPO = 'U'");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" AND AE_ESPTE.TER_CODI = @P_TER_CODI ");
                }
                sql.Append(" AND AE_DESPA.PRO_CONT = @P_PRO_CONT ");
                sql.Append(" UNION");
                sql.Append(" SELECT AE_ESPAC.ESP_CONT, AE_ESPAC.ESP_CODI, AE_ESPAC.ESP_NOMB, AE_ESPAC.ESP_ESTA,");
                sql.Append(" AE_RESER.RES_CONT, AE_RESER.RES_FINI, AE_RESER.RES_FINA, AE_RESER.RES_ESTA,");
                sql.Append(" AE_RESER.RES_COLO, AE_RESER.RES_EVEN, AE_RESER.RES_OPCI, AE_RESER.RES_TIPO,");
                sql.Append(" 'No Disponible' RES_DESC");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" FROM AE_ESPAC, AE_RESER,AE_CLASE,AE_ESPTE,AE_DESPA");
                }
                else
                {
                    sql.Append(" FROM AE_ESPAC, AE_RESER,AE_CLASE,AE_DESPA");
                }
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_RESER.EMP_CODI");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_RESER.ESP_CONT");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_CLASE.EMP_CODI");
                sql.Append(" AND AE_ESPAC.CLA_CONT = AE_CLASE.CLA_CONT");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" AND AE_ESPAC.EMP_CODI = AE_ESPTE.EMP_CODI ");
                    sql.Append(" AND AE_ESPAC.ESP_CONT = AE_ESPTE.ESP_CONT ");
                }
                sql.Append(" AND AE_ESPAC.EMP_CODI = @P_EMP_CODI");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI ");
                //sql.Append(" AND AE_ESPAC.ESP_CONT = @P_ESP_CONT");
                sql.Append(" AND YEAR(AE_RESER.RES_FINI) = @P_YEAR");
                sql.Append(" AND MONTH(AE_RESER.RES_FINI) = @P_MONTH");
                sql.Append(" AND DAY(AE_RESER.RES_FINI) = @P_DAY");
                sql.Append(" AND AE_RESER.RES_ESTA = 'C'");
                sql.Append(" AND AE_RESER.RES_TIPO = 'I'");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" AND AE_ESPTE.TER_CODI = @P_TER_CODI ");
                }
                sql.Append(" AND AE_DESPA.PRO_CONT = @P_PRO_CONT ");
                sql.Append(" UNION");
                sql.Append(" SELECT AE_ESPAC.ESP_CONT, AE_ESPAC.ESP_CODI, AE_ESPAC.ESP_NOMB, AE_ESPAC.ESP_ESTA,");
                sql.Append(" NULL, AE_MANTE.MAN_FINI, AE_MANTE.MAN_FFIN, 'M' RES_ESTA,");
                sql.Append(" 0 RES_CONT, 0 RES_EVEN, 2 RES_OPCI, 'U' RES_TIPO,");
                sql.Append(" 'Mantenimiento' RES_DESC");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" FROM AE_ESPAC, AE_MANTE,AE_CLASE,AE_ESPTE,AE_DESPA");
                }
                else
                {
                    sql.Append(" FROM AE_ESPAC, AE_MANTE,AE_CLASE,AE_DESPA");
                }
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_MANTE.EMP_CODI");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_MANTE.ESP_CONT");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_CLASE.EMP_CODI");
                sql.Append(" AND AE_ESPAC.CLA_CONT = AE_CLASE.CLA_CONT");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" AND AE_ESPAC.EMP_CODI = AE_ESPTE.EMP_CODI ");
                    sql.Append(" AND AE_ESPAC.ESP_CONT = AE_ESPTE.ESP_CONT ");
                }
                sql.Append(" AND AE_ESPAC.EMP_CODI = @P_EMP_CODI");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI ");
                //sql.Append(" AND AE_ESPAC.ESP_CONT = @P_ESP_CONT");
                sql.Append(" AND YEAR(AE_MANTE.MAN_FINI) = @P_YEAR");
                sql.Append(" AND MONTH(AE_MANTE.MAN_FINI) = @P_MONTH");
                sql.Append(" AND DAY(AE_MANTE.MAN_FINI) = @P_DAY");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" AND AE_ESPTE.TER_CODI = @P_TER_CODI ");
                }
                sql.Append(" AND AE_DESPA.PRO_CONT = @P_PRO_CONT ");
                sql.Append(" ORDER BY 1,6");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_CLA_CONT", Cla_cont));
                listAux.Add(new Parameter("@P_YEAR", year));
                listAux.Add(new Parameter("@P_MONTH", month));
                listAux.Add(new Parameter("@P_DAY", day));
                //listAux.Add(new Parameter("@P_ESP_CONT", esp_cont));
                listAux.Add(new Parameter("@P_PRO_CONT", pro_cont));
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    listAux.Add(new Parameter("@P_TER_CODI", Ter_codi));
                }
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

        public List<TONoDisponible> GetNoDisponi(short emp_codi, int Cla_cont, int year)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(" SELECT AE_DHORA.DHO_FECH");
                sql.Append(" FROM AE_HORAR, AE_DHORA, AE_CLASE, AE_ESPAC");
                sql.Append(" WHERE AE_HORAR.EMP_CODI = AE_DHORA.EMP_CODI");
                sql.Append(" AND AE_HORAR.HOR_CONT = AE_DHORA.HOR_CONT");
                sql.Append(" AND AE_HORAR.EMP_CODI = AE_ESPAC.EMP_CODI");
                sql.Append(" AND AE_HORAR.ESP_CONT = AE_ESPAC.ESP_CONT");
                sql.Append(" AND AE_CLASE.EMP_CODI = AE_ESPAC.EMP_CODI");
                sql.Append(" AND AE_CLASE.CLA_CONT = AE_ESPAC.CLA_CONT");
                sql.Append(" AND AE_HORAR.HOR_ESTA <> 'A'");
                sql.Append(" AND YEAR(DHO_FECH) = @P_YEAR");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT");


                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_CLA_CONT", Cla_cont));
                listAux.Add(new Parameter("@P_YEAR", year));

                Parameter[] oParameter = listAux.ToArray();

                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.ReadList(pTOContext, sql.ToString(), MakeNoDisp, oParameter);
                return objeto;
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }
        public Func<IDataReader, TONoDisponible> MakeNoDisp = reader => new TONoDisponible
        {
            FechasNoDisponible = reader["DHO_FECH"].AsString(),
        };

        public int CantidadHorarios(short emp_codi, int Cla_cont, int year, int month, int day, float ter_codi, int pro_cont, string esp_mdit, string Op_Disp)
        {
            List<Parameter> listAux = new List<Parameter>();

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append(" SELECT SUM(CANT)CANT FROM(SELECT COUNT(DISTINCT AE_ESPAC.ESP_CONT)CANT ");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" FROM AE_ESPAC, AE_RESER, AE_CLASE,AE_DESPA,AE_ESPTE ");
                }
                else
                {
                    sql.Append(" FROM AE_ESPAC, AE_RESER, AE_CLASE ");
                }
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_RESER.EMP_CODI ");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_RESER.ESP_CONT ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_CLASE.EMP_CODI ");
                sql.Append(" AND AE_ESPAC.CLA_CONT = AE_CLASE.CLA_CONT ");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" AND AE_ESPAC.ESP_CONT = AE_ESPTE.ESP_CONT ");
                    sql.Append(" AND AE_ESPAC.EMP_CODI = AE_ESPTE.EMP_CODI ");
                    sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT ");
                    sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI ");
                    sql.Append(" AND AE_DESPA.PRO_CONT = @P_PRO_CONT ");
                }
                sql.Append(" AND AE_ESPAC.EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT ");

                sql.Append(" AND YEAR(AE_RESER.RES_FINI) = @YEAR ");
                sql.Append(" AND MONTH(AE_RESER.RES_FINI) = @MONTH ");
                sql.Append(" AND DAY(AE_RESER.RES_FINI) = @DAY ");
                sql.Append(" AND AE_RESER.RES_ESTA IN('R', 'C', 'U', 'E') ");
                sql.Append(" AND AE_RESER.RES_TIPO IN('U', 'I') ");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" AND AE_ESPTE.TER_CODI = @P_TER_CODI ");
                }
                sql.Append(" AND AE_ESPAC.ESP_CONT NOT IN(SELECT AE_ESPAC.ESP_CONT ");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" FROM AE_ESPAC, AE_MANTE, AE_CLASE ,AE_DESPA,AE_ESPTE ");
                }
                else
                {
                    sql.Append(" FROM AE_ESPAC, AE_MANTE, AE_CLASE ,AE_DESPA,AE_ESPTE ");
                }
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_MANTE.EMP_CODI ");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_MANTE.ESP_CONT ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_CLASE.EMP_CODI ");
                sql.Append(" AND AE_ESPAC.CLA_CONT = AE_CLASE.CLA_CONT ");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" AND AE_ESPAC.ESP_CONT = AE_ESPTE.ESP_CONT ");
                    sql.Append(" AND AE_ESPAC.EMP_CODI = AE_ESPTE.EMP_CODI ");
                    sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT ");
                    sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI ");
                }
                sql.Append(" AND AE_ESPAC.EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT ");
                sql.Append(" AND YEAR(AE_MANTE.MAN_FINI) = @YEAR ");
                sql.Append(" AND MONTH(AE_MANTE.MAN_FINI) = @MONTH ");
                sql.Append(" AND DAY(AE_MANTE.MAN_FINI) = @DAY ");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append("         AND AE_ESPTE.TER_CODI = @P_TER_CODI ");
                }
                sql.Append(")");
                sql.Append(" UNION ALL ");
                sql.Append(" SELECT COUNT(DISTINCT AE_ESPAC.ESP_CONT)CANT ");
                sql.Append(" FROM AE_ESPAC, AE_MANTE, AE_CLASE,AE_DESPA,AE_ESPTE ");
                sql.Append(" WHERE AE_ESPAC.EMP_CODI = AE_MANTE.EMP_CODI ");
                sql.Append(" AND AE_ESPAC.ESP_CONT = AE_MANTE.ESP_CONT ");
                sql.Append(" AND AE_ESPAC.EMP_CODI = AE_CLASE.EMP_CODI ");
                sql.Append(" AND AE_ESPAC.CLA_CONT = AE_CLASE.CLA_CONT ");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" AND AE_ESPAC.ESP_CONT = AE_ESPTE.ESP_CONT ");
                    sql.Append(" AND AE_ESPAC.EMP_CODI = AE_ESPTE.EMP_CODI ");
                    sql.Append(" AND AE_ESPAC.ESP_CONT = AE_DESPA.ESP_CONT ");
                    sql.Append(" AND AE_ESPAC.EMP_CODI = AE_DESPA.EMP_CODI ");
                }
                sql.Append(" AND AE_ESPAC.EMP_CODI = @P_EMP_CODI ");
                sql.Append(" AND AE_CLASE.CLA_CONT = @P_CLA_CONT ");
                sql.Append(" AND YEAR(AE_MANTE.MAN_FINI) = @YEAR ");
                sql.Append(" AND MONTH(AE_MANTE.MAN_FINI) = @MONTH ");
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    sql.Append(" AND AE_ESPTE.TER_CODI = @P_TER_CODI ");
                }
                sql.Append(" AND DAY(AE_MANTE.MAN_FINI) = @DAY) D                                                       ");

                listAux.Add(new Parameter("@P_EMP_CODI", emp_codi));
                listAux.Add(new Parameter("@P_CLA_CONT", Cla_cont));
                listAux.Add(new Parameter("@YEAR", year));
                listAux.Add(new Parameter("@MONTH", month));
                listAux.Add(new Parameter("@DAY", day));
                listAux.Add(new Parameter("@P_PRO_CONT", pro_cont));
                if (Op_Disp != "F" && esp_mdit == "S")
                {
                    listAux.Add(new Parameter("@P_TER_CODI", ter_codi));
                }

                Parameter[] oParameter = listAux.ToArray();
                OTOContext pTOContext = new OTOContext();

                var conection = DBFactory.GetDB(pTOContext);
                var objeto = conection.GetScalar(pTOContext, sql.ToString(), oParameter);
                return objeto.AsInt();
            }
            catch (Exception ex)
            {
                BOException.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return 0;
            }
        }

    }
}