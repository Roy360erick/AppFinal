namespace AppFinal.ViewModels
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using AppFinal.Models;
    using AppFinal.Services;
    using AppFinal.Views;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class IncidenciaItemViewModel : Incidencia
    {

        private ApiService apiService;

        public IncidenciaItemViewModel()
        {
            this.apiService = new ApiService();
        }

        #region Comandos
        public ICommand DeleteIncidenciaCommand
        {
            get
            {
                return new RelayCommand(DeleteIncidencia);
            }
        }

        public ICommand EditIncidenciaCommand
        {
            get
            {
                return new RelayCommand(EditIncidencia);
            }
        }
        #endregion


        #region Metodos

        private async void DeleteIncidencia()
        {
            var answer = await Application.Current.MainPage.DisplayAlert("Confirmacion","¿Esta seguro de eliminar esta incidencia?","Aceptar","Cancelar");
            if (!answer)
            {
                return;
            }

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", connection.Message, "Aceptar");
                return;
            }

            var url = Application.Current.Resources["UrlAPI"].ToString();
            var prefix = Application.Current.Resources["Prefix"].ToString();
            var controller = Application.Current.Resources["Controller"].ToString();
            var response = await this.apiService.Delete(url, prefix, controller,this.Id);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            var incidenciasViewModel = IncidenciasViewModel.GetInstance();
            var deletedIncidencia = incidenciasViewModel.Incidencias.Where(P => P.Id == this.Id).FirstOrDefault();
            if(deletedIncidencia != null)
            {
                incidenciasViewModel.Incidencias.Remove(deletedIncidencia);
            }
        }




        private async void EditIncidencia()
        {
            MainViewModel.GetInstance().EditIncidencia = new EditIncidenciaViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditIncidenciaPage());
        }

        #endregion
    }
}
