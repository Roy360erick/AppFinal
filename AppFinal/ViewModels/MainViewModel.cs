namespace AppFinal.ViewModels
{
    using System;
    using System.Windows.Input;
    using AppFinal.Views;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class MainViewModel
    {
        #region Propiedades

        public IncidenciasViewModel Incidencias { get; set; }

        public AddIncidenciaViewModel AddIncidencia { get; set; }

        public EditIncidenciaViewModel EditIncidencia { get; set; }

        #endregion



        public MainViewModel()
        {
            instance = this;
            this.Incidencias = new IncidenciasViewModel();
        }


        #region Sigleton 

        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }

        #endregion


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
