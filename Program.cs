using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Configuración de autenticación y autorización
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization();

// OpenAPI
builder.Services.AddOpenApi();

// Controllers (MVC)
builder.Services.AddControllers();

// TokenValidationParameters para UserProfileService
builder.Services.AddSingleton<TokenValidationParameters>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var azureAdSection = configuration.GetSection("AzureAd");
    return new TokenValidationParameters
    {
        ValidIssuer = azureAdSection["Issuer"],
        ValidAudience = azureAdSection["Audience"],
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = null // Debes configurar la clave pública de tu Azure AD aquí
    };
});

// UserProfileService
builder.Services.AddScoped<Application.UseCases.UserProfileService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Opcional: Cambia el puerto si lo necesitas
builder.WebHost.UseUrls("http://*:5152");

var app = builder.Build();

app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/", () => "Client Portal Alpex");
app.Run();
