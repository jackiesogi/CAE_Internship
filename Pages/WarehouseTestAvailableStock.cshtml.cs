using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FW_StorageM.Pages
{
    public class WarehouseTestAvailableStockModel : PageModel
    {
        // Server Name that will appear on the page
        public string Server { get; set; } = "";

        public List<WarehouseTestStockInRowData> WarehouseTestStockInRowDataList = new List<WarehouseTestStockInRowData>();
        public List<WarehouseTestStockOutRowData> WarehouseTestStockOutRowDataList = new List<WarehouseTestStockOutRowData>();

        public List<WarehouseTestStockInRowData> WarehouseTestWasteStockInRowDataList = new List<WarehouseTestStockInRowData>();
        public List<WarehouseTestStockOutRowData> WarehouseTestWasteStockOutRowDataList = new List<WarehouseTestStockOutRowData>();

        public Dictionary<string, WarehouseTestAvailableStock> WarehouseTestAvailableStockDictionary = new Dictionary<string, WarehouseTestAvailableStock>();
        public Dictionary<string, List<DetailedStockItem>> WarehouseTestAvailableDetailedStockDictionary = new Dictionary<string, List<DetailedStockItem>>();

        public Dictionary<string, List<DetailedStockItem>> WarehouseTestWasteStockDictionary = new Dictionary<string, List<DetailedStockItem>>();


        public SearchFormworkCondition condition = new SearchFormworkCondition();
        
        [BindProperty]
        public string SearchFormworkName { get; set; } = "";
	
	// Add Constructor
	public WarehouseTestAvailableStockModel() : base()
        {
            this.Server = "192.168.122.78,1433";
            this.builder = new SqlConnectionStringBuilder
            {
                //DataSource = Server,
                //InitialCatalog = "internship.db",
                //UserID = "CloudSAa21a6b23",
                //Password = "**********",
                //Encrypt = true,
                //TrustServerCertificate = true
                DataSource = Server, 
                InitialCatalog = "internship_local.db",
                UserID = "sa",
                Password = "**********",
                Encrypt = true,
                TrustServerCertificate = true
            };
        }

        private SqlConnectionStringBuilder builder;
        
        public async Task<IActionResult> OnPostAsync()
        {
            string SearchName = string.Empty;
            SearchName = $"{SearchFormworkName}";
            string connectionString = builder.ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(); 
                string queryStringIn = "SELECT Id, RecordTime, RecordUser, StockArea, StockLocation, FormworkName, FormworkType, SPCode, Width1, Width2, Width3, Height, Quantity, FormworkSourceLevel1, FormworkSourceLevel2, FormworkSourceLevel3, FormworkSourceLevel4, Mark FROM [formwork-warehouseTestStockIn]";
	
                if (!string.IsNullOrEmpty(SearchName))
                    queryStringIn += " WHERE FormworkName = @FormworkName";
                else
                    queryStringIn += " WHERE FormworkName IS NOT NULL";

                // Debug purpose
                // Console.WriteLine("queryStringIn = " + queryStringIn);
                
                using (SqlCommand command = new SqlCommand(queryStringIn, connection))
                {
                    if (!string.IsNullOrEmpty(SearchName))
                    {
                        Console.WriteLine("SearchName = " + SearchName);
                        command.Parameters.AddWithValue("@FormworkName", SearchName);
                    }

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        WarehouseTestStockInRowDataList.Clear();
                        while (await reader.ReadAsync())
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

                            Console.WriteLine("Add FormworkName: " + tempWarehouseTestStockInRowData.FormworkName);
                            
                            if (tempWarehouseTestStockInRowData.StockArea != "廢料")
                            {
                                Console.WriteLine("THIS IS STOCK");
                                WarehouseTestStockInRowDataList.Add(tempWarehouseTestStockInRowData);
                            }
                            else
                            {
                                Console.WriteLine("THIS IS WASTE");
                                WarehouseTestWasteStockInRowDataList.Add(tempWarehouseTestStockInRowData);
                            }
                        }
                    }
                }

                string queryStringOut = "SELECT Id, RecordTime, RecordUser, StockArea, StockLocation, FormworkName, FormworkType, SPCode, Width1, Width2, Width3, Height, Quantity, FormworkDestinationLevel1, FormworkDestinationLevel2, FormworkDestinationLevel3, FormworkDestinationLevel4, Mark FROM [formwork-warehouseTestStockOut]";

                if (!string.IsNullOrEmpty(SearchName))
                    queryStringOut += " Where FormworkName = @FormworkName";
                else
                    queryStringIn += " WHERE FormworkName IS NOT NULL";


                using (SqlCommand command = new SqlCommand(queryStringOut, connection))
                {
                    if (!string.IsNullOrEmpty(SearchName))
                    {
                        command.Parameters.AddWithValue("@FormworkName", SearchName);
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        WarehouseTestStockOutRowDataList.Clear();
                        while (await reader.ReadAsync())
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

                            Console.WriteLine("Add FormworkName: " + tempWarehouseTestStockOutRowData.FormworkName);

                            if (tempWarehouseTestStockOutRowData.StockArea != "廢料")
                            {
                                Console.WriteLine("THIS IS STOCK");
                                WarehouseTestStockOutRowDataList.Add(tempWarehouseTestStockOutRowData);
                            }
                            else
                            {
                                Console.WriteLine("THIS IS WASTE");
                                WarehouseTestWasteStockOutRowDataList.Add(tempWarehouseTestStockOutRowData);
                            }
                        }
                    }
                }
            }

            WarehouseTestAvailableStockDictionary.Clear();
            WarehouseTestAvailableDetailedStockDictionary.Clear();
            WarehouseTestWasteStockDictionary.Clear();

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

            foreach (WarehouseTestStockInRowData tempIn in WarehouseTestStockInRowDataList)
            {
                if (!WarehouseTestAvailableDetailedStockDictionary.ContainsKey(tempIn.FormworkName))
                {
                    WarehouseTestAvailableDetailedStockDictionary[tempIn.FormworkName] = new List<DetailedStockItem>();
                }

                DetailedStockItem tempDetailedStockItem = new DetailedStockItem
                {
                    FormworkName = tempIn.FormworkName,
                    FormworkType = tempIn.FormworkType
                };

                WarehouseItem warehouseItem = new WarehouseItem
                {
                    SPCode = tempIn.SPCode,
                    StockArea = tempIn.StockArea,
                    StockLocation = tempIn.StockLocation,
                    Mark = tempIn.Mark,
                    Quantity = tempIn.Quantity,
                    Width1 = tempIn.Width1,
                    Width2 = tempIn.Width2,
                    Width3 = tempIn.Width3,
                    Height = tempIn.Height
                };

                tempDetailedStockItem.WarehouseItems.Add(warehouseItem);

                WarehouseTestAvailableDetailedStockDictionary[tempIn.FormworkName].Add(tempDetailedStockItem);
            }

            foreach (WarehouseTestStockOutRowData tempOut in WarehouseTestStockOutRowDataList)
            {
                if (!WarehouseTestAvailableDetailedStockDictionary.ContainsKey(tempOut.FormworkName))
                {
                    WarehouseTestAvailableDetailedStockDictionary[tempOut.FormworkName] = new List<DetailedStockItem>();
                }

                DetailedStockItem tempDetailedStockItem = new DetailedStockItem
                {
                    FormworkName = tempOut.FormworkName,
                    FormworkType = tempOut.FormworkType
                };

                WarehouseItem warehouseItem = new WarehouseItem
                {
                    SPCode = tempOut.SPCode,
                    StockArea = tempOut.StockArea,
                    StockLocation = tempOut.StockLocation,
                    Mark = tempOut.Mark,
                    Quantity = tempOut.Quantity * (-1),
                    Width1 = tempOut.Width1,
                    Width2 = tempOut.Width2,
                    Width3 = tempOut.Width3,
                    Height = tempOut.Height
                };

                tempDetailedStockItem.WarehouseItems.Add(warehouseItem);

                WarehouseTestAvailableDetailedStockDictionary[tempOut.FormworkName].Add(tempDetailedStockItem);
            }





            foreach (WarehouseTestStockInRowData tempIn in WarehouseTestWasteStockInRowDataList)
            {
                if (!WarehouseTestWasteStockDictionary.ContainsKey(tempIn.FormworkName))
                {
                    WarehouseTestWasteStockDictionary[tempIn.FormworkName] = new List<DetailedStockItem>();
                }

                DetailedStockItem tempDetailedStockItem = new DetailedStockItem
                {
                    FormworkName = tempIn.FormworkName,
                    FormworkType = tempIn.FormworkType
                };

                WarehouseItem warehouseItem = new WarehouseItem
                {
                    SPCode = tempIn.SPCode,
                    StockArea = tempIn.StockArea,
                    StockLocation = tempIn.StockLocation,
                    Mark = tempIn.Mark,
                    Quantity = tempIn.Quantity,
                    Width1 = tempIn.Width1,
                    Width2 = tempIn.Width2,
                    Width3 = tempIn.Width3,
                    Height = tempIn.Height
                };

                tempDetailedStockItem.WarehouseItems.Add(warehouseItem);

                WarehouseTestWasteStockDictionary[tempIn.FormworkName].Add(tempDetailedStockItem);
            }

            foreach (WarehouseTestStockOutRowData tempOut in WarehouseTestWasteStockOutRowDataList)
            {
                if (!WarehouseTestWasteStockDictionary.ContainsKey(tempOut.FormworkName))
                {
                    WarehouseTestWasteStockDictionary[tempOut.FormworkName] = new List<DetailedStockItem>();
                }

                DetailedStockItem tempDetailedStockItem = new DetailedStockItem
                {
                    FormworkName = tempOut.FormworkName,
                    FormworkType = tempOut.FormworkType
                };

                WarehouseItem warehouseItem = new WarehouseItem
                {
                    SPCode = tempOut.SPCode,
                    StockArea = tempOut.StockArea,
                    StockLocation = tempOut.StockLocation,
                    Mark = tempOut.Mark,
                    Quantity = tempOut.Quantity * (-1),
                    Width1 = tempOut.Width1,
                    Width2 = tempOut.Width2,
                    Width3 = tempOut.Width3,
                    Height = tempOut.Height
                };

                tempDetailedStockItem.WarehouseItems.Add(warehouseItem);

                WarehouseTestWasteStockDictionary[tempOut.FormworkName].Add(tempDetailedStockItem);
            }

            // Initialize a variable to hold the latest update time
            DateTimeOffset latestUpdateTime = DateTimeOffset.MinValue;

            // Check the latest update time in the Stock In list
            if (WarehouseTestStockInRowDataList.Any())
            {
                var maxInTime = WarehouseTestStockInRowDataList.Max(x => x.RecordTime);
                if (maxInTime > latestUpdateTime)
                {
                    latestUpdateTime = maxInTime;
                }
            }

            // Check the latest update time in the Stock Out list
            if (WarehouseTestStockOutRowDataList.Any())
            {
                var maxOutTime = WarehouseTestStockOutRowDataList.Max(x => x.RecordTime);
                if (maxOutTime > latestUpdateTime)
                {
                    latestUpdateTime = maxOutTime;
                }
            }

            // Find the Taipei Time Zone
            TimeZoneInfo taipeiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");

            // Convert latestUpdateTime to Taipei Time Zone
            DateTimeOffset taipeiTime = TimeZoneInfo.ConvertTime(latestUpdateTime, taipeiTimeZone);

            LatestUpdateTime = taipeiTime;


            // Now, latestUpdateTime contains the most recent update time across all formworks


            IsPostRequestHandled = true;

            return Page();
        }

        public Dictionary<string, WarehouseTestAvailableStock> GetAvailableStockDictionary()
        {
            return WarehouseTestAvailableStockDictionary;
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

        public class DetailedStockItem
        {
            public string FormworkName { get; set; } = string.Empty;
            public string FormworkType { get; set; } = string.Empty;
            public List<WarehouseItem> WarehouseItems { get; set; } = new List<WarehouseItem>();
        }

        public class WarehouseItem
        {
            public string SPCode { get; set; } = string.Empty;
            public string StockArea { get; set; } = string.Empty;
            public string StockLocation { get; set; } = string.Empty;
            public string Mark { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public int Width1 { get; set; }
            public int Width2 { get; set; }
            public int Width3 { get; set; }
            public int Height { get; set; }
        }

        public bool IsPostRequestHandled { get; set; } = false;
        public DateTimeOffset LatestUpdateTime { get; set; } = DateTimeOffset.MinValue;



        public class SearchFormworkCondition
        {
            public string FormworkName { get; set; } = "";
            //public string StartTime { get; set; }
            //public string EndTime { get; set; }
        }
    }
}
