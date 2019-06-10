using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using AppFinal.Models;
using AppFinal.Services;
using GalaSoft.MvvmLight.Command;
using Xamarin.Forms;

namespace AppFinal.ViewModels
{
    public class    EditIncidenciaViewModel: BaseViewModel
    {
        #region Atributos

        private Incidencia incidencia;
        private bool isEnable;
        private ApiService apiService;
        private TipoIncidencia tipoIncidenciaSelect;
        private int tipoIncidenciaDefecto;

        #endregion


        #region Propiedades
        public List<TipoIncidencia> TipoIncidenciasList { get; set; }

        public int TipoIncidenciaDefecto
        {
            get
            {
                return this.tipoIncidenciaDefecto;
            }
            set
            {
                this.SetValue(ref this.tipoIncidenciaDefecto, value);
            }
        }

        public TipoIncidencia TipoIncidenciaSelect
        {
            get
            {
                return this.tipoIncidenciaSelect;
            }
            set
            {
                this.SetValue(ref this.tipoIncidenciaSelect, value);
            }
        }

        public bool IsEnable
        {
            get 
                { return this.isEnable; }
            set { this.SetValue(ref this.isEnable, value); }
        }
        public Incidencia Incidencia
        {
            get
            {
                return this.incidencia;
            }
            set
            {
                this.SetValue(ref  this.incidencia, value);
            }
        }
        #endregion



        #region Contructores]
        public EditIncidenciaViewModel(Incidencia incidencia)
        {
            this.incidencia = incidencia;
            this.TipoIncidenciasList = GetTipoIncidencias().OrderBy(t => t.Key).ToList();
            this.TipoIncidenciaSelect = this.TipoIncidenciasList[int.Parse(this.incidencia.TipoIncidencia)];
            this.apiService = new ApiService();
            var id = incidencia.TipoIncidencia;
            this.IsEnable = true;
        }

        public EditIncidenciaViewModel()
        {
        }
        #endregion


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

        public ICommand UpdateCommand
        {
            get
            {
                return new RelayCommand(Update);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new RelayCommand(Delete);
            }
        }



        private async void Delete()
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Confirmacion", "¿Esta seguro de eliminar esta incidencia?", "Aceptar", "Cancelar");
            if (!answer)
            {
                return;
            }
            this.isEnable = false;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.isEnable = true;
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Aceptar");
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["Prefix"].ToString();
            var controller = Application.Current.Resources["Controller"].ToString();
            var response = await this.apiService.Delete(url, prefix, controller, this.Incidencia.Id);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            var incidenciasViewModel = IncidenciasViewModel.GetInstance();
            var deletedIncidencia = incidenciasViewModel.Incidencias.Where(P => P.Id == this.Incidencia.Id).FirstOrDefault();
            if (deletedIncidencia != null)
            {
                incidenciasViewModel.Incidencias.Remove(deletedIncidencia);
            }

            this.IsEnable = true;
            await Application.Current.MainPage.Navigation.PopAsync();

        }



        private async void Update()
        {
            if (string.IsNullOrEmpty(this.incidencia.Responsable))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe ingresar un responsable", "Aceptar");
                return;
            }

            if (this.TipoIncidenciaSelect == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe seleccionar un tipo de incidencte", "Aceptar");
                return;
            }


            if (string.IsNullOrEmpty(this.incidencia.FechaIncidencia.ToString()))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Debe ingresar una fecha", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(this.incidencia.Motivo))
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


            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["Prefix"].ToString();
            var controller = Application.Current.Resources["Controller"].ToString();
            var response = await this.apiService.Put(url, prefix, controller, this.incidencia);

            if (!response.IsSuccess)
            {
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }


            var incidenciasViewModel = IncidenciasViewModel.GetInstance();
            var deletedIncidencia = incidenciasViewModel.Incidencias.Where(P => P.Id == this.incidencia.Id).FirstOrDefault();
            if (deletedIncidencia != null)
            {
                incidenciasViewModel.Incidencias.Remove(deletedIncidencia);
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
