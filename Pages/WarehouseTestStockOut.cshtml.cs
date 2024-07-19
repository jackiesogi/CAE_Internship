using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using System.Collections.Generic;
using static FW_StorageM.Pages.WarehouseTestAvailableStockModel;

namespace FW_StorageM.Pages
{
    public class WarehouseTestStockOutModel : PageModel
    {
        public List<WarehouseTestStockInRowData> WarehouseTestStockInRowDataList = new List<WarehouseTestStockInRowData>();
        public List<WarehouseTestStockOutRowData> WarehouseTestStockOutRowDataList = new List<WarehouseTestStockOutRowData>();
        public List<WarehouseTestStockOutRowData> invalidRows = new List<WarehouseTestStockOutRowData>();

        public Dictionary<string, WarehouseTestAvailableStock> WarehouseTestAvailableStockDictionary = new Dictionary<string, WarehouseTestAvailableStock>();

        private SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
        {
            
        };

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {

            string connectionString = builder.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT Id, RecordTime, RecordUser, StockArea, StockLocation, FormworkName, FormworkType, SPCode, Width1, Width2, Width3, Height, Quantity, FormworkSourceLevel1, FormworkSourceLevel2, FormworkSourceLevel3, FormworkSourceLevel4, Mark FROM WarehouseTestStockIn", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        WarehouseTestStockInRowDataList.Clear();
                        while (reader.Read())
                        {
                            WarehouseTestStockInRowData tempWarehouseTestStockInRowData = new WarehouseTestStockInRowData();
                            tempWarehouseTestStockInRowData.Id = (int)reader.GetSqlInt32(0);
                            tempWarehouseTestStockInRowData.RecordTime = reader.GetDateTimeOffset(1);
                            tempWarehouseTestStockInRowData.RecordUser = reader.GetString(2);
                            tempWarehouseTestStockInRowData.StockArea = reader.GetString(3);
                            tempWarehouseTestStockInRowData.StockLocation = reader.GetString(4);
                            tempWarehouseTestStockInRowData.FormworkName = reader.GetString(5);
                            tempWarehouseTestStockInRowData.FormworkType = reader.GetString(6);
                            tempWarehouseTestStockInRowData.SPCode = reader.GetString(7);
                            tempWarehouseTestStockInRowData.Width1 = (int)reader.GetSqlInt32(8);
                            tempWarehouseTestStockInRowData.Width2 = (int)reader.GetSqlInt32(9);
                            tempWarehouseTestStockInRowData.Width3 = (int)reader.GetSqlInt32(10);
                            tempWarehouseTestStockInRowData.Height = (int)reader.GetSqlInt32(11);
                            tempWarehouseTestStockInRowData.Quantity = (int)reader.GetSqlInt32(12);
                            tempWarehouseTestStockInRowData.FormworkSourceLevel1 = reader.GetString(13);
                            tempWarehouseTestStockInRowData.FormworkSourceLevel2 = reader.GetString(14);
                            tempWarehouseTestStockInRowData.FormworkSourceLevel3 = reader.GetString(15);
                            tempWarehouseTestStockInRowData.FormworkSourceLevel4 = reader.GetString(16);
                            tempWarehouseTestStockInRowData.Mark = reader.GetString(17);

                            WarehouseTestStockInRowDataList.Add(tempWarehouseTestStockInRowData);
                        }
                    }
                }


                using (SqlCommand command = new SqlCommand("SELECT Id, RecordTime, RecordUser, StockArea, StockLocation, FormworkName, FormworkType, SPCode, Width1, Width2, Width3, Height, Quantity, FormworkDestinationLevel1, FormworkDestinationLevel2, FormworkDestinationLevel3, FormworkDestinationLevel4, Mark FROM WarehouseTestStockOut", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        WarehouseTestStockOutRowDataList.Clear();
                        while (reader.Read())
                        {
                            WarehouseTestStockOutRowData tempWarehouseTestStockOutRowData = new WarehouseTestStockOutRowData();
                            tempWarehouseTestStockOutRowData.Id = (int)reader.GetSqlInt32(0);
                            tempWarehouseTestStockOutRowData.RecordTime = reader.GetDateTimeOffset(1);
                            tempWarehouseTestStockOutRowData.RecordUser = reader.GetString(2);
                            tempWarehouseTestStockOutRowData.StockArea = reader.GetString(3);
                            tempWarehouseTestStockOutRowData.StockLocation = reader.GetString(4);
                            tempWarehouseTestStockOutRowData.FormworkName = reader.GetString(5);
                            tempWarehouseTestStockOutRowData.FormworkType = reader.GetString(6);
                            tempWarehouseTestStockOutRowData.SPCode = reader.GetString(7);
                            tempWarehouseTestStockOutRowData.Width1 = (int)reader.GetSqlInt32(8);
                            tempWarehouseTestStockOutRowData.Width2 = (int)reader.GetSqlInt32(9);
                            tempWarehouseTestStockOutRowData.Width3 = (int)reader.GetSqlInt32(10);
                            tempWarehouseTestStockOutRowData.Height = (int)reader.GetSqlInt32(11);
                            tempWarehouseTestStockOutRowData.Quantity = (int)reader.GetSqlInt32(12);
                            tempWarehouseTestStockOutRowData.FormworkDestinationLevel1 = reader.GetString(13);
                            tempWarehouseTestStockOutRowData.FormworkDestinationLevel2 = reader.GetString(14);
                            tempWarehouseTestStockOutRowData.FormworkDestinationLevel3 = reader.GetString(15);
                            tempWarehouseTestStockOutRowData.FormworkDestinationLevel4 = reader.GetString(16);
                            tempWarehouseTestStockOutRowData.Mark = reader.GetString(17);

                            WarehouseTestStockOutRowDataList.Add(tempWarehouseTestStockOutRowData);
                        }
                    }
                }
            }

            WarehouseTestAvailableStockDictionary.Clear();

            foreach (WarehouseTestStockInRowData tempIn in WarehouseTestStockInRowDataList)
            {
                if (!WarehouseTestAvailableStockDictionary.ContainsKey(tempIn.FormworkName))
                {
                    WarehouseTestAvailableStock tempWarehouseTestAvailableStock = new WarehouseTestAvailableStock();
                    tempWarehouseTestAvailableStock.FormworkName = tempIn.FormworkName;
                    tempWarehouseTestAvailableStock.FormworkType = tempIn.FormworkType;
                    tempWarehouseTestAvailableStock.SPCode = tempIn.SPCode;
                    tempWarehouseTestAvailableStock.Width1 = tempIn.Width1;
                    tempWarehouseTestAvailableStock.Width2 = tempIn.Width2;
                    tempWarehouseTestAvailableStock.Width3 = tempIn.Width3;
                    tempWarehouseTestAvailableStock.Height = tempIn.Height;
                    tempWarehouseTestAvailableStock.Quantity = tempIn.Quantity;

                    WarehouseTestAvailableStockDictionary.Add(tempIn.FormworkName, tempWarehouseTestAvailableStock);
                }
                else
                {
                    WarehouseTestAvailableStockDictionary[tempIn.FormworkName].Quantity += tempIn.Quantity;
                }
            }

            foreach (WarehouseTestStockOutRowData tempOut in WarehouseTestStockOutRowDataList)
            {
                if (!WarehouseTestAvailableStockDictionary.ContainsKey(tempOut.FormworkName))
                {
                    WarehouseTestAvailableStock tempWarehouseTestAvailableStock = new WarehouseTestAvailableStock();
                    tempWarehouseTestAvailableStock.FormworkName = tempOut.FormworkName;
                    tempWarehouseTestAvailableStock.FormworkType = tempOut.FormworkType;
                    tempWarehouseTestAvailableStock.SPCode = tempOut.SPCode;
                    tempWarehouseTestAvailableStock.Width1 = tempOut.Width1;
                    tempWarehouseTestAvailableStock.Width2 = tempOut.Width2;
                    tempWarehouseTestAvailableStock.Width3 = tempOut.Width3;
                    tempWarehouseTestAvailableStock.Height = tempOut.Height;
                    tempWarehouseTestAvailableStock.Quantity = tempOut.Quantity * (-1);

                    WarehouseTestAvailableStockDictionary.Add(tempOut.FormworkName, tempWarehouseTestAvailableStock);
                }
                else
                {
                    WarehouseTestAvailableStockDictionary[tempOut.FormworkName].Quantity -= tempOut.Quantity;
                }
            }


            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int startRow = 2;
                    int endRow = worksheet.Dimension.Rows;

                    WarehouseTestStockOutRowDataList.Clear();
                    invalidRows.Clear();

                    

                    for (int row = startRow; row <= endRow; row++)
                    {
                        WarehouseTestStockOutRowData tempWarehouseTestStockOutRowData = new WarehouseTestStockOutRowData();
                        tempWarehouseTestStockOutRowData.StockArea = worksheet.Cells[row, 2].GetValue<string>() ?? "";
                        tempWarehouseTestStockOutRowData.StockLocation = worksheet.Cells[row, 3].GetValue<string>() ?? "";
                        tempWarehouseTestStockOutRowData.FormworkName = worksheet.Cells[row, 4].GetValue<string>() ?? "";
                        tempWarehouseTestStockOutRowData.FormworkType = worksheet.Cells[row, 5].GetValue<string>() ?? "";
                        tempWarehouseTestStockOutRowData.SPCode = worksheet.Cells[row, 6].GetValue<string>() ?? "";
                        tempWarehouseTestStockOutRowData.Width1 = worksheet.Cells[row, 7].GetValue<int?>() ?? 0;
                        tempWarehouseTestStockOutRowData.Width2 = worksheet.Cells[row, 8].GetValue<int?>() ?? 0;
                        tempWarehouseTestStockOutRowData.Width3 = worksheet.Cells[row, 9].GetValue<int?>() ?? 0;
                        tempWarehouseTestStockOutRowData.Height = worksheet.Cells[row, 10].GetValue<int?>() ?? 0;
                        tempWarehouseTestStockOutRowData.Quantity = worksheet.Cells[row, 11].GetValue<int>();
                        tempWarehouseTestStockOutRowData.FormworkDestinationLevel1 = worksheet.Cells[row, 12].GetValue<string>() ?? "";
                        /*
                        tempWarehouseTestStockOutRowData.FormworkDestinationLevel2 = worksheet.Cells[row, 13].GetValue<string>() ?? "";
                        tempWarehouseTestStockOutRowData.FormworkDestinationLevel3 = worksheet.Cells[row, 14].GetValue<string>() ?? "";
                        tempWarehouseTestStockOutRowData.FormworkDestinationLevel4 = worksheet.Cells[row, 15].GetValue<string>() ?? "";
                        */
                        tempWarehouseTestStockOutRowData.FormworkDestinationLevel2 = "";
                        tempWarehouseTestStockOutRowData.FormworkDestinationLevel3 = "";
                        tempWarehouseTestStockOutRowData.FormworkDestinationLevel4 = "";
                        tempWarehouseTestStockOutRowData.Mark = worksheet.Cells[row, 13].GetValue<string>() ?? "";


                        tempWarehouseTestStockOutRowData.StockArea = tempWarehouseTestStockOutRowData.StockArea.Replace(" ", "").ToUpper(); ;
                        tempWarehouseTestStockOutRowData.StockLocation = tempWarehouseTestStockOutRowData.StockLocation.Replace(" ", "").ToUpper(); ;
                        tempWarehouseTestStockOutRowData.FormworkName = tempWarehouseTestStockOutRowData.FormworkName.Replace(" ", "").ToUpper();
                        tempWarehouseTestStockOutRowData.FormworkType = tempWarehouseTestStockOutRowData.FormworkType.Replace(" ", "").ToUpper();
                        tempWarehouseTestStockOutRowData.SPCode = tempWarehouseTestStockOutRowData.SPCode.Replace(" ", "").ToUpper();
                        tempWarehouseTestStockOutRowData.FormworkDestinationLevel1 = tempWarehouseTestStockOutRowData.FormworkDestinationLevel1.Replace(" ", "").ToUpper();
                        tempWarehouseTestStockOutRowData.FormworkDestinationLevel2 = tempWarehouseTestStockOutRowData.FormworkDestinationLevel2.Replace(" ", "").ToUpper();
                        tempWarehouseTestStockOutRowData.FormworkDestinationLevel3 = tempWarehouseTestStockOutRowData.FormworkDestinationLevel3.Replace(" ", "").ToUpper();
                        tempWarehouseTestStockOutRowData.FormworkDestinationLevel4 = tempWarehouseTestStockOutRowData.FormworkDestinationLevel4.Replace(" ", "").ToUpper();
                        tempWarehouseTestStockOutRowData.Mark = tempWarehouseTestStockOutRowData.Mark.Replace(" ", "").ToUpper();



                        if (!WarehouseTestAvailableStockDictionary.ContainsKey(tempWarehouseTestStockOutRowData.FormworkName) )
                        {
                            invalidRows.Add(tempWarehouseTestStockOutRowData);
                            Console.WriteLine("1ContainsKey");
                        }
                        else if (tempWarehouseTestStockOutRowData.Quantity > WarehouseTestAvailableStockDictionary[tempWarehouseTestStockOutRowData.FormworkName].Quantity) 
                        {
                            invalidRows.Add(tempWarehouseTestStockOutRowData);
                            Console.WriteLine("2Quantity");
                        }
                        else if (string.IsNullOrWhiteSpace(tempWarehouseTestStockOutRowData.FormworkName) &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockOutRowData.FormworkType) &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockOutRowData.SPCode) &&
                            tempWarehouseTestStockOutRowData.Width1 <= 0 &&
                            tempWarehouseTestStockOutRowData.Width2 <= 0 &&
                            tempWarehouseTestStockOutRowData.Width3 <= 0 &&
                            tempWarehouseTestStockOutRowData.Height <= 0 &&
                            tempWarehouseTestStockOutRowData.Quantity <= 0 &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockOutRowData.FormworkDestinationLevel1) &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockOutRowData.FormworkDestinationLevel2) &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockOutRowData.FormworkDestinationLevel3) &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockOutRowData.FormworkDestinationLevel4) &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockOutRowData.Mark))
                        {
                            invalidRows.Add(tempWarehouseTestStockOutRowData);
                            Console.WriteLine("3EmptyRow");

                        } else
                        {
                            WarehouseTestStockOutRowDataList.Add(tempWarehouseTestStockOutRowData);
                        }
                            
                    }

                    using SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    //string RecordUser = "�ܮw�޲z��";
                    string RecordUser = User.Identity.Name;
                    // Insert new record
                    for (int i = 0; i < WarehouseTestStockOutRowDataList.Count; i++)
                    {
                        string StockArea = WarehouseTestStockOutRowDataList[i].StockArea.Replace(" ", "").ToUpper(); ;
                        string StockLocation = WarehouseTestStockOutRowDataList[i].StockLocation.Replace(" ", "").ToUpper(); ;
                        string FormworkName = WarehouseTestStockOutRowDataList[i].FormworkName.Replace(" ", "").ToUpper();
                        string FormworkType = WarehouseTestStockOutRowDataList[i].FormworkType.Replace(" ", "").ToUpper();
                        string SPCode = WarehouseTestStockOutRowDataList[i].SPCode.Replace(" ", "").ToUpper();
                        int Width1 = WarehouseTestStockOutRowDataList[i].Width1;
                        int Width2 = WarehouseTestStockOutRowDataList[i].Width2;
                        int Width3 = WarehouseTestStockOutRowDataList[i].Width3;
                        int Height = WarehouseTestStockOutRowDataList[i].Height;
                        int Quantity = WarehouseTestStockOutRowDataList[i].Quantity;
                        string FormworkDestinationLevel1 = WarehouseTestStockOutRowDataList[i].FormworkDestinationLevel1.Replace(" ", "").ToUpper();
                        string FormworkDestinationLevel2 = WarehouseTestStockOutRowDataList[i].FormworkDestinationLevel2.Replace(" ", "").ToUpper();
                        string FormworkDestinationLevel3 = WarehouseTestStockOutRowDataList[i].FormworkDestinationLevel3.Replace(" ", "").ToUpper();
                        string FormworkDestinationLevel4 = WarehouseTestStockOutRowDataList[i].FormworkDestinationLevel4.Replace(" ", "").ToUpper();
                        string Mark = WarehouseTestStockOutRowDataList[i].Mark.Replace(" ", "").ToUpper();


                        string insertQueryToWarehouseTestStockOut = "INSERT INTO WarehouseTestStockOut(RecordUser, StockArea, StockLocation, FormworkName, FormworkType, SPCode, Width1, Width2, Width3, Height, Quantity, FormworkDestinationLevel1, FormworkDestinationLevel2, FormworkDestinationLevel3, FormworkDestinationLevel4, Mark) " +
                                                        "VALUES(@RecordUser, @StockArea, @StockLocation, @FormworkName, @FormworkType, @SPCode, @Width1, @Width2, @Width3, @Height, @Quantity, @FormworkDestinationLevel1, @FormworkDestinationLevel2, @FormworkDestinationLevel3, @FormworkDestinationLevel4, @Mark)";

                        using SqlCommand commandToWarehouseTestStockOut = new SqlCommand(insertQueryToWarehouseTestStockOut, connection);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@RecordUser", RecordUser);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@StockArea", StockArea);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@StockLocation", StockLocation);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@FormworkName", FormworkName);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@FormworkType", FormworkType);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@SPCode", SPCode);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@Width1", Width1);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@Width2", Width2);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@Width3", Width3);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@Height", Height);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@Quantity", Quantity);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@FormworkDestinationLevel1", FormworkDestinationLevel1);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@FormworkDestinationLevel2", FormworkDestinationLevel2);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@FormworkDestinationLevel3", FormworkDestinationLevel3);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@FormworkDestinationLevel4", FormworkDestinationLevel4);
                        commandToWarehouseTestStockOut.Parameters.AddWithValue("@Mark", Mark);
                        commandToWarehouseTestStockOut.ExecuteNonQuery();
                    }
                }
                TempData["UploadMessage"] = "已成功上傳";
            }
            else
            {
                TempData["UploadMessage"] = "檔案格式有誤";
            }
            return Page();
        }
        public class WarehouseTestStockOutRowData
        {
            public int Id { get; set; }
            public DateTimeOffset RecordTime { get; set; }
            public string RecordUser { get; set; } = string.Empty;
            public string StockArea { get; set; } = string.Empty;
            public string StockLocation { get; set; } = string.Empty;
            public string FormworkName { get; set; } = string.Empty;
            public string FormworkType { get; set; } = string.Empty;
            public string SPCode { get; set; } = string.Empty;
            public int Width1 { get; set; }
            public int Width2 { get; set; }
            public int Width3 { get; set; }
            public int Height { get; set; }
            public int Quantity { get; set; }
            public string FormworkDestinationLevel1 { get; set; } = string.Empty;
            public string FormworkDestinationLevel2 { get; set; } = string.Empty;
            public string FormworkDestinationLevel3 { get; set; } = string.Empty;
            public string FormworkDestinationLevel4 { get; set; } = string.Empty;
            public string Mark { get; set; } = string.Empty;
        }

        public class WarehouseTestStockInRowData
        {
            public int Id { get; set; }
            public DateTimeOffset RecordTime { get; set; }
            public string RecordUser { get; set; } = string.Empty;
            public string StockArea { get; set; } = string.Empty;
            public string StockLocation { get; set; } = string.Empty;
            public string FormworkName { get; set; } = string.Empty;
            public string FormworkType { get; set; } = string.Empty;
            public string SPCode { get; set; } = string.Empty;
            public int Width1 { get; set; }
            public int Width2 { get; set; }
            public int Width3 { get; set; }
            public int Height { get; set; }
            public int Quantity { get; set; }
            public string FormworkSourceLevel1 { get; set; } = string.Empty;
            public string FormworkSourceLevel2 { get; set; } = string.Empty;
            public string FormworkSourceLevel3 { get; set; } = string.Empty;
            public string FormworkSourceLevel4 { get; set; } = string.Empty;
            public string Mark { get; set; } = string.Empty;
        }

        public class WarehouseTestAvailableStock
        {
            public string StockArea { get; set; } = string.Empty;
            public string StockLocation { get; set; } = string.Empty;
            public string FormworkName { get; set; } = string.Empty;
            public string FormworkType { get; set; } = string.Empty;
            public string SPCode { get; set; } = string.Empty;
            public int Width1 { get; set; }
            public int Width2 { get; set; }
            public int Width3 { get; set; }
            public int Height { get; set; }
            public int Quantity { get; set; }
            public string Mark { get; set; } = string.Empty;
        }

    }
}
