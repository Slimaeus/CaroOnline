# Caro Online Game


## appsettings.json in CaroAPI
```json
{
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "CaroDatabase": "Data Source=<Your Server Name>;Database=<Your database name>;Integrated Security=True;"
  },
  "JWT": {
    "Key": "HFQ8GmeZwwXiX3LjU5ZL9ffBdUMJNDxL",
    "Issuer": "https://localhost:7118;",
    "Audience": "https://localhost:7118;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ApiContacts": {
    "Thai": {
      "Name": "Nguyen Hong Thai",
      "Url": "https://github.com/Slimaeus"
    }
  }
}

```
## appsettings.json in CaroMVC 
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "GameSqliteDb": "DataSource=../Data/CaroGame.db"
  },
  "CaroAPIBaseUrl": "https://localhost:7118/api/",
  "JWT": {
    "Key": "HFQ8GmeZwwXiX3LjU5ZL9ffBdUMJNDxL",
    "Issuer": "https://localhost:7118;",
    "Audience": "https://localhost:7118;"
  }
}
```
