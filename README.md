# Korjn.OracleClientInject

> 💡 Lightweight, testable Oracle connection factory for .NET with full DI support, secure credential handling, and built-in `IOptions` validation.

---

## ✨ Features

- ✅ Simple and consistent factory API via `IOracleConnectionFactory`
- ✅ Fully supports `IOptions<T>` pattern with `[Required]` validation
- 🔐 Lazy, secure connection string construction (no secrets in memory until needed)
- 🎯 Supports both **sync** and **async** connection creation
- 🧪 Easily mockable for testing scenarios
- 🛠 Integration with `TNS_ADMIN` and `tnsnames.ora` support
- 🌱 Clean setup via `AddOracleClient(...)` extensions

---

## 📦 Installation

```
Install-Package Korjn.OracleClientInject
```

---

## 🚀 Quick Start

### 1. Register via DI

In your `Program.cs`:

```csharp
builder.Services.AddOracleClient(options =>
{
    options.DataSource = "MyTNSAlias";
    options.UserName = "scott";
    options.Password = "tiger";
    options.DefaultSchema = "myschema";
});
```

Or with access to other services:

```csharp
builder.Services.AddOracleClient((options, sp) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    options.DataSource = config["Db:DataSource"];
    options.UserName = config["Db:User"];
    options.Password = config["Db:Password"];
});
```

---

### 2. Inject and use `IOracleConnectionFactory`

```csharp
public class MyService(IOracleConnectionFactory factory)
{
    public async Task DoSomethingAsync()
    {
        await using var conn = await factory.CreateConnectionAsync(CancellationToken.None);
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT * FROM DUAL";

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            Console.WriteLine(reader.GetString(0));
        }
    }
}
```

---

## 🛠 Configuration Example (appsettings.json)

```json
{
  "OracleDbClient": {
    "TnsnamesFile": "C:\oracle\network\admin\tnsnames.ora",
    "DataSource": "ORCL",
    "UserName": "scott",
    "Password": "tiger",
    "DefaultSchema": "HR",
    "Pooling": true,
    "MinPoolSize": 1,
    "MaxPoolSize": 100,
    "IncrPoolSize": 1,
    "DecrPoolSize": 1,
    "ConnectionLifeTime": 0,
    "ConnectionTimeout": 15
  }
}
```

---

## 🧩 API Overview

### OracleConnectionOptions

| Property              | Type     | Default | Description                                        |
|-----------------------|----------|---------|----------------------------------------------------|
| TnsnamesFile          | string?  | —       | Path to `tnsnames.ora` file                        |
| DataSource            | string   | —       | TNS alias or host:[port]/service                   |
| UserName              | string?  | —       | Oracle DB username                                 |
| Password              | string?  | —       | Oracle DB password                                 |
| DefaultSchema         | string?  | —       | Optional schema for session (`ALTER SESSION`)      |
| Pooling               | bool?    | true    | Enable or disable connection pooling               |
| MinPoolSize           | int?     | 1       | Minimum connections in pool                        |
| MaxPoolSize           | int?     | 100     | Maximum connections in pool                        |
| IncrPoolSize          | int?     | 1       | Pool increment size when exhausted                 |
| DecrPoolSize          | int?     | 1       | Pool decrement size when idle                      |
| ConnectionLifeTime    | int?     | 0       | Seconds until pooled connection is destroyed       |
| ConnectionTimeout     | int?     | 15      | Seconds to wait before connection times out        |

---

## 🔧 Interface: IOracleConnectionFactory

```csharp
public interface IOracleConnectionFactory
{
    OracleConnectionOptions ConnectionOptions { get; }
    string ConnectionStringAttributes { get; }
    string ConnectionString { get; }

    OracleConnection CreateConnection();
    OracleConnection CreateConnection(string userName, string password);

    Task<OracleConnection> CreateConnectionAsync(CancellationToken cancellationToken);
    Task<OracleConnection> CreateConnectionAsync(string userName, string password, CancellationToken cancellationToken);
}
```

---

## 🧩 Extension Methods

```csharp
public static class ServiceCollectionExtensions
{
    IServiceCollection AddOracleClient(Action<OracleConnectionOptions> configure);
    IServiceCollection AddOracleClient(Action<OracleConnectionOptions, IServiceProvider> configureOptions);
}
```

Both methods:
- Register `IOracleConnectionFactory` as singleton
- Validate `OracleConnectionOptions` on app start

---

## 📜 License

MIT © [Korjn](https://github.com/Korjn)