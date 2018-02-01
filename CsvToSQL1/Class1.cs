using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;

namespace CsvToSQL1
{
    public class CsvBulkCopyDataIntoSqlServer
    {
        bool _truncate = false;
        int  _tableNumber = 2;
        string _databaseName = "a-test3"; // "a-test4D"; // "a-test3";

        string _serverName = @"DESKTOP-ETTAF0H";
        string _path = @"C:\Temp\1999994-2000000\"; // @"C:\Temp\a-test1";
        string[] _tableNames = { "Aaddr", "Ablocks", "Atx" };

        const int _batchSize = 100000;

        Dictionary<string, System.Type> _fieldTypes = new Dictionary<string, System.Type>();

        public CsvBulkCopyDataIntoSqlServer()
        {
            _fieldTypes.Add("Address", typeof(System.String));
            _fieldTypes.Add("From", typeof(System.String));
            _fieldTypes.Add("To", typeof(System.String));
            _fieldTypes.Add("Type", typeof(System.String));
            _fieldTypes.Add("BlockHash", typeof(System.String));
            _fieldTypes.Add("BlockNumber", typeof(System.UInt32)); // 2,147,483,647
            _fieldTypes.Add("EndpointType", typeof(System.String));
            _fieldTypes.Add("TxCount", typeof(System.UInt32));
            _fieldTypes.Add("TxHash", typeof(System.String));
            _fieldTypes.Add("TxIndex", typeof(System.UInt32));
            _fieldTypes.Add("Timestamp", typeof(System.UInt32));
            _fieldTypes.Add("Value", typeof(System.UInt64));
            _fieldTypes.Add("Gas", typeof(System.UInt64));
            _fieldTypes.Add("Year", typeof(System.UInt32));
            _fieldTypes.Add("Month", typeof(System.UInt32));
            _fieldTypes.Add("Week", typeof(System.UInt32));
            _fieldTypes.Add("DayOfYear", typeof(System.UInt32));
            _fieldTypes.Add("DayOfMonth", typeof(System.UInt32));
            _fieldTypes.Add("DayOfWeek", typeof(System.UInt32));
            _fieldTypes.Add("Hour", typeof(System.UInt32));
            _fieldTypes.Add("ValueLarge", typeof(System.Decimal));
        }

