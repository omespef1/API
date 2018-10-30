
using SevenReservas.DAO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SevenReservas.BO
{
    public class BOAgenda
    {
        DAOAgenda daoAgenda = new DAOAgenda();
        DAOProducto daoProducto = new DAOProducto();
        short emp_codi = short.Parse(ConfigurationManager.AppSettings["Emp_codi"]);

        public IEnumerable<TODisponible> GetDisponibles(int esp_cont, int pro_cont, int year, int month, int day)
        {
            var ocupados = daoAgenda.GetOcupados(emp_codi, esp_cont, year, month, day);
            DateTime fechaMin = new DateTime();
            DateTime fechaMax = new DateTime();
            DateTime ahora = DateTime.Now;
            daoAgenda.GetFechaMinMax(emp_codi, year, month, day, esp_cont, out fechaMin, out fechaMax);
            var producto = daoProducto.GetProducto(emp_codi, pro_cont);

            if (fechaMin == DateTime.MinValue)
            {
                fechaMin = new DateTime(year, month, day);
                //fechaMax = new DateTime(year, month, day, 23, 59, 59);
                fechaMax = new DateTime(year, month, day);

            }

            List<TODisponible> listItems = new List<TODisponible>();
            for (DateTime i = fechaMin; i < fechaMax && producto != null && producto.Pro_dmin > 0; i = i.AddMinutes(producto.Pro_dmin))
            {
                TODisponible item = new TODisponible();
                item.FechaInicio = i;
                item.FechaFin = i.AddMinutes(producto.Pro_dmin);

                if (item.FechaInicio > ahora)
                    listItems.Add(item);
            }

            //cruzar lista horario del espacio con las reservas de este
            //return (start1 < end2 && start2 < end1) ||
            //(start1 == start2 && (start1 == end1 || start2 == end2));
            foreach (var itemHorario in listItems)
            {

                foreach (var itemOcupado in ocupados)
                {
                    if ((itemHorario.FechaInicio < itemOcupado.Res_fina && itemOcupado.Res_fini <= itemHorario.FechaFin)
                       || (itemHorario.FechaInicio == itemOcupado.Res_fini && itemHorario.FechaFin == itemOcupado.Res_fina))
                    {
                        itemHorario.Estado = 1;
                        break;
                    }
                    else
                        itemHorario.Estado = 0;

                }
            }
            var ret = listItems.Where(x => x.Estado == 0);
            return ret;
        }


        public DTOEntities.TSalida ValidarDisponibilidadHorario(DateTime fini, DateTime fina, int cla_cont,
            float ter_codi, int pro_cont, string esp_mdit, string Op_Disp, int CantEspacios, List<TOOcupado> intervalosNoDisponiblesOcupados)
        {
            //cantidad de espacios (si es tercero, cantidad de espacios asignados a ese tercero)
            //int CantEspacios = daoAgenda.CantidadHorarios(emp_codi, cla_cont, fini.Year, fini.Month, fini.Day, ter_codi, pro_cont, esp_mdit, Op_Disp);
            //listado de los intervalos que se encuentran no disponibles u ocupados
            DateTime ahora = DateTime.Now;
            //List<TOOcupado> intervalosNoDisponiblesOcupados = daoAgenda.GetOcupadosDipo(emp_codi, cla_cont, fini.Year, fini.Month, fini.Day, esp_mdit, -1, pro_cont, Op_Disp, ter_codi);
            if (fini <= ahora)
            {
                return new DTOEntities.TSalida("Este horario no se encuentra disponible", 0);
            }
            else
            {
                //evaluando hora del intervalo con respecto a los horarios no disponibles
                var intervalosNoDisponiblesFilter = intervalosNoDisponiblesOcupados.Where(x => fini < x.Res_fina && x.Res_fini < fina);
                if (intervalosNoDisponiblesFilter.Select(o => o.Esp_cont).Distinct().Count() >= CantEspacios)
                {
                    return new DTOEntities.TSalida("Este horario no se encuentra disponible", 0);
                }
                else
                {
                    if (Op_Disp == "F")
                    {
                        /*buscamos si para este horario existe un tercero que pertenezca al mismo espacio que tenga disponibilidad*/
                        //verificamos que el tercero no tenga reserva en este horario
                        //Cantidad de terceros reservados para horario enviado
                        int reservatercerosEspacio = daoAgenda.GetReservaTercerosEspacio(emp_codi, cla_cont, fini, fina);
                        //Cantidad de terceros configurados para el espacio
                        int cantidadTercerosEspacio = daoAgenda.GetCantidadTercerosEspacio(emp_codi, cla_cont);
                        if (reservatercerosEspacio == 0 || reservatercerosEspacio < cantidadTercerosEspacio)
                        {
                            //validar que el tercero tenga disponible este horario
                            List<TODisponible> horariosTercero = daoAgenda.GetHorariosTercero(fini.Year, fini.Month, fini.Day, cla_cont, pro_cont, (int)ter_codi, "D", Op_Disp);
                            if (horariosTercero == null || !horariosTercero.Any() || horariosTercero.Where(x => fini >= x.FechaInicio && fina <= x.FechaFin).Count() == 0)
                            {
                                return new DTOEntities.TSalida("No hay profesionales disponibles en este horario", 0);
                            }
                            else
                            {
                                //buscamos haber si el horario se encuentra dentro del rango de inactividad
                                List<TONoHorDisponible> horariosInactividad = daoAgenda.GetHorariosTerceroNoDisp(fini.Year, fini.Month, fini.Day, cla_cont, pro_cont, (int)ter_codi, "I", Op_Disp,fini,fina);
                                if (horariosInactividad == null || !horariosInactividad.Any() ||horariosInactividad.Count() < cantidadTercerosEspacio)
                                {
                                    int reser = daoAgenda.GetEspSinReserva(emp_codi, fini.Year, fini.Month, fini.Day, cla_cont, pro_cont, fini, fina);
                                    if (reser == 0)
                                    {
                                        return new DTOEntities.TSalida("Este horario no se encuentra disponible", 0);
                                    }
                                    else
                                    {
                                        return new DTOEntities.TSalida("", 1);
                                    }
                                    //return new DTOEntities.TSalida("", 1);
                                }
                                else
                                {
                                    return new DTOEntities.TSalida("El tercero está inactivo en este horario", 0);
                                }
                            }
                        }
                        else
                        {
                            return new DTOEntities.TSalida("No hay profesionales disponibles en este horario", 0);
                        }
                    }
                    else if (esp_mdit == "S")
                    {
                        //verificamos que el tercero no tenga reserva en este horario
                        int reservatercero = daoAgenda.GetReservaTercero(emp_codi, (int)ter_codi, pro_cont, fini, fina);
                        if (reservatercero == 0)
                        {
                            //validar que el tercero tenga disponible este horario
                            List<TODisponible> horariosTercero = daoAgenda.GetHorariosTercero(fini.Year, fini.Month, fini.Day, cla_cont, pro_cont, (int)ter_codi, "D", "");
                            if (horariosTercero != null && horariosTercero.Any() && horariosTercero.Where(x => fini < x.FechaFin && fina > x.FechaInicio).Count() == 0)
                            {
                                return new DTOEntities.TSalida("El tercero no está disponible en este horario", 0);
                            }
                            else
                            {
                                //buscamos haber si el horario se encuentra dentro del rango de inactividad
                                List<TODisponible> horariosInactividad = daoAgenda.GetHorariosTercero(fini.Year, fini.Month, fini.Day, cla_cont, pro_cont, (int)ter_codi, "I", "");
                                if (horariosInactividad == null || !horariosInactividad.Any() || horariosInactividad.Where(x => fini < x.FechaFin && fina > x.FechaInicio).Count() == 0)
                                {
                                    return new DTOEntities.TSalida("", 1);
                                }
                                else
                                {
                                    return new DTOEntities.TSalida("El tercero está inactivo en este horario", 0);
                                }
                            }
                        }
                        else
                        {
                            return new DTOEntities.TSalida("Este horario no se encuentra disponible", 0);
                        }
                    }
                    else
                    {
                        int reser = daoAgenda.GetEspSinReserva(emp_codi, fini.Year, fini.Month, fini.Day, cla_cont, pro_cont, fini, fina);
                        if (reser == 0)
                        {
                            return new DTOEntities.TSalida("Este horario no se encuentra disponible", 0);
                        }
                        else
                        {
                            return new DTOEntities.TSalida("", 1);
                        }
                    }
                }
            }
        }

        public IEnumerable<TODisponible> GetDisponiblesParametroDisp(int cla_cont, int pro_cont, int year, int month, int day, string esp_mdit, float ter_codi, string Op_Disp = "")
        {
            DateTime fechainicial = DateTime.Now;
            DateTime fechaFinal = DateTime.Now;
            //disponibilidad ya sea por tercero o espacio
            daoAgenda.GetFechaMinMaxDiponibilidad(emp_codi, year, month, day, cla_cont, out fechainicial, out fechaFinal, pro_cont, esp_mdit, ter_codi, Op_Disp);
            var producto = daoProducto.GetProducto(emp_codi, pro_cont);

            //int CantEspacios = daoAgenda.CantidadHorarios(emp_codi, cla_cont, year, month, day, ter_codi, producto.Pro_cont, esp_mdit);
            if (fechainicial == DateTime.MinValue)
            {
                fechainicial = new DateTime(year, month, day, 6, 0, 0);
                fechaFinal = new DateTime(year, month, day);
            }

            List<TODisponible> listItems = new List<TODisponible>();
            for (DateTime i = fechainicial; i < fechaFinal && producto != null && producto.Pro_dmin > 0; i = i.AddMinutes(producto.Pro_dmin))
            {
                TODisponible item = new TODisponible();
                item.FechaInicio = i;
                item.FechaFin = i.AddMinutes(producto.Pro_dmin);
                if (fechaFinal < item.FechaFin)
                {
                    break;
                }
                listItems.Add(item);
            }

            //listado de los intervalos que se encuentran no disponibles u ocupados
            //List<TOOcupado> intervalosNoDisponiblesOcupados = daoAgenda.GetOcupadosDipo(emp_codi, cla_cont, year, month, day, esp_mdit, -1, pro_cont, ter_codi);

            int CantEspacios = daoAgenda.CantidadHorarios(emp_codi, cla_cont, fechainicial.Year, fechainicial.Month, fechainicial.Day, ter_codi, pro_cont, esp_mdit, Op_Disp);
            List<TOOcupado> intervalosNoDisponiblesOcupados = daoAgenda.GetOcupadosDipo(emp_codi, cla_cont, fechainicial.Year, fechainicial.Month, fechainicial.Day, esp_mdit, -1, pro_cont, Op_Disp, ter_codi);

            DateTime ahora = DateTime.Now;

            listItems.ForEach(itemHora =>
            {
                DTOEntities.TSalida salida = ValidarDisponibilidadHorario(itemHora.FechaInicio,
                    itemHora.FechaFin, cla_cont, ter_codi, pro_cont, esp_mdit, Op_Disp, CantEspacios, intervalosNoDisponiblesOcupados);
                if (salida.retorno == 0)
                {
                    itemHora.Estado = 1;
                }
                else
                {
                    itemHora.Estado = 0;
                }
 
            });
            if (listItems != null)
                return listItems.Where(o => o.Estado == 0).ToList();
            return null;
        }

        public TOTransaction<List<TODisponible>> GetDisponiblesParametroViewMonth(int cla_cont, int pro_cont, int year, int month ,string esp_mdit, float ter_codi, string Op_Disp = "")
        {
            List<TODisponible> result = new List<TODisponible>();
            try
            {
                DateTime CurrentDate = DateTime.Now;
                int MinDay = 1;
                if(CurrentDate.Month == month && CurrentDate.Year == year)
                {
                    MinDay = CurrentDate.Day;
                }               
                int DaysInMonth = DateTime.DaysInMonth(year, month);
                for (int CurrentDay = MinDay; CurrentDay <= DaysInMonth; CurrentDay++)
                {
                    TODayDisponiblity DayView = new TODayDisponiblity();
                    DateTime fechainicial = DateTime.Now;
                    DateTime fechaFinal = DateTime.Now;
                    //disponibilidad ya sea por tercero o espacio
                    daoAgenda.GetFechaMinMaxDiponibilidad(emp_codi, year, month, CurrentDay, cla_cont, out fechainicial, out fechaFinal, pro_cont, esp_mdit, ter_codi, Op_Disp);
                    var producto = daoProducto.GetProducto(emp_codi, pro_cont);

                    //int CantEspacios = daoAgenda.CantidadHorarios(emp_codi, cla_cont, year, month, day, ter_codi, producto.Pro_cont, esp_mdit);
                    if (fechainicial == DateTime.MinValue)
                    {
                        fechainicial = new DateTime(year, month, CurrentDay, 6, 0, 0);
                        fechaFinal = new DateTime(year, month, CurrentDay);
                    }

                    List<TODisponible> listItems = new List<TODisponible>();
                    for (DateTime i = fechainicial; i < fechaFinal && producto != null && producto.Pro_dmin > 0; i = i.AddMinutes(producto.Pro_dmin))
                    {
                        TODisponible item = new TODisponible();
                        item.FechaInicio = i;
                        item.FechaFin = i.AddMinutes(producto.Pro_dmin);
                        if (fechaFinal < item.FechaFin)
                        {
                            break;
                        }
                        listItems.Add(item);
                    }

                    //listado de los intervalos que se encuentran no disponibles u ocupados
                    //List<TOOcupado> intervalosNoDisponiblesOcupados = daoAgenda.GetOcupadosDipo(emp_codi, cla_cont, year, month, day, esp_mdit, -1, pro_cont, ter_codi);

                    int CantEspacios = daoAgenda.CantidadHorarios(emp_codi, cla_cont, fechainicial.Year, fechainicial.Month, fechainicial.Day, ter_codi, pro_cont, esp_mdit, Op_Disp);
                    List<TOOcupado> intervalosNoDisponiblesOcupados = daoAgenda.GetOcupadosDipo(emp_codi, cla_cont, fechainicial.Year, fechainicial.Month, fechainicial.Day, esp_mdit, -1, pro_cont, Op_Disp, ter_codi);

                    DateTime ahora = DateTime.Now;

                    listItems.ForEach(itemHora =>
                    {
                        DTOEntities.TSalida salida = ValidarDisponibilidadHorario(itemHora.FechaInicio,
                            itemHora.FechaFin, cla_cont, ter_codi, pro_cont, esp_mdit, Op_Disp, CantEspacios, intervalosNoDisponiblesOcupados);
                        if (salida.retorno == 0)
                        {
                            itemHora.Estado = 1;
                        }
                        else
                        {
                            itemHora.Estado = 0;
                        }

                    });
                    if (listItems != null && listItems.Any())
                    {
                        result.AddRange(listItems.Where(o => o.Estado == 0).ToList());
                       
                    }
                                       
                }
               
                return new TOTransaction<List<TODisponible>>() { ObjTransaction = result, Retorno = 0, TxtError = "" };
            }

            catch(Exception ex)
            {
                return new TOTransaction<List<TODisponible>>() { ObjTransaction = null, Retorno = 1, TxtError = ex.Message };
            }
          

           
        }


        public IEnumerable<TONoDisponible> GetNoDisponiblesParametroDisp(int cla_cont, int year)
        {
            TONoDisponible TNoDisp = new TONoDisponible();

            var FechaNoD = daoAgenda.GetNoDisponi(emp_codi, cla_cont, year);

            return FechaNoD;
        }
        private DateTime step(DateTime i, int minutos)
        {
            i = i.AddMinutes(minutos);
            return i;
        }
    }

}