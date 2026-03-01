# ProjectTemplateForBlazorServer 🚀

Blazor Server 的專案範本 🎨，用於新增專案時可以一鍵產出完整專案結構 ✨

## 📂 專案結構

本專案採用多專案架構設計，包含以下三個子專案：

| 專案名稱 | 說明 | 📦 技術堆疊 |
|---------|------|------------|
| **BlazorServer.Server** | 🖥️ 主伺服器專案，負責 API 與 Blazor Server 托管 | ASP.NET Core, Blazor Server, NLog, NSwag, MiniProfiler, Dapper, MySQL, SQLite |
| **BlazorServer.Client** | 🌐 客戶端 Blazor 專案，處理 UI 渲染 | Blazor (Client-side) |
| **BlazorServer.Shared** | 🔄 共用專案，放置共享模型與程式碼 | .NET Standard |

## 🛠️ 主要功能

- 🔥 **Blazor Server** - 微軟現代化 UI 框架
- 📡 **API 自動生成** - 透過 NSwag 快速產生 Swagger 文件
- 📊 **效能監控** - 使用 MiniProfiler 追蹤效能瓶頸
- 🗄️ **資料庫支援** - MySQL + SQLite，並使用 Dapper ORM
- 📝 **日誌系統** - NLog 完整日誌記錄
- ⚙️ **多環境配置** - Debug/Release 環境切換
- 🔧 **一鍵發布** - 內建 Publish.bat 腳本

## 🚦 快速開始

```bash
# 還原專案依賴
dotnet restore

# 執行專案
dotnet run --project BlazorServer.Server
```

## 📋 API 文件

專案啟動後，存取以下 URL 查看 Swagger API 文件：

```
http://localhost:5000/swagger
```

## 📊 效能分析

使用 MiniProfiler 查看頁面效能：

```
http://localhost:5000/profiler
```

## 📁 專案範本使用方式

1. 將此資料庫複製到 Visual Studio 專案範本目錄
2. 在 Visual Studio 中選擇「建立新專案」
3. 搜尋 `BlazorServer` 即可看到此範本
4. 一鍵產出完整專案結構！ 🎉

## 🤝 技術支援

- 🔗 [Blazor 官方文檔](https://docs.microsoft.com/zh-tw/aspnet/core/blazor/)
- 📚 [NSwag 官方文檔](https://github.com/RicoSuter/NSwag)
- 🛠️ [Dapper 文件](https://dapper-tutorial.net/)

---

⭐ 如果這個範本對你有幫助，請幫我加個星標！
