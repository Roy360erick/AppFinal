[assembly: Xamarin.Forms.Dependency(typeof(AppFinal.iOS.Implentations.PathService))]
namespace AppFinal.iOS.Implentations
{
    using System;
    using System.IO;
    using AppFinal.Intefaces;
    public class PathService : IPathService
    {
        public string GetDatabasePath()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, "Incidencias.db3");
        } 
    }
}
