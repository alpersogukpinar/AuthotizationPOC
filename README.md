# AuthorizationPOC

Bu proje, Entity Framework Core ile SQL Server üzerinde çalışan bir yetkilendirme altyapısı örneğidir.

## Başlangıç

### 1. Gerekli NuGet Paketleri

Aşağıdaki komutlarla Entity Framework Core ve SQL Server için gerekli NuGet paketlerini yükleyin (sürüm 9.0.5):

```sh
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.5
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.5
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.5
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.5
```

### 2. Docker ile SQL Server Kurulumu

Proje kök dizininde bulunan `docker-compose.yml` dosyası ile SQL Server başlatmak için:

```sh
docker compose up -d
```

SQL Server bağlantı bilgileri:
- Sunucu: `localhost,1440`
- Kullanıcı adı: `sa`
- Parola: `YourStrong!Passw0rd`
- Veritabanı: `AuthorizationDB`

### 3. Bağlantı Dizesi

`appsettings.json` dosyanıza aşağıdaki bağlantı dizesini ekleyin:

```json
"ConnectionStrings": {
  "AuthorizationDb": "Server=localhost,1440;Database=AuthorizationDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True"
}
```

### 4. Migration Oluşturma ve Uygulama

İlk migration’ı oluşturmak için terminalde proje dizininde şu komutu çalıştırın:

```sh
dotnet ef migrations add InitialCreate --project ./BuildingBlocks.Authorization --startup-project ./AuthorizationService
```

Migration’ı veritabanına uygulamak için:

```sh
dotnet ef database update --project ./BuildingBlocks.Authorization --startup-project ./AuthorizationService
```

> Not: Proje yollarını kendi dizin yapınıza göre güncelleyebilirsiniz.  
> `AuthorizationService` API projenizin adı olmalıdır.

### 5. Otomatik Migration (Opsiyonel)

Uygulama başlarken veritabanı ve tabloların otomatik oluşmasını sağlamak için `Program.cs` veya `Startup.cs` dosyanızda aşağıdaki kodu kullanabilirsiniz:

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
    db.Database.Migrate();
}
```

---

## Ekstra

- Veritabanı şeması için `BuildingBlocks.Authorization/database/authorization-schema.dbml` dosyasını kullanabilirsiniz.
- Diyagramı [dbdiagram.io](https://dbdiagram.io) ile görselleştirebilirsiniz.

---

Herhangi bir sorunda veya katkı için PR gönderebilirsiniz.