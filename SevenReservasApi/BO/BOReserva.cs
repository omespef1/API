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
    public class BOReserva
    {
        IReserva comReserva = new ComReservas.ComReserva();
        DAOReserva daoReserva = new DAOReserva();
        DAOAgenda daoAgenda = new DAOAgenda();
        DAOAeClase daoClase = new DAOAeClase();
        BO.BOAgenda boAgenda = new BOAgenda();


        public TOTransaction CrearReserva(TOAeReser reserva)
        {
            try
            {
                /*revisamos si hay plazo disponible para poder generar la reserva con el campo CLA_TICA de la clase*/
                TOAeClase clase = daoClase.GetAeClaseByClaCont(reserva.Emp_codi, reserva.Cla_cont);
                if (clase != null && clase.Cla_Fchr != null && reserva.Res_fini.Date > clase.Cla_Fchr.Value.Date)
                {
                    throw new Exception("FECHA LÍMITE RESERVA: " + clase.Cla_Fchr.Value.ToString("yyyy-MM-dd"));
                }
                var ite_cont = ConfigurationManager.AppSettings["MedioReserva"].ToString();
                if (ite_cont != null)
                    reserva.Ite_cont = int.Parse(ite_cont);
               
                //consultar si el tercero ya tiene reserva a esa misma hora
                if (reserva.Esp_mdit == "S")
                {
                    int cantidadReserva = daoAgenda.GetReservaTercero(reserva.Emp_codi, (int)reserva.Ter_codi, reserva.Productos[0].Pro_cont, reserva.Res_fini, reserva.Res_fina);
                    if (cantidadReserva != 0)
                    {
                        throw new Exception("El tercero tiene una reserva activa para esta hora");
                    }
                }
                int CantEspacios = daoAgenda.CantidadHorarios(reserva.Emp_codi, reserva.Cla_cont, reserva.Res_fini.Year, reserva.Res_fini.Month, reserva.Res_fini.Day,
                    (float)reserva.Ter_codi, reserva.Productos[0].Pro_cont, reserva.Esp_mdit, "");
                List<TOOcupado> intervalosNoDisponiblesOcupados = daoAgenda.GetOcupadosDipo(reserva.Emp_codi, reserva.Cla_cont, reserva.Res_fini.Year, reserva.Res_fini.Month, reserva.Res_fini.Day,
                    reserva.Esp_mdit, -1, reserva.Productos[0].Pro_cont, "", (float)reserva.Ter_codi);

                DTOEntities.TSalida salida = boAgenda.ValidarDisponibilidadHorario(reserva.Res_fini, reserva.Res_fina, reserva.Cla_cont, (float)reserva.Ter_codi,
                    reserva.Productos[0].Pro_cont, reserva.Esp_mdit, "", CantEspacios, intervalosNoDisponiblesOcupados);
                if (salida.retorno == 0)
                {
                    throw new Exception(salida.Txterror);
                }

                if (reserva.Esp_cont == 0)
                {
                    int esp_cont = 0;
                    if (reserva.Esp_mdit == "S")
                    {
                        //revisamos los espacios que tiene disponible el tercero y verificamos cuales tienen disponibilidad (disponiblilidad de rango)
                        //no incluye las reservas
                        //daoAgenda.GetEspSinReservaTercero(reserva.Emp_codi, reserva.Res_fini.Year, reserva.Res_fini.Month, reserva.Res_fini.Day, reserva.Cla_cont, reserva.Productos[0].Pro_cont, out esp_cont, reserva.Res_fini, reserva.Res_fina, reserva.Esp_mdit, reserva.Ter_codi);
                        //esp_cont = daoAgenda.GetEspSinReservaTercero(reserva.Emp_codi, reserva.Res_fini.Year, reserva.Res_fini.Month, reserva.Res_fini.Day, reserva.Cla_cont, reserva.Productos[0].Pro_cont, reserva.Res_fini, reserva.Res_fina, (int)reserva.Ter_codi);
                    }
                    else
                    {
                        esp_cont = daoAgenda.GetEspSinReserva(reserva.Emp_codi, reserva.Res_fini.Year, reserva.Res_fini.Month, reserva.Res_fini.Day, reserva.Cla_cont, reserva.Productos[0].Pro_cont, reserva.Res_fini, reserva.Res_fina);
                        if (esp_cont == 0)
                        {
                            throw new Exception ("Este horario ya no se encuentra disponible");
                        }
                    }
                    reserva.Esp_cont = esp_cont;
                }

                return comReserva.CrearReserva(reserva);
            }
            catch (Exception ex)
            {
                return new TOTransaction() { InvoiceId = 0, Retorno = 1, TxtError = ex.Message };
            }
        }


        public Models.TOTransaction<List<TOInfReser>> GetInfoReserva(int soc_cont, int sbe_cont,short emp_codi)
        {
            try
            {
                DAOGnTerce daoTerce =new  DAOGnTerce();
                DateTime ahora = DateTime.Now;
                List<TOInfReser> reservas = new List<TOInfReser>(daoReserva.GetInfoReservas(emp_codi, soc_cont, sbe_cont));             
                foreach (var item in reservas)
                {
                    if (item.FechaInicio < ahora)
                        item.Res_vige = "N";
                    else
                        item.Res_vige = "S";
                    var tercero = daoTerce.GetGnTerce(emp_codi, item.Ter_codi);
                    item.Ter_foto = string.IsNullOrEmpty(tercero.Ter_foto.ToString()) ? null : tercero.Ter_foto;
                    item.Ter_noco = tercero.Ter_noco;

                }
                return new Models.TOTransaction<List<TOInfReser>>() { ObjTransaction = reservas, Retorno = 0, TxtError = "" };
            }
            catch (Exception ex)
            {
                return new Models.TOTransaction<List<TOInfReser>>() { ObjTransaction = null, Retorno = 1, TxtError = ex.Message };
            }
           
            
        }

        public DTOEntities.TOTransaction CancelarReserva(int id, int motivo,short emp_codi)
        {
            return comReserva.CancelarReserva(emp_codi, id, motivo);
        }
    }

}