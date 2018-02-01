using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtherToSQL1
{
    class Program
    {
        static void Main(string[] args)
        {
            string queryString = "SELECT OrderID, CustomerID FROM dbo.Orders;";

            using (SqlConnection connection =
                       new SqlConnection(connectionString))
            {
                SqlCommand command =
                    new SqlCommand(queryString, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Call Read before accessing data.
                while (reader.Read())
                {
                    ReadSingleRow((IDataRecord)reader);
                }

                // Call Close when done reading.
                reader.Close();
            }
        }
    }
}
