using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOEntities
{
    public class TSalida
    {
        public TSalida() { }
        public TSalida(string txterror, int retorno)
        {
            Txterror = txterror;
            this.retorno = retorno;
        }
        public string Txterror { get; set; }
        public int retorno { get; set; }
    }
}
