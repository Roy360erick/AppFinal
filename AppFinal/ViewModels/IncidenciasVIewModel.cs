namespace AppFinal.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using AppFinal.Models;
    using AppFinal.Services;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class IncidenciasViewModel : BaseViewModel
    {

        private ApiService apiService;

        private ObservableCollection<Incidencia> incidencias;

        public bool isRefreshing;

        public ObservableCollection<Incidencia> Incidencias 
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
            this.apiService = new ApiService();
            this.LoadIncidencias();
        }


        private async void LoadIncidencias()
        {
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();
            if(!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Aceptar");
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["Prefix"].ToString();
            var controller = Application.Current.Resources["Controller"].ToString();
            var response = await this.apiService.GetList<Incidencia>(url,prefix, controller);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = true;
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
            }

            var list = (List<Incidencia>)response.Result;
            this.Incidencias = new ObservableCollection<Incidencia>(list);
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
