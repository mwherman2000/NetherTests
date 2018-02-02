using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EtherToSQL1
{
    class Program
    {
        static string _serverName = @"DESKTOP-ETTAF0H";
        static string _databaseName = "a-test3"; // "a-test4D"; // "a-test3";

        static string _AMTableName = "AddressMaster2";
        static string _ABTableName = "AddressBalances2";

        static int _batchSize = 1000;

        static string _connectionString = String.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True;MultipleActiveResultSets=True;Connection Timeout=120",
                                                         _serverName, _databaseName);
        static string _queryABTop1 = String.Format("SELECT TOP 1 * FROM {0};", _ABTableName);
        static string _queryAMAll = String.Format("SELECT [Address], [Type] FROM [{0}] WHERE [Address] > '0x0ab41f5bd491413b53aeda1b4ee61eb1dcd0dc35' ORDER BY [Address], [Type]", _AMTableName);
        //static string _templateAMOffsetFetch = "SELECT [Address], [Type] FROM [{0}] ORDER BY [Address], [Type] " +
        //                                        "OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY;";

        static DataTable dtAM = null;
        static DataTable dtAB = null;

        static void Main(string[] args)
        {
            Console.WriteLine("Connection String: " + _connectionString);

            DataTable dtAB = new DataTable();
            dtAB.TableName = _ABTableName;
            DataTable dtAM = new DataTable();
            dtAM.TableName = _AMTableName;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("ConnectionTimeout: " + connection.ConnectionTimeout.ToString());

                Console.WriteLine("_queryABTop1: " + _queryABTop1);
                SqlCommand commandABTop1 = new SqlCommand(_queryABTop1, connection);
                SqlDataAdapter daABLoader = new SqlDataAdapter(commandABTop1);
                int rows = daABLoader.Fill(dtAB);
                if (rows > 0) dtAB.Rows.Clear();

                // Create the bulk copy object
                var sqlBulkCopy = new SqlBulkCopy(connection)
                {
                    DestinationTableName = dtAB.TableName,
                    BulkCopyTimeout = 120 // seconds
                };

                // Setup the column mappings, anything ommitted is skipped
                foreach (DataColumn c in dtAB.Columns)
                {
                    Console.WriteLine("SQL Column:\t" + c.Ordinal.ToString() + " " + c.ColumnName);
                    sqlBulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName);
                }

                /* https://stackoverflow.com/questions/38828943/does-sqldataadapterfill-fetch-the-whole-result-set-or-is-it-on-demand */

                int offset = 0;
                Console.WriteLine("_queryAMAll: " + _queryAMAll);
                SqlDataAdapter daAMLoader = new SqlDataAdapter(_queryAMAll, connection);
                dtAM.Rows.Clear();
                rows = daAMLoader.Fill(offset, _batchSize, dtAM);
                while (rows > 0)
                {
                    Console.Write("." + offset.ToString());

                    // Create a batch of rows
                    dtAB.Rows.Clear();
                    foreach (DataRow drAM in dtAM.Rows)
                    {
                        string addr = drAM[0].ToString();

                        Task<HexBigInteger> t = GetAccountBalanceWei(addr);
                        t.Wait(10000);
                        var hexWei = t.Result;
                        decimal balanceWei = Web3.Convert.FromWei(hexWei.Value, UnitConversion.EthUnit.Wei);
                        decimal balanceEther = Web3.Convert.FromWei(hexWei.Value, UnitConversion.EthUnit.Ether);

                        DataRow drAB = dtAB.NewRow();
                        drAB[0] = addr;
                        drAB[1] = DateTime.UtcNow;
                        drAB[2] = balanceWei;
                        drAB[3] = balanceEther;
                        dtAB.Rows.Add(drAB);
                    }

                    // Bulk insert batch of rows
                    sqlBulkCopy.WriteToServer(dtAB);

                    dtAM.Rows.Clear();
                    offset += _batchSize;
                    rows = daAMLoader.Fill(offset, _batchSize, dtAM);
                }
                Console.WriteLine("|");
                connection.Close();
            }
        }

        static public async Task<HexBigInteger> GetAccountBalanceWei(string addr)
        {
            HexBigInteger balanceWei = new HexBigInteger(0);
            bool success = false;
            while (!success)
            {
                try
                {
                    balanceWei = await web3Client.Eth.GetBalance.SendRequestAsync(addr);
                    success = true;
                }
                catch (Exception ex)
                {
                    success = false;
                    Console.WriteLine("(" + addr + ":" + ex.HResult.ToString("X8") + ")");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine();
                    Console.Write("(S5)");
                    Thread.Sleep(5000);
                }
            }
            return balanceWei;
        }

        static Web3 web3Client = GetWeb3Client();

        static Web3 GetWeb3Client()
        {
            //Web3 web3 = new Web3();
            Web3 web3 = new Web3("https://mainnet.infura.io/hZeiirtHOLO11uuyLySi");
            //var ipcClient = new Nethereum.JsonRpc.IpcClient.IpcClient("./geth.ipc");
            //Web3 web3 = new Web3(ipcClient);
            return web3;
        }
    }
}
