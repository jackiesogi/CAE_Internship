using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FW_StorageM.Pages
{
    public class WarehouseTestStockInRecordModel : PageModel
    {
        public List<WarehouseTestStockInRowData> WarehouseTestStockInRowDataList = new List<WarehouseTestStockInRowData>();

        private SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
        {
            
        };
        public async Task<IActionResult> OnPostAsync()
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

                            
                            DateTimeOffset originalTime = reader.GetDateTimeOffset(1);
                            TimeZoneInfo taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");

                            tempWarehouseTestStockInRowData.RecordTime = TimeZoneInfo.ConvertTime(originalTime, taipeiTimeZone);

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
            }
            return Page();
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
            public string SPCode { get; set; }
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
    }
}