        public void LoadCsvDataIntoSqlServer()
        {
            // This should be the full path
            var fileName = _path + _tableNames[_tableNumber] + ".csv";

            using (var textFieldParser = new TextFieldParser(fileName))
            {
                textFieldParser.TextFieldType = FieldType.Delimited;
                textFieldParser.Delimiters = new[] { "," };
                textFieldParser.HasFieldsEnclosedInQuotes = false;

                DataTable dtSQLLoader = new DataTable();
                var dtCSVInput = new DataTable(_tableNames[_tableNumber]);
 
                switch (dtCSVInput.TableName)
                {
                    case "Aaddr":
                        {
                            dtCSVInput.Columns.Add("Address", _fieldTypes["Address"]);
                            dtCSVInput.Columns.Add("Type", _fieldTypes["Type"]);
                            dtCSVInput.Columns.Add("BlockNumber", _fieldTypes["BlockNumber"]);
                            dtCSVInput.Columns.Add("EndpointType", _fieldTypes["EndpointType"]);
                            dtCSVInput.Columns.Add("TxHash", _fieldTypes["TxHash"]);
                            dtCSVInput.Columns.Add("Timestamp", _fieldTypes["Timestamp"]);
                            dtCSVInput.Columns.Add("ValueLarge", _fieldTypes["ValueLarge"]);

                            dtCSVInput.Columns.Add("Year", _fieldTypes["Year"]);
                            dtCSVInput.Columns.Add("Month", _fieldTypes["Month"]);
                            dtCSVInput.Columns.Add("Week", _fieldTypes["Week"]);
                            dtCSVInput.Columns.Add("DayOfYear", _fieldTypes["DayOfYear"]);
                            dtCSVInput.Columns.Add("DayOfMonth", _fieldTypes["DayOfMonth"]);
                            dtCSVInput.Columns.Add("DayOfWeek", _fieldTypes["DayOfWeek"]);
                            dtCSVInput.Columns.Add("Hour", _fieldTypes["Hour"]);
                            break;
                        }
                    case "Ablocks":
                        {
                            dtCSVInput.Columns.Add("BlockNumber", _fieldTypes["BlockNumber"]);
                            dtCSVInput.Columns.Add("Timestamp", _fieldTypes["Timestamp"]);
                            dtCSVInput.Columns.Add("BlockHash", _fieldTypes["BlockHash"]);
                            dtCSVInput.Columns.Add("TxCount", _fieldTypes["TxCount"]);

                            dtCSVInput.Columns.Add("Year", _fieldTypes["Year"]);
                            dtCSVInput.Columns.Add("Month", _fieldTypes["Month"]);
                            dtCSVInput.Columns.Add("Week", _fieldTypes["Week"]);
                            dtCSVInput.Columns.Add("DayOfYear", _fieldTypes["DayOfYear"]);
                            dtCSVInput.Columns.Add("DayOfMonth", _fieldTypes["DayOfMonth"]);
                            dtCSVInput.Columns.Add("DayOfWeek", _fieldTypes["DayOfWeek"]);
                            dtCSVInput.Columns.Add("Hour", _fieldTypes["Hour"]);
                            break;
                        }
                    case "Atx":
                        {
                            dtCSVInput.Columns.Add("BlockNumber", _fieldTypes["BlockNumber"]);
                            dtCSVInput.Columns.Add("TxIndex", _fieldTypes["TxIndex"]);
                            dtCSVInput.Columns.Add("TxHash", _fieldTypes["TxHash"]);
                            dtCSVInput.Columns.Add("From", _fieldTypes["From"]);
                            dtCSVInput.Columns.Add("To", _fieldTypes["To"]);
                            dtCSVInput.Columns.Add("ValueLarge", _fieldTypes["ValueLarge"]);
                            dtCSVInput.Columns.Add("Gas", _fieldTypes["Gas"]);
                            dtCSVInput.Columns.Add("Timestamp", _fieldTypes["Timestamp"]);

                            dtCSVInput.Columns.Add("Year", _fieldTypes["Year"]);
                            dtCSVInput.Columns.Add("Month", _fieldTypes["Month"]);
                            dtCSVInput.Columns.Add("Week", _fieldTypes["Week"]);
                            dtCSVInput.Columns.Add("DayOfYear", _fieldTypes["DayOfYear"]);
                            dtCSVInput.Columns.Add("DayOfMonth", _fieldTypes["DayOfMonth"]);
                            dtCSVInput.Columns.Add("DayOfWeek", _fieldTypes["DayOfWeek"]);
                            dtCSVInput.Columns.Add("Hour", _fieldTypes["Hour"]);
                            break;
                        }
                    default:
                        {
                            throw new ArgumentOutOfRangeException("Unknown table name: '" + dtCSVInput.TableName + "'");
                        }
                }

                var connectionString = string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True;MultipleActiveResultSets=True",
    _serverName, _databaseName);
                Console.WriteLine("Connection String: " + connectionString);
                using (var sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.Open();

                    // Truncate the live table
                    string sqlTruncate = String.Format(@"TRUNCATE TABLE [{0}]", _tableNames[_tableNumber]);
                    Console.WriteLine("Command: " + sqlTruncate);
                    if (_truncate)
                    {
                        using (var sqlCommand = new SqlCommand(sqlTruncate, sqlConnection))
                        {
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Skipping: " + sqlTruncate);
                    }

                    // SELECT against the empty table to get the extract schema needed for the bulk load
                    string sqlSelectStar = String.Format("SELECT TOP 1 * FROM [{0}].[dbo].[{1}]", _databaseName, _tableNames[_tableNumber]);
                    SqlCommand cmdSQLLoader = new SqlCommand(sqlSelectStar, sqlConnection);
                    Console.WriteLine("Command: " + sqlSelectStar);
                    SqlDataAdapter daSQLLoader = new SqlDataAdapter(cmdSQLLoader);
                    int rows = daSQLLoader.Fill(dtSQLLoader);
                    dtSQLLoader.TableName = _tableNames[_tableNumber];
                    if (rows > 0) dtSQLLoader.Rows.Clear();

                    // Create the bulk copy object
                    var sqlBulkCopy = new SqlBulkCopy(sqlConnection)
                    {
                        DestinationTableName = dtSQLLoader.TableName,
                        BulkCopyTimeout = 120 // seconds
                    };

                    // Setup the column mappings, anything ommitted is skipped
                    foreach (DataColumn c in dtSQLLoader.Columns)
                    {
                        Console.WriteLine("SQL Column:\t" + c.Ordinal.ToString() + " " + c.ColumnName);
                        sqlBulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                    }

                    // Loop through the CSV and load each set of 100,000 records into a DataTable
                    // Then send it to the LiveTable

                    textFieldParser.ReadFields(); // Throw header row away

                    // Initialize input mapping tables
                    int nCSVColumns = dtCSVInput.Columns.Count;
                    int[] iCSVOrdinals = new int[nCSVColumns];
                    int[] iSQLOrdinals = new int[nCSVColumns];
                    int iColumn = 0;
                    foreach (DataColumn c in dtCSVInput.Columns)
                    {
                        Console.WriteLine("CSV Column:\t" + c.Ordinal.ToString() + " " + c.ColumnName);
                        int so = dtSQLLoader.Columns[c.ColumnName].Ordinal;
                        int co = dtCSVInput.Columns[c.ColumnName].Ordinal;
                        Console.WriteLine("CSV Column:\t" + so.ToString() + " < " + co.ToString());
                        iSQLOrdinals[iColumn] = so;
                        iCSVOrdinals[iColumn] = co;
                        iColumn++;
                    }

                    var createdCount = 0;
                    while (!textFieldParser.EndOfData)
                    {
                        var csvData = textFieldParser.ReadFields();
                        dtCSVInput.Rows.Clear();
                        dtCSVInput.Rows.Add(csvData);
                        DataRow drCSVInput = dtCSVInput.Rows[0];

                        DataRow drSQLLoader = dtSQLLoader.NewRow();
                        for (int iCol = 0; iCol < nCSVColumns; iCol++)
                        {
                            int so = iSQLOrdinals[iCol];
                            int co = iCSVOrdinals[iCol];
                            drSQLLoader[so] = drCSVInput[co];
                            if (createdCount == 0) Console.WriteLine(dtSQLLoader.Columns[so].ColumnName + ":\t" + drSQLLoader[so].ToString() + " " + drCSVInput[co].ToString());
                        }
                        dtSQLLoader.Rows.Add(drSQLLoader);

                        createdCount++;

                        if (createdCount % _batchSize == 0)
                        {
                            InsertDataTable(sqlBulkCopy, sqlConnection, dtSQLLoader);
                            Console.Write("." + ((int)(createdCount / _batchSize)).ToString());
                            //break; // DEBUG
                        }
                        //break; // HEADER ROW
                    }

                    // Don't forget to send the last batch under 100,000
                    if (createdCount % _batchSize != 0)
                    {
                        InsertDataTable(sqlBulkCopy, sqlConnection, dtSQLLoader);
                        Console.Write("*");
                    }
                    Console.WriteLine("|");

                    sqlConnection.Close();
                }
            }

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        protected void InsertDataTable(SqlBulkCopy sqlBulkCopy, SqlConnection sqlConnection, DataTable dataTable)
        {
            sqlBulkCopy.WriteToServer(dataTable);

            dataTable.Rows.Clear();
        }
    }
}