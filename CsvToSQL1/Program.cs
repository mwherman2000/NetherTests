using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToSQL1
{
    class Program
    {
        static void Main(string[] args)
        {
            CsvBulkCopyDataIntoSqlServer copyAgent = new CsvBulkCopyDataIntoSqlServer();
            copyAgent.LoadCsvDataIntoSqlServer();
        }
    }
}
