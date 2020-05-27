using Dapper;
using FlightControlWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using System.Diagnostics;
using FlightControlWeb.Types;

namespace FlightControlWeb
{
    public class SqliteDataAccess : IDataManager
    {
        public SqliteDataAccess()
        {
        }
        
        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public List<dynamic> ExcuteQuery(string QueryString)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var x = cnn.Query(QueryString, new DynamicParameters()).ToList();
                return x;
            }

        }




        public List<dynamic> ExcuteQuery<T>(string QueryString, T genericObject)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var x = cnn.Query(QueryString,genericObject).ToList();
                return x;
            }
        }
    }
}
