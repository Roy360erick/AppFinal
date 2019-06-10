using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppFinal.Intefaces;
using AppFinal.Models;
using SQLite;
using Xamarin.Forms;

namespace AppFinal.Services
{
    public class DataService
    {

        #region Atributos
        private SQLiteAsyncConnection connection;
        #endregion
        public DataService()
        {
            this.OpenOrCreateDB();
        }

        private async void OpenOrCreateDB()
        {
            var databasepath = DependencyService.Get<IPathService>().GetDatabasePath();
            this.connection = new SQLiteAsyncConnection(databasepath);
            await connection.CreateTableAsync<Incidencia>().ConfigureAwait(false);
        }

        public async Task Insert<T>(T model)
        {
            await this.connection.InsertAsync(model);
        }

        public async Task Insert<T>(List<T> models)
        {
            await this.connection.InsertAllAsync(models);
        }

        public async Task Update<T>(T model)
        {
            await this.connection.UpdateAsync(model);
        }

        public async Task Update<T>(List<T> models)
        {
            await this.connection.UpdateAllAsync(models);
        }

        public async Task Detele<T>(T model)
        {
            await this.connection.DeleteAsync(model);
        }

    }
}
