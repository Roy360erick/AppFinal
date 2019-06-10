namespace AppFinal.Models
{
    using System;
    public class Incidencia
    {
        public int Id { get; set; }
        public string Responsable { get; set; }
        public string Motivo { get; set; }
        public string TipoIncidencia { get; set; }
        public DateTime FechaIncidencia { get; set; }
        public bool Estado { get; set; }


        public override string ToString()
        {
            return this.Responsable;
        }
    }
}
