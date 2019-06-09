using System;
using System.Collections.Generic;
using System.Linq;
using AppFinal.Models;

namespace AppFinal.ViewModels
{
    public class AddIncidenciaViewModel : BaseViewModel
    {

        public List<TipoIncidencia> TipoIncidenciasList { get; set; }

       
        public AddIncidenciaViewModel()
        {
            this.TipoIncidenciasList = GetTipoIncidencias().OrderBy(t => t.Key).ToList();
        }


        #region Repositorio de Tipo de incidencias

        public List<TipoIncidencia> GetTipoIncidencias()
        {
            var TiposIncidencias = new List<TipoIncidencia>()
            {
                new TipoIncidencia(){ Key = 1 , Value = "Leve"},
                new TipoIncidencia(){ Key = 1 , Value = "Moderado"},
                new TipoIncidencia(){ Key = 1 , Value = "Grave"},
                new TipoIncidencia(){ Key = 1 , Value = "Critico"}
            };
            return TiposIncidencias;
        }

        #endregion



    }
}
