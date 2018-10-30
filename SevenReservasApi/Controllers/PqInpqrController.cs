using DTOEntities;
using SevenReservas.BO;
using SevenReservas.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace SevenReservas.Controllers
{
    public class PqInpqrController : ApiController
    {
        BOPqInpqr boPqInpqr = new BOPqInpqr();

        public TOTransaction<List<Models.TOPqInpqr>> Get(int soc_cont, int sbe_cont,short emp_codi)
        {
            return boPqInpqr.GetPqInpqr(soc_cont, sbe_cont,emp_codi);
        }

        [HttpGet]
        [Route("api/pqinpqr/segui")]
        public IEnumerable<Models.TOPqDinpq> GetPqDinpq(int inp_cont,short emp_codi)
        {
            return boPqInpqr.GetPqDinpq(inp_cont,emp_codi);
        }

        [HttpGet]
        [Route("api/pqinpqr/seguiDetail")]
        public IEnumerable<Models.TOPqDinpq> GetPqDinpqDetail(int inp_cont,int din_cont,short emp_codi)
        {
            return boPqInpqr.GetPqDinpqDetail(inp_cont, din_cont,emp_codi);
        }


        [HttpPost]
        [Route("api/pqinpqr/Aceptar")]
        public TOTransaction<Models.TOPqInpqr> Post(DTOEntities.TOPqInpqr toPqInpqr)
        {
            return boPqInpqr.SetPqInpqr(toPqInpqr);
        }
    }
}
