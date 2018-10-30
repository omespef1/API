using SevenReservas.DAO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SevenReservas.BO
{
    public class BOProducto
    {
        DAOProducto daoProducto = new DAOProducto();

        DAOProducto daoProductod = new DAOProducto();
       

        public TOTransaction<List<TOProducto>> GetProductos(int Cla_cont,short emp_codi)
        {
            try
            {
                List<TOProducto> result =  new List<TOProducto>(daoProducto.GetProductos(emp_codi, Cla_cont));
                if (result == null || !result.Any())
                    throw new Exception("No se encontraron productos");
                return new TOTransaction<List<TOProducto>>() { ObjTransaction = result, Retorno = 0, TxtError = "" };
            }
            catch(Exception ex)
            {
                return new TOTransaction<List<TOProducto>>() { ObjTransaction = null, Retorno = 1, TxtError = ex.Message };
            }
           
        }
    }

}