//Step 1 - add package NPOI
using Microsoft.SqlServer.Server;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace ClassDataExport
{
    internal class Program
    {
        static DataTable DTForExcelExport = new DataTable();
        static void Main(string[] args)
        {

            string filePath = @"C:\Users\XXXX\source\repos\ClassDataExport\ExcelExport\Project_" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".xlsx";
            string txtFilePath = @"C:\Users\XXX\source\repos\ClassDataExport\ExcelExport\ProductId.txt";
            DataTable dataTable = new DataTable();

            //Data from SQL
            //dataTable = SQLGetData();
            //dataTable = DataFromTxt(txtFilePath);

            //takeData From Oracle;
            OrcaleGetData(dataTable);

            Console.WriteLine("Excel Creation Start");
            ExportToExcel(DTForExcelExport, filePath);
            Console.WriteLine("Excel Created successfully");
            Console.ReadLine();
        }

        public static DataTable SQLGetData()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(@"Connection String"))
            {
                string query = "SELECT ID FROM Product";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);

                connection.Open();
                adapter.Fill(dataTable);
            }
            return dataTable;
        }

        public static DataTable DataFromTxt(string FilePath)
        {
            // Create a DataTable to hold the data
            DataTable dataTable = new DataTable("MyDataTable");
            dataTable.Columns.Add("ID", typeof(string));

            // Read the data from the text file
            try
            {
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    // Read the header line
                    string headerLine = reader.ReadLine();

                    // Read each line in the file
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        // Split the line by commas
                        string[] values = line.Split(',');

                        // Create a new DataRow and populate it
                        if (values.Length > 0)
                        {
                            DataRow row = dataTable.NewRow();
                            row["ID"] = values[0];
                            dataTable.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return dataTable;
        }
      
        public static void OrcaleGetData(DataTable IdData)
        {
            string ProductId = string.Empty;

            int batchSize = 1000;
            int totalRecords = IdData.Rows.Count;
            DataTable dataTable = new DataTable();

            for (int offset = 0; offset < totalRecords; offset += batchSize)
            {
                string productIds = string.Join(",", IdData.Rows   
                                          .Cast<DataRow>()
                                          .Skip(offset) // Skip the processed records
                                          .Take(batchSize) // Take the next 1,000 records
                                          .Select(row => $"'{row["ID"].ToString()}'")); // Please check the pass Id

                if (string.IsNullOrEmpty(productIds))
                {
                    break; // Exit if there are no more IDs to process
                }

                string query = "SELECT * FROM Product WHERE ID IN (" + productIds + ")";
                using (OracleConnection connection = new OracleConnection("Connection String"))
                {
                    using (OracleCommand command = new OracleCommand(query, connection))
                    {
                        using (OracleDataAdapter adapter = new OracleDataAdapter(command))
                        {
                            connection.Open();
                            adapter.Fill(dataTable);
                        }
                    }
                }
                //dataTable = SampleData();
                if(dataTable.Rows.Count > 0) 
                DTForExcelExport.Merge(dataTable);
            }
        }

        public static void ExportToExcel(DataTable dataTable, string filePath)
        {
            // Create a new workbook and a worksheet
            IWorkbook workbook = new XSSFWorkbook();
            ISheet worksheet = workbook.CreateSheet("Sheet1");

            // Create header row
            IRow headerRow = worksheet.CreateRow(0);
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                headerRow.CreateCell(i).SetCellValue(dataTable.Columns[i].ColumnName);
            }

            // Populate the worksheet with data from the DataTable
            // Max Execl 1048576
            int RowCount = dataTable.Rows.Count;

            if (RowCount > 1048575)
            {
                RowCount = 1048575;
                Console.WriteLine("Expected 1048575 --> Excel Rows Count -- {0}", RowCount);
            }

            for (int i = 0; i < RowCount; i++)
            {
                IRow row = worksheet.CreateRow(i + 1);
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    row.CreateCell(j).SetCellValue(dataTable.Rows[i][j].ToString());
                }
            }

            // Write the workbook to a file
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }
        }
    }
}
