using BuildingBlocks.Authorization.Services;
using BuildingBlocks.Authorization.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// AuthorizationModule DI ile PermissionCacheService ve policy provider'ı ekler
builder.Services.AddAuthorizationModule(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("Fake")
    .AddScheme<AuthenticationSchemeOptions, AlwaysSucceedAuthHandler>("Fake", null);



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


// Servis ayağa kalkarken permissionlar okunup cache'leniyor...
await Task.Delay(2000); // 2 saniye bekle, diğer servislerin açılması için
using (var scope = app.Services.CreateScope())
{
    var cacheService = scope.ServiceProvider.GetRequiredService<PermissionCacheService>();
    // ApplicationCode'u config'den al
    var applicationCode = builder.Configuration.GetValue<string>("ApplicationCode") ?? "MoneyTransferService";
    await cacheService.CachePermissionsAsync(applicationCode);
}


app.Run();



public class AlwaysSucceedAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public AlwaysSucceedAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        System.Text.Encodings.Web.UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "TestUser") }, "Fake");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Fake");
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}