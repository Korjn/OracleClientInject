# Korjn.OracleClientInject

> Lightweight Oracle connection factory with DI, validation, and secure configuration via IOptions pattern.

## ✨ Features
- Built-in support for `IOptions<T>` and `[Required]` validation
- Lazy, cached connection string construction
- Secure session schema handling via `ALTER SESSION`
- Safe, injectable factory (`IOracleConnectionFactory`) for tests or services
- Two easy extension methods to register in your app

---

## 📦 Installation

```bash
# Coming soon via NuGet
Install-Package Korjn.OracleClientInject
```

---

## 🔧 Usage

### 1. Add to `Program.cs`

```csharp
builder.Services.AddOracleClient(builder.Configuration);
```

Or with inline options:

```csharp
builder.Services.AddOracleClient(options =>
{
    options.DataSource = "MyTNSAlias";
    options.TnsnamesFile = "tnsnames.ora";
    options.UserName = "scott";
    options.Password = "tiger";
    options.DefaultSchema = "myschema";
});
```

### 2. Inject and use `IOracleConnectionFactory`

```csharp
public class MyService(IOracleConnectionFactory factory)
{
    public async Task DoSomethingAsync()
    {
        using var conn = factory.CreateConnection();
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

## 📄 Configuration Example

```json
{
  "OracleDbClient": {
    "TnsnamesFile": "C:\\oracle\\network\\admin\\tnsnames.ora",
    "DataSource": "ORCL",
    "UserName": "scott",
    "Password": "tiger",
    "DefaultSchema": "HR"
  }
}
```

---

## 📜 License

MIT © [Korjn](https://github.com/Korjn)