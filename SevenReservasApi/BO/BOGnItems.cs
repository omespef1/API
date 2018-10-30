using SevenReservas.DAO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.BO
{
    public class BOGnItems
    {
        DAOGnItems daoGnItems = new DAOGnItems();
        public TOTransaction< List<TOGnItems>> GetGnItems (int tit_cont)
        {
            try
            {
                List<TOGnItems> litems = new List<TOGnItems>(daoGnItems.GetGnItems(tit_cont));
                //TOGnItems item = new TOGnItems();
                // item.Ite_nomb = "Seleccione";
                // litems.Add(item);
                if (litems != null && !litems.Any())
                    throw new Exception("No se encontraron items para cancelación de reserva");
                return new TOTransaction<List<TOGnItems>>() { ObjTransaction = litems, TxtError = "", Retorno = 0 };
            }
            catch(Exception ex)
            {
                return new TOTransaction<List<TOGnItems>>() { ObjTransaction = null, Retorno = 1, TxtError = ex.Message };
            }
           
            //litems.AddRange();

            //return litems;
        }
    }
}