using System;
namespace AppFinal.ViewModels
{
    public class MainViewModel
    {
        public IncidenciasViewModel Incidencias{ get; set; }


        public MainViewModel()
        {
            this.Incidencias = new IncidenciasViewModel();
        }
    }
}
