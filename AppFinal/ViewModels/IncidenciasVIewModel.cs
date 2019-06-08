namespace AppFinal.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using AppFinal.Models;
    using AppFinal.Services;
    using Xamarin.Forms;

    public class IncidenciasVIewModel : BaseViewModel
    {

        private ApiService apiService;

        private ObservableCollection<Incidencia> incidencias;

        public ObservableCollection<Incidencia> Incidencias 
        {
            get { return this.incidencias; }
            set { this.SetValue(ref this.incidencias , value); }
        }

        public IncidenciasVIewModel()
        {
            this.apiService = new ApiService();
            this.LoadIncidencias();
        }

        private async void LoadIncidencias()
        {
            var response = await this.apiService.GetList<Incidencia>("https://integrador-roy360erick.c9users.io:8080", "/api", "/incidencia");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
            }

            var list = (List<Incidencia>)response.Result;
            this.Incidencias = new ObservableCollection<Incidencia>(list);

        }
    }
}
