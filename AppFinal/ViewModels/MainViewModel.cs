namespace AppFinal.ViewModels
{
    using System;
    using System.Windows.Input;
    using AppFinal.Views;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class MainViewModel
    {
        public IncidenciasViewModel Incidencias { get; set; }

        public AddIncidenciaViewModel AddIncidencia { get; set; }



        public MainViewModel()
        {
            this.Incidencias = new IncidenciasViewModel();
        }

        public ICommand AddIncidenciaCommand 
        {
            get
            {
                return new RelayCommand(GoToAddIncidencia);
            }
        }

        private async void GoToAddIncidencia()
        {
            this.AddIncidencia = new AddIncidenciaViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AddIncidenciaPage());
        }
    }
}
