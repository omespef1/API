
using SevenReservas.BO;
using SevenReservas.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Linq;

namespace SevenReservas.Controllers
{
    public class GnTerceController : ApiController
    {
        BOGnTerce boGnTerce = new BOGnTerce();

        [HttpPost]
        public List<TOGnTerce> GetImageTerce(List<string> codes)
        {
            //System.Threading.Thread.Sleep(5000);
            List<TOGnTerce> images = boGnTerce.GetImageTerce(codes);
            if (images != null && images.Count > 0)
            {
                return boGnTerce.GetImageTerce(codes);
            }
            else
            {
                return null;
            }
        }

        //public string GetImageTerce(string ter_codi)
        //{
        //    var img = boGnTerce.GetImageTerce(ter_codi);
        //    if (img.Ter_foto != null)
        //    {
        //        return "data:image/jpeg;base64," + Convert.ToBase64String(boGnTerce.GetImageTerce(ter_codi).Ter_foto);
        //    }
        //    else
        //    {
        //        return "";
        //    }

        //}

        // GET: api/Usuarios/5
        [HttpGet]
        [Route("api/GnTerce")]
        public List<TOGnTerce> Get(int Cla_cont, int pro_cont)
        {
            var terceros = boGnTerce.GetGnTerce(Cla_cont, pro_cont);

            return terceros;
        }

        // GET: api/Usuarios/5
        [HttpGet]
        [Route("api/GnTerce/GetGnTerceNewVersion")]
        public TOTransaction< List<TOGnTerce>> GetGnTerceNewVersion(int Cla_cont, int pro_cont, DateTime? Fini = null, DateTime? Ffin = null, string Op_Disp = "")
        {
            if (Fini != null)
                Fini = Fini.Value.AddMinutes(1);
            if (Ffin != null)
                Ffin = Ffin.Value.AddMinutes(-1);
            var terceros = boGnTerce.GetGnTerceNewVersion(Cla_cont, pro_cont, Fini, Ffin, Op_Disp);

            return terceros;
        }
    }
}
