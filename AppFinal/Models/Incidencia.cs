namespace AppFinal.Models
{
    using System;
    public class Incidencia
    {
        public int id { get; set; }
        public string responsable { get; set; }
        public string motivo { get; set; }
        public int tipoIncidencia { get; set; }
        public DateTime fechaIncidencia { get; set; }
        public bool estado { get; set; }
    }
}
