namespace SevenReservas.Models
{
    public class TOGnTerce
    {
       public TOGnTerce()
        {
            Disponible = true;
        }


        public int Ter_codi { get; set; }
        public string Ter_noco { get; set; }
        public  byte[] Ter_foto  { get; set; }
        public bool Disponible { get; set; }
    }
}
