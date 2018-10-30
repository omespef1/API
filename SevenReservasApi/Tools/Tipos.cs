using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SevenReservas.Tools
{
    public static class Tipos
    {
        public static byte[] AsFoto(this object objeto)
        {
            try
            {
                return (byte[])objeto;
            } catch
            {
                return null;
            }
            
        }
    }
}