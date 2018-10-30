using SevenReservas.DAO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenReservas.BO
{
    public class BOGnTerce
    {
        DAOGnTerce daoGnTerce = new DAOGnTerce();
        DAOAgenda daoAgenda = new DAOAgenda();

        short emp_codi = short.Parse(ConfigurationManager.AppSettings["Emp_codi"]);

        public List<TOGnTerce> GetImageTerce(List<string> ter_codes)
        {
            return daoGnTerce.GetImageTerce(ter_codes);
        }

        public List<TOGnTerce> GetGnTerce(int cla_cont, int pro_cont)
        {
            //  return daoGnTerce.GetGnTerce(emp_codi, cla_cont, pro_cont);
            var listaTerceros = daoGnTerce.GetGnTerce(emp_codi, cla_cont, pro_cont);
            List<TOGnTerce> l = new List<TOGnTerce>();
            foreach (var item in listaTerceros)
            {
                item.Disponible = true;
                l.Add(item);
            }
            return l;
        }

        public TOTransaction<List<TOGnTerce>> GetGnTerceNewVersion(int cla_cont, int pro_cont, DateTime? Fini, DateTime? Ffin, string Op_Disp)
        {
            try
            {
                 List<TOGnTerce> listaTercerosHorario = new List<TOGnTerce>( daoGnTerce.GetGnTerceNewVersion(emp_codi, cla_cont, pro_cont, Fini, Ffin, Op_Disp));
                var  listaTercerosProducto =  daoGnTerce.GetGnTerce(emp_codi, cla_cont, pro_cont);
                if (listaTercerosHorario == null || !listaTercerosHorario.Any())
                    throw new Exception("No se encontraron terceros.");
                foreach(TOGnTerce tercero in listaTercerosHorario)
                {
                    tercero.Ter_foto = listaTercerosProducto.Where(t => t.Ter_codi == tercero.Ter_codi).FirstOrDefault().Ter_foto;
                }
                return new TOTransaction<List<TOGnTerce>>() { ObjTransaction = listaTercerosHorario, Retorno = 0, TxtError = "" };
                //List<TOGnTerce> l = new List<TOGnTerce>();
                //foreach (var item in listaTerceros)
                //{
                //    item.Disponible = true;
                //    l.Add(item);
                //}
                //return l;
            }
            catch(Exception ex)
            {
                return new TOTransaction<List<TOGnTerce>>() { ObjTransaction = null, TxtError = ex.Message, Retorno = 1 };
            }
          
        }
    }

}