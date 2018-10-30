using BackEndReservas;
using DTOEntities;
using SevenReservas.DAO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SevenReservas.BO
{
    public class BOConsumos
    {
        DAOConsumos daoConsumos = new DAOConsumos();
      

        public TOTransaction<List<TOConsumos>> GetConsumos(int soc_cont, int sbe_cont, int fac_mesp,int fac_anop,short emp_codi)
        {
            List<TOConsumos> lConsumos = new List<TOConsumos>();
            try
            {
                var consumos = daoConsumos.GetConsumos(emp_codi, soc_cont, sbe_cont, fac_mesp, fac_anop);

                DateTime con_fech = DateTime.MinValue;
                /*
               // if (consumos.Count > 0)
               //     reserva = consumos[0].Res_nume;
               */
                foreach (var item in consumos)
                {
                    if (item.Con_fech != con_fech)
                    {
                        TOConsumos to = new TOConsumos();
                        to.lConsumos.AddRange(consumos.Where(x => x.Con_fech == item.Con_fech));
                        to.Con_fech = item.Con_fech;
                        to.Dvt_valo = item.Dvt_valo;
                        lConsumos.Add(to);
                    }
                    con_fech = item.Con_fech;
                }
                /*            foreach (var item in consumos)
                            {
                                if (item.Res_nume != reserva)
                                {
                                    TOConsumos to = new TOConsumos();
                                    to.lConsumos.AddRange(consumos.Where(x => x.Res_nume == item.Res_nume));
                                    to.Res_nume = item.Res_nume;

                                    lConsumos.Add(to);
                                }

                                reserva = item.Res_nume;
                            }
                            */


                //lConsumos = consumos;

                return new TOTransaction<List<TOConsumos>>() { ObjTransaction = lConsumos, TxtError = "", Retorno = 0 };
            }
            catch(Exception ex)
            {
                return new TOTransaction<List<TOConsumos>>() { ObjTransaction = null, Retorno = 1, TxtError = ex.Message };
            }
          
        }
    }
}