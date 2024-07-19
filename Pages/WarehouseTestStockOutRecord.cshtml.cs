using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FW_StorageM.Pages
{
    public class WarehouseTestStockOutRecordModel : PageModel
    {
        public List<WarehouseTestStockOutRowData> WarehouseTestStockOutRowDataList = new List<WarehouseTestStockOutRowData>();

        private SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
        {
            
        };
        
        public async Task<IActionResult> OnPostAsync()
        {
            string connectionString = builder.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT Id, RecordTime, RecordUser, StockArea, StockLocation, FormworkName, FormworkType, SPCode, Width1, Width2, Width3, Height, Quantity, FormworkDestinationLevel1, FormworkDestinationLevel2, FormworkDestinationLevel3, FormworkDestinationLevel4, Mark FROM WarehouseTestStockOut", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        WarehouseTestStockOutRowDataList.Clear();
                        while (reader.Read())
                        {
                            WarehouseTestStockOutRowData tempWarehouseTestStockOutRowData = new WarehouseTestStockOutRowData();
                            tempWarehouseTestStockOutRowData.Id = (int)reader.GetSqlInt32(0);

                            DateTimeOffset originalTime = reader.GetDateTimeOffset(1);
                            TimeZoneInfo taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");

                            tempWarehouseTestStockOutRowData.RecordTime = TimeZoneInfo.ConvertTime(originalTime, taipeiTimeZone);


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
            public string SPCode { get; set; }
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
    }
}
