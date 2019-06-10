namespace AppFinal.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using AppFinal.Models;
    using AppFinal.Services;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class IncidenciasViewModel : BaseViewModel
    {

        #region Atributos

        private ApiService apiService;

        public bool isRefreshing;

        #endregion

        private ObservableCollection<IncidenciaItemViewModel> incidencias;



        public ObservableCollection<IncidenciaItemViewModel> Incidencias 
        {
            get { return this.incidencias; }
            set { this.SetValue(ref this.incidencias , value); }
        }

        public bool IsRefreshing 
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }



        public IncidenciasViewModel()
        {
            instance = this;
            this.apiService = new ApiService();
            this.LoadIncidencias();
        }

        #region Sigleton 

        private static IncidenciasViewModel instance;

        public static IncidenciasViewModel GetInstance()
        {
            if(instance == null )
            {
                return new IncidenciasViewModel();  
            }

            return instance;
        }

        #endregion


        private async void LoadIncidencias()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();
            if(!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Aceptar");
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["Prefix"].ToString();
            var controller = Application.Current.Resources["Controller"].ToString();
            var response = await this.apiService.GetList<Incidencia>(url,prefix, controller);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = true;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            var list = (List<Incidencia>)response.Result;
            var myList = list.Select(p => new IncidenciaItemViewModel
            {
                Id = p.Id,
                Responsable = p.Responsable,
                TipoIncidencia = p.TipoIncidencia,
                FechaIncidencia = p.FechaIncidencia,
                Estado = p.Estado,
                Motivo = p.Motivo
            });

            this.Incidencias = new ObservableCollection<IncidenciaItemViewModel>(myList);
            this.IsRefreshing = false;

        }

        public ICommand RefreshCommand
        {
            get 
            {
                return new RelayCommand(LoadIncidencias);
            }
        }


    }
}
