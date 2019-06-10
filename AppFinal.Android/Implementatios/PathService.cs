[assembly: Xamarin.Forms.Dependency(typeof(AppFinal.Droid.Implementatios.PathService))]
namespace AppFinal.Droid.Implementatios
{
    using System;
    using System.IO;
    using AppFinal.Intefaces;
    using Xamarin.Forms;


    public class PathService :IPathService
    {

        public string GetDatabasePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, "Incidencias.db3");
        }
    }
}
