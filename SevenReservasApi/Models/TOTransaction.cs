using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.Models
{
    public class TOTransaction<T> where T:class
    {
        public int Retorno { get; set; }
        public string TxtError { get; set; }
        public T ObjTransaction { get; set; }
    }
}