using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToSQL1
{
    class Program
    {
        const string _serverName = @"DESKTOP-ETTAF0H";
        const string _databaseName = "a-test3";

        static void Main(string[] args)
        {
            CsvBulkCopyDataIntoSqlServer copyAgent = new CsvBulkCopyDataIntoSqlServer(_serverName, _databaseName);
            copyAgent.LoadCsvDataIntoSqlServer();
        }
    }
}
