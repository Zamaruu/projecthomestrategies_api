using MySql.Data.MySqlClient;    
using System;    
using System.Collections.Generic;    
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;

namespace HomeStrategiesApi.Helper    
{    
    public class HomeStrategiesDBContext   
    {    
        private readonly string MySQLConnectionString;  

        public HomeStrategiesDBContext()    
        {    
            MySQLConnectionString = AppSettingsService.AppSetting["ApiSettings:MySQLConnectionString"];
        }    
    
        public MySqlConnection GetSQLConnection()    
        {    
            return new MySqlConnection(MySQLConnectionString);    
        }

        public void GetMongoDBConnection(){

        }
    }    
}  