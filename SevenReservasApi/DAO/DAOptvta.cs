using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SevenFramework.DataBase;
using SevenFramework.DataBase.Utils;
using SevenFramework.Exceptions;
using SevenFramework.Helpers;

namespace SevenReservas.DAO
{
    public class DAOptvta
    {
        internal IEnumerable<Models.PTVTA> GetPtvta(short emp_codi)
        {
            try
            {
                List<SQLParams> sqlPrms = new List<SQLParams>()
                {
                    new SQLParams("emp_codi", emp_codi),
                    new SQLParams("ptv_esta", "A"),
                };
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT * FROM sp_ptvta WHERE emp_codi = @emp_codi AND ptv_esta = @ptv_esta AND ptv_nmef > 0");
                //string sql = "SELECT * FROM sp_ptvta WHERE emp_codi = @emp_codi AND ptv_esta = @ptv_esta AND ptv_nmef > 0"; //DBHelper.SelectQueryString<Models.PTVTA>(sqlPrms);

                //sql.AppendLine(" SELECT pt.* FROM sp_ptvta pt                                                      ");
                //sql.AppendLine(" INNER JOIN PV_MESSL me ON pt.emp_codi = me.EMP_CODI AND pt.ptv_cont = me.BOD_CONT ");
                //sql.AppendLine(" WHERE pt.emp_codi = @emp_codi AND pt.ptv_esta = @ptv_esta AND pt.ptv_nmef > 0     ");

                IEnumerable<Models.PTVTA> data = new DbConnection().GetList<Models.PTVTA>(sql.ToString(), sqlPrms);
                //consultamos las mesas ocupadas
                if (data != null && data.Any())
                {
                    foreach (Models.PTVTA ptv in data)
                    {
                        //buscamos que tenga turno abierto
                        sqlPrms.Clear();

                        sqlPrms.Add(new SQLParams("emp_codi", emp_codi));
                        sqlPrms.Add(new SQLParams("ptv_cont", ptv.ptv_cont));
                        sqlPrms.Add(new SQLParams("tur_esta", "A"));

                        sql.Clear();

                        sql.AppendLine(" SELECT sp_ptvta.ptv_cont FROM sp_turno,sp_tepos,sp_ptvta                                        ");
                        sql.AppendLine(" WHERE sp_ptvta.emp_codi = sp_tepos.emp_codi                                                     ");
                        sql.AppendLine(" and sp_ptvta.ptv_cont = sp_tepos.ptv_cont                                                       ");
                        sql.AppendLine(" and sp_turno.emp_codi = sp_tepos.emp_codi                                                       ");
                        sql.AppendLine(" and sp_turno.tep_cont = sp_tepos.tep_cont                                                       ");
                        sql.AppendLine(" and sp_ptvta.emp_codi = @emp_codi and sp_ptvta.ptv_cont = @ptv_cont AND sp_turno.tur_esta = @tur_esta ");

                        IEnumerable<Models.sp_turno> sp_turno = new DbConnection().GetList<Models.sp_turno>(sql.ToString(), sqlPrms);
                        if (sp_turno == null || !sp_turno.Any())
                        {
                            ptv.numMesas = 0;
                            continue;
                        }

                        //mesas parametrizadas
                        sqlPrms.Clear();
                        sqlPrms.Add(new SQLParams("emp_codi", emp_codi));
                        sqlPrms.Add(new SQLParams("ptv_cont", ptv.ptv_cont));
                        sql.Clear();

                        sql.AppendLine(" SELECT * FROM PV_MESSL ME                              ");
                        sql.AppendLine(" INNER JOIN GN_ITEMS AS IT ON IT.ITE_CONT = ME.ITE_CONT ");
                        sql.AppendLine(" WHERE EMP_CODI = @emp_codi AND BOD_CONT = @ptv_cont    ");


                        List<Models.pv_messl> pv_messl = new DbConnection().GetList<Models.pv_messl>(sql.ToString(), sqlPrms);
                        if (pv_messl == null || !pv_messl.Any())
                        {
                            //si no se encuentran mesas parametrizadas entonces no se debe mostrar el punto de venta
                            ptv.numMesas = 0;
                            continue;
                        }

                        //buscamos las mesas no disponibles parametrizadas en el web.config
                        string ItemsMesasNoDisponibles = System.Configuration.ConfigurationManager
                            .AppSettings.Get("ItemsMesasNoDisponibles");

                        if (ItemsMesasNoDisponibles != null && ItemsMesasNoDisponibles.Trim().Length > 0)
                        {
                            string[] ItemsNoDisponibles = ItemsMesasNoDisponibles.Split(';')
                                .Select(o => o.Trim()).ToArray();
                            foreach (string mesa in ItemsNoDisponibles)
                            {
                                pv_messl.RemoveAll(o => o.ITE_CODI.Equals(mesa));
                            }

                            if (pv_messl == null || !pv_messl.Any())
                            {
                                ptv.numMesas = 0;
                                continue;
                            }
                        }

                        sqlPrms.Clear();
                        //Mesas Ocupadas
                        sqlPrms.Add(new SQLParams("emp_codi", emp_codi));
                        sqlPrms.Add(new SQLParams("ptv_cont", ptv.ptv_cont));
                        sql.Clear();

                        sql.AppendLine(" SELECT DISTINCT sp_mesas.mes_nume FROM sp_turno,sp_tepos,sp_ptvta,sp_mesas ");
                        sql.AppendLine(" WHERE sp_ptvta.emp_codi = sp_tepos.emp_codi                                ");
                        sql.AppendLine(" and sp_ptvta.ptv_cont = sp_tepos.ptv_cont                                  ");
                        sql.AppendLine(" and sp_turno.emp_codi = sp_tepos.emp_codi                                  ");
                        sql.AppendLine(" and sp_turno.tep_cont = sp_tepos.tep_cont                                  ");
                        sql.AppendLine(" and sp_turno.emp_codi = sp_mesas.emp_codi                                  ");
                        sql.AppendLine(" and sp_turno.tur_cont = sp_mesas.tur_cont                                  ");
                        sql.AppendLine(" and sp_ptvta.emp_codi = @emp_codi and sp_ptvta.ptv_cont = @ptv_cont        ");

                        IEnumerable<Models.sp_mesas> sp_mesas = new DbConnection().GetList<Models.sp_mesas>(sql.ToString(), sqlPrms);
                        if (sp_mesas != null && sp_mesas.Any())
                        {
                            //if (ptv.ptv_nmef > 0)
                            //    ptv.numMesas = Math.Abs(ptv.ptv_nmef - ptv.ptv_nmei) + 1 - sp_mesas.Count();
                            //else
                            //    ptv.numMesas = Math.Abs(ptv.ptv_nmef - ptv.ptv_nmei) + 1;
                            ptv.numMesas = pv_messl.Count - sp_mesas.Count();
                        }
                        else
                        {
                            //if (ptv.ptv_nmef > 0)
                            //    ptv.numMesas = Math.Abs(ptv.ptv_nmef - ptv.ptv_nmei) + 1;
                            //else
                            //    ptv.numMesas = 0;
                            ptv.numMesas = pv_messl.Count;
                        }
                        ptv.totaMesas = pv_messl.Count;
                    }


                    //foreach (Models.PTVTA ptv in data)
                    //{
                    //    sqlPrms.Clear();
                    //    sqlPrms.Add(new SQLParams("emp_codi", emp_codi));
                    //    sqlPrms.Add(new SQLParams("ptv_cont", ptv.ptv_cont));

                    //    StringBuilder sqlx = new StringBuilder();

                    //    sqlx.AppendLine(" SELECT DISTINCT sp_mesas.mes_nume FROM sp_turno,sp_tepos,sp_ptvta,sp_mesas ");
                    //    sqlx.AppendLine(" WHERE sp_ptvta.emp_codi = sp_tepos.emp_codi                                ");
                    //    sqlx.AppendLine(" and sp_ptvta.ptv_cont = sp_tepos.ptv_cont                                  ");
                    //    sqlx.AppendLine(" and sp_turno.emp_codi = sp_tepos.emp_codi                                  ");
                    //    sqlx.AppendLine(" and sp_turno.tep_cont = sp_tepos.tep_cont                                  ");
                    //    sqlx.AppendLine(" and sp_turno.emp_codi = sp_mesas.emp_codi                                  ");
                    //    sqlx.AppendLine(" and sp_turno.tur_cont = sp_mesas.tur_cont                                  ");
                    //    sqlx.AppendLine(" and sp_ptvta.emp_codi = @emp_codi and sp_ptvta.ptv_cont = @ptv_cont        ");

                    //    IEnumerable<Models.sp_mesas> sp_mesas = new DbConnection().GetList<Models.sp_mesas>(sqlx.ToString(), sqlPrms);
                    //    if (sp_mesas != null && sp_mesas.Any())
                    //    {
                    //        if (ptv.ptv_nmef > 0)
                    //            ptv.numMesas = Math.Abs(ptv.ptv_nmef - ptv.ptv_nmei) + 1 - sp_mesas.Count();
                    //        else
                    //            ptv.numMesas = Math.Abs(ptv.ptv_nmef - ptv.ptv_nmei) + 1;
                    //    }
                    //    else
                    //    {
                    //        if (ptv.ptv_nmef > 0)
                    //            ptv.numMesas = Math.Abs(ptv.ptv_nmef - ptv.ptv_nmei) + 1;
                    //        else
                    //            ptv.numMesas = 0;
                    //    }
                    //    ptv.totaMesas = Math.Abs(ptv.ptv_nmef - ptv.ptv_nmei) + 1;
                    //}
                }



                return data == null ? null : data.Where(o => o.numMesas > 0).ToList();
            }
            catch (Exception ex)
            {
                ExceptionManager.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }

        internal IEnumerable<Models.Mesa> GetMesas(short emp_codi, long ptv_cont, long ptv_nmei, long ptv_nmef)
        {
            try
            {
                List<SQLParams> sqlPrms = new List<SQLParams>()
                {
                    new SQLParams("emp_codi", emp_codi),
                    new SQLParams("pvt_cont", ptv_cont),
                };

                StringBuilder sql = new StringBuilder();
                sql.AppendLine(" SELECT DISTINCT sp_mesas.mes_nume FROM sp_turno,sp_tepos,sp_ptvta,sp_mesas ");
                sql.AppendLine(" WHERE sp_ptvta.emp_codi = sp_tepos.emp_codi                                ");
                sql.AppendLine(" AND sp_ptvta.ptv_cont = sp_tepos.ptv_cont                                  ");
                sql.AppendLine(" AND sp_turno.emp_codi = sp_tepos.emp_codi                                  ");
                sql.AppendLine(" AND sp_turno.tep_CONT = sp_tepos.tep_cont                                  ");
                sql.AppendLine(" AND sp_turno.emp_codi = sp_mesas.emp_codi                                  ");
                sql.AppendLine(" AND sp_turno.tur_cont = sp_mesas.tur_cont                                  ");
                sql.AppendLine(" AND sp_ptvta.emp_codi = @emp_codi AND sp_ptvta.ptv_cont = @pvt_cont        ");

                IEnumerable<Models.sp_mesas> data = new DbConnection().GetList<Models.sp_mesas>(sql.ToString(), sqlPrms);
                //consultamos las mesas ocupadas
                List<Models.Mesa> mesas = new List<Models.Mesa>();
                if (ptv_nmef > 0)
                {
                    for (long i = ptv_nmei; i <= ptv_nmef; i++)
                    {
                        if (data == null || !data.Any(o => o.mes_nume == i))
                            mesas.Add(new Models.Mesa()
                            {
                                mes_nume = i
                            });
                    }
                }
                return mesas;
            }
            catch (Exception ex)
            {
                ExceptionManager.Throw(this.GetType().ToString(), System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                return null;
            }
        }
    }
}