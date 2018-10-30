
using SevenReservas.BO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace SevenReservas.Controllers
{
    public class AgendaController : ApiController
    {
        BOAgenda boAgenda = new BOAgenda();

       /* public IEnumerable<TODisponible> Get(int esp_cont, int pro_cont, int year, int month, int day)
        {
            return boAgenda.GetDisponibles(esp_cont, pro_cont, year, month, day);
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cla_cont"></param>
        /// <param name="pro_cont"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="esp_mdit"></param>
        /// <param name="ter_codi"></param>
        /// <param name="Op_Disp">Opción de disponibilidad: P - por profesional, F - por fecha</param>
        /// <returns></returns>
        //public IEnumerable<TODisponible> Get(
        //    int Cla_cont, 
        //    int pro_cont, 
        //    int year, 
        //    int month, 
        //    int day, 
        //    string esp_mdit, 
        //    float ter_codi,
        //    string Op_Disp = ""
        //    )
        //{
        //    return boAgenda.GetDisponiblesParametroDisp(Cla_cont, pro_cont, year, month, day, esp_mdit, ter_codi, Op_Disp);
        //}

        public TOTransaction<List<TODisponible>> Get(int Cla_cont,
            int pro_cont,
            int year,
            int month,
            string esp_mdit,
            float ter_codi,
            string Op_Disp = "")
        {
            return boAgenda.GetDisponiblesParametroViewMonth(Cla_cont, pro_cont, year, month, esp_mdit, ter_codi, Op_Disp);
        }


     /*   public IEnumerable<TONoDisponible> GetNoDisponibles(int Cla_cont, int year)
        {
            return boAgenda.GetNoDisponiblesParametroDisp(Cla_cont, year);
        }*/
        //     // PUT: api/Usuarios/5
        //     [HttpPut]
        //     public void Put(int id, TOSoSocio value)
        //     {
        //     }
        //
        //     // DELETE: api/Usuarios/5
        //     public void Delete(int id)
        //     {
        //     }
    }
}
