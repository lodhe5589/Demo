using System;
using System.Data;
using System.Data.SqlClient;

namespace TableTypeInLine
{
    internal class Program
    {
        static void Main()
        {
            try
            {
                string connectionString = "ConnectString";


                DataTable myTable = new DataTable();
                myTable.Columns.Add("Id", typeof(int));


                myTable.Rows.Add(1);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlQuery = @"SELECT * FROM @MyTableParam;";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                     
                        SqlParameter tvpParam = command.Parameters.AddWithValue("@MyTableParam", myTable);
                        tvpParam.SqlDbType = SqlDbType.Structured;
                        tvpParam.TypeName = "MyTableType";

                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Id: {reader["Id"]}");
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
