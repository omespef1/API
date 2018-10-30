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
    public class BOEcEvent
    {
        DAOEcEvent daoEcEvent = new DAOEcEvent();
        short emp_codi = short.Parse(ConfigurationManager.AppSettings["Emp_codi"]);

        public IEnumerable<TOEcEvent> GetEcEvent(int soc_cont, int sbe_cont)
        {
            var datos = daoEcEvent.GetEcEvent(emp_codi, soc_cont, sbe_cont);
            
            foreach (var item in datos)
            {
                switch (item.Eve_esta)
                {
                    case "C": item.Eve_esta = "Confirmado"; break;
                    case "P": item.Eve_esta = "Pendiente"; break;

                    default: item.Eve_esta = "No definido";
                        break;
                }

                item.desplegar = "C";
            }
            return datos;
        }

    }
}