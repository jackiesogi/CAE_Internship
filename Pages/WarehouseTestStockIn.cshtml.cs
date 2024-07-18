using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;

namespace FW_StorageM.Pages
{
    public class WarehouseTestStockInModel : PageModel
    {
        public List<WarehouseTestStockInRowData> WarehouseTestStockInRowDataList = new List<WarehouseTestStockInRowData>();
        public List<WarehouseTestStockInRowData> invalidRows = new List<WarehouseTestStockInRowData>();

        private SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
        {
            
        };

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int startRow = 2;
                    int endRow = worksheet.Dimension.Rows;

                    WarehouseTestStockInRowDataList.Clear();
                    invalidRows.Clear();

                    for (int row = startRow; row <= endRow; row++)
                    {
                        WarehouseTestStockInRowData tempWarehouseTestStockInRowData = new WarehouseTestStockInRowData();
                        tempWarehouseTestStockInRowData.StockArea = worksheet.Cells[row, 2].GetValue<string>() ?? "";
                        tempWarehouseTestStockInRowData.StockLocation = worksheet.Cells[row, 3].GetValue<string>() ?? "";
                        tempWarehouseTestStockInRowData.FormworkName = worksheet.Cells[row, 4].GetValue<string>() ?? "";
                        tempWarehouseTestStockInRowData.FormworkType = worksheet.Cells[row, 5].GetValue<string>() ?? "";
                        tempWarehouseTestStockInRowData.SPCode = worksheet.Cells[row, 6].GetValue<string>() ?? "";
                        tempWarehouseTestStockInRowData.Width1 = worksheet.Cells[row, 7].GetValue<int?>() ?? 0;
                        tempWarehouseTestStockInRowData.Width2 = worksheet.Cells[row, 8].GetValue<int?>() ?? 0;
                        tempWarehouseTestStockInRowData.Width3 = worksheet.Cells[row, 9].GetValue<int?>() ?? 0;
                        tempWarehouseTestStockInRowData.Height = worksheet.Cells[row, 10].GetValue<int?>() ?? 0;
                        tempWarehouseTestStockInRowData.Quantity = worksheet.Cells[row, 11].GetValue<int>();
                        tempWarehouseTestStockInRowData.FormworkSourceLevel1 = worksheet.Cells[row, 12].GetValue<string>() ?? "";
                        /*
                        tempWarehouseTestStockInRowData.FormworkSourceLevel2 = worksheet.Cells[row, 13].GetValue<string>() ?? "";
                        tempWarehouseTestStockInRowData.FormworkSourceLevel3 = worksheet.Cells[row, 14].GetValue<string>() ?? "";
                        tempWarehouseTestStockInRowData.FormworkSourceLevel4 = worksheet.Cells[row, 15].GetValue<string>() ?? "";
                        */
                        tempWarehouseTestStockInRowData.FormworkSourceLevel2 = "";
                        tempWarehouseTestStockInRowData.FormworkSourceLevel3 = "";
                        tempWarehouseTestStockInRowData.FormworkSourceLevel4 = "";
                        tempWarehouseTestStockInRowData.Mark = worksheet.Cells[row, 13].GetValue<string>() ?? "";

                        if ((
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockInRowData.FormworkName) &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockInRowData.FormworkType) &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockInRowData.SPCode) &&
                            tempWarehouseTestStockInRowData.Width1 <= 0 &&
                            tempWarehouseTestStockInRowData.Width2 <= 0 &&
                            tempWarehouseTestStockInRowData.Width3 <= 0 &&
                            tempWarehouseTestStockInRowData.Height <= 0 &&
                            (tempWarehouseTestStockInRowData.Quantity <= 0 || tempWarehouseTestStockInRowData.Quantity == null) &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockInRowData.FormworkSourceLevel1) &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockInRowData.FormworkSourceLevel2) &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockInRowData.FormworkSourceLevel3) &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockInRowData.FormworkSourceLevel4) &&
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockInRowData.Mark)) ||
                            string.IsNullOrWhiteSpace(tempWarehouseTestStockInRowData.FormworkName) ||
                            (tempWarehouseTestStockInRowData.Quantity <= 0 || tempWarehouseTestStockInRowData.Quantity == null))
                        {
                            // Add the row to the list of invalid rows
                            invalidRows.Add(tempWarehouseTestStockInRowData);
                        }
                        else
                        {
                            // Add valid rows to the main list
                            WarehouseTestStockInRowDataList.Add(tempWarehouseTestStockInRowData);
                        }


                    }
                    string connectionString = builder.ConnectionString;
                    using SqlConnection connection = new SqlConnection(connectionString);
                    connection.Open();
                    string RecordUser = User.Identity.Name;
                    //string RecordUser = "�ܮw�޲z��";
                    // Insert new record
                    for (int i = 0; i < WarehouseTestStockInRowDataList.Count; i++)
                    {
                        string StockArea = WarehouseTestStockInRowDataList[i].StockArea.Replace(" ", "").ToUpper(); ;
                        string StockLocation = WarehouseTestStockInRowDataList[i].StockLocation.Replace(" ", "").ToUpper();
                        string FormworkName = WarehouseTestStockInRowDataList[i].FormworkName.Replace(" ", "").ToUpper();
                        string FormworkType = WarehouseTestStockInRowDataList[i].FormworkType.Replace(" ", "").ToUpper();
                        string SPCode = WarehouseTestStockInRowDataList[i].SPCode.Replace(" ", "").ToUpper();
                        int Width1 = WarehouseTestStockInRowDataList[i].Width1;
                        int Width2 = WarehouseTestStockInRowDataList[i].Width2;
                        int Width3 = WarehouseTestStockInRowDataList[i].Width3;
                        int Height = WarehouseTestStockInRowDataList[i].Height;
                        int Quantity = WarehouseTestStockInRowDataList[i].Quantity;
                        string FormworkSourceLevel1 = WarehouseTestStockInRowDataList[i].FormworkSourceLevel1.Replace(" ", "").ToUpper();
                        string FormworkSourceLevel2 = WarehouseTestStockInRowDataList[i].FormworkSourceLevel2.Replace(" ", "").ToUpper();
                        string FormworkSourceLevel3 = WarehouseTestStockInRowDataList[i].FormworkSourceLevel3.Replace(" ", "").ToUpper();
                        string FormworkSourceLevel4 = WarehouseTestStockInRowDataList[i].FormworkSourceLevel4.Replace(" ", "").ToUpper();
                        string Mark = WarehouseTestStockInRowDataList[i].Mark.Replace(" ", "").ToUpper();


                        string insertQueryToWarehouseTestStockIn = "INSERT INTO WarehouseTestStockIn(RecordUser, StockArea, StockLocation, FormworkName, FormworkType, SPCode, Width1, Width2, Width3, Height, Quantity, FormworkSourceLevel1, FormworkSourceLevel2, FormworkSourceLevel3, FormworkSourceLevel4, Mark) " +
                                                        "VALUES(@RecordUser, @StockArea, @StockLocation, @FormworkName, @FormworkType, @SPCode, @Width1, @Width2, @Width3, @Height, @Quantity, @FormworkSourceLevel1, @FormworkSourceLevel2, @FormworkSourceLevel3, @FormworkSourceLevel4, @Mark)";

                        using SqlCommand commandToWarehouseTestStockIn = new SqlCommand(insertQueryToWarehouseTestStockIn, connection);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@RecordUser", RecordUser);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@StockArea", StockArea);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@StockLocation", StockLocation);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@FormworkName", FormworkName);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@FormworkType", FormworkType);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@SPCode", SPCode);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@Width1", Width1);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@Width2", Width2);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@Width3", Width3);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@Height", Height);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@Quantity", Quantity);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@FormworkSourceLevel1", FormworkSourceLevel1);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@FormworkSourceLevel2", FormworkSourceLevel2);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@FormworkSourceLevel3", FormworkSourceLevel3);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@FormworkSourceLevel4", FormworkSourceLevel4);
                        commandToWarehouseTestStockIn.Parameters.AddWithValue("@Mark", Mark);
                        commandToWarehouseTestStockIn.ExecuteNonQuery();
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
        public class WarehouseTestStockInRowData
        {
            public int Id { get; set; }
            public DateTimeOffset RecordTime { get; set; }
            public string RecordUser { get; set; }
            public string StockArea { get; set; }
            public string StockLocation { get; set; }
            public string FormworkName { get; set; }
            public string FormworkType { get; set; }
            public string SPCode { get; set; }
            public int Width1 { get; set; }
            public int Width2 { get; set; }
            public int Width3 { get; set; }
            public int Height { get; set; }
            public int Quantity { get; set; }
            public string FormworkSourceLevel1 { get; set; }
            public string FormworkSourceLevel2 { get; set; }
            public string FormworkSourceLevel3 { get; set; }
            public string FormworkSourceLevel4 { get; set; }
            public string Mark { get; set; }
        }
    }
}
