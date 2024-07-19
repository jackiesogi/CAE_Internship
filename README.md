# 鋁模板庫存管理系統

## 建置專案

#### 先 clone 這個 repository
```sh
git clone https://github.com/jackiesogi/CAE_Internship.git
cd CAE_Internship
```

#### 確保有安裝 .NET SDK
```bash
sudo apt install dotnet-sdk-8.0    # Ubuntu
winget install Microsoft.DotNet.SDK.8  # Windows
```
如果Runtime是其他版本, 請先在`Formwork-Warehouse-Razor.csproj`檔案內修改`<TargetFramework>`

#### 安裝所需套件
```powershell
dotnet add package Microsoft.Data.SqlClient
dotnet add package EPPlus 
dotnet add package Microsoft.EntityFrameworkCore  # 如果使用的是.NET 6 請加上 --version 6.0.15
```

#### 填入連線資訊
在 `Pages/WarehouseTestAvailableStock.cshtml.cs` 中找到如下區塊並填入資訊
```csharp
public WarehouseTestAvailableStockModel() : base()
{
    this.Server = "YourServerAddress";
    this.builder = new SqlConnectionStringBuilder
    {
        DataSource = Server, 
        InitialCatalog = "YourDatabaseName",
        UserID = "YourUserID",
        Password = "YourPassword",
        Encrypt = true,
        TrustServerCertificate = true
    };
}
```

#### 接著開啟 powershell 或 bash 開始建置
```powershell
dotnet build
dotnet run
```

#### 開啟瀏覽器
開啟瀏覽器，輸入`localhost:5100`（註：Port 可在 `Program.cs` 中修改）

![localhost:5100](/img/WarehouseTestAvailableStock.png)
