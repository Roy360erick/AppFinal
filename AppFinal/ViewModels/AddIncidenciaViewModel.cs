namespace AppFinal.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using AppFinal.Models;
    using AppFinal.Services;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class AddIncidenciaViewModel : BaseViewModel
    {

        #region Atributos
        private bool isEnable;
        private ApiService apiService;
        #endregion

        #region Propiedades

        public List<TipoIncidencia> TipoIncidenciasList { get; set; }
        public string Responsable { get; set; }

        public TipoIncidencia TipoIncidenciaSelect { get; set; }
        public DateTime FechaIncidencia { get; set; }
        public bool Estado { get; set; }
        public string Motivo { get; set; }
        public bool IsEnable 
        {
            get { return this.isEnable; }
            set { this.SetValue(ref this.isEnable, value); }
        }
        #endregion





        public AddIncidenciaViewModel()
        {
            this.TipoIncidenciasList = GetTipoIncidencias().OrderBy(t => t.Key).ToList();
            this.Estado = true;
            this.IsEnable = true;
            this.apiService = new ApiService();
        }


        #region Repositorio de Tipo de incidencias

        public List<TipoIncidencia> GetTipoIncidencias()
        {
            var TiposIncidencias = new List<TipoIncidencia>()
            {
                new TipoIncidencia(){ Key = 1 , Value = "Leve"},
                new TipoIncidencia(){ Key = 2 , Value = "Moderado"},
                new TipoIncidencia(){ Key = 3 , Value = "Grave"},
                new TipoIncidencia(){ Key = 4 , Value = "Critico"}
            };
            return TiposIncidencias;
        }

        #endregion

        #region Commandos

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }



        private async void Save()
        {
            if (string.IsNullOrEmpty(this.Responsable))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe ingresar un responsable", "Aceptar");
                return;
            }

            if (this.TipoIncidenciaSelect == null )
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar un tipo de incidencte", "Aceptar");
                return;
            }


            if (string.IsNullOrEmpty(this.FechaIncidencia.ToString()))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe ingresar una fecha", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.Motivo))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe ingresar un motivo", "Aceptar");
                return;
            }

            this.IsEnable = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Aceptar");
                return;
            }

            var incidencia = new Incidencia()
            {
                Responsable = this.Responsable,
                TipoIncidencia =Convert.ToString(this.TipoIncidenciaSelect.Key),
                FechaIncidencia = this.FechaIncidencia,
                Estado = this.Estado,
                Motivo = this.Motivo
            };

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["Prefix"].ToString();
            var controller = Application.Current.Resources["Controller"].ToString();
            var response = await this.apiService.Post(url, prefix, controller,incidencia);

            if (!response.IsSuccess)
            {
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            var newIncidencia = (Incidencia)response.Result;
            var ViewModel = IncidenciasViewModel.GetInstance();

            ViewModel.Incidencias.Add(new IncidenciaItemViewModel
            {
                Id = newIncidencia.Id,
                Responsable = newIncidencia.Responsable,
                TipoIncidencia = newIncidencia.TipoIncidencia,
                FechaIncidencia = newIncidencia.FechaIncidencia,
                Estado = newIncidencia.Estado,
                Motivo = newIncidencia.Motivo
            });

            this.IsEnable = true;
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        #endregion
    }
}
