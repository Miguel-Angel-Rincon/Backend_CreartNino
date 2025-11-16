using Api_CreartNino.Models;
using Api_CreartNino.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static Api_CreartNino.Controllers.PedidosController;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "http://www.ApiCreartNino.somee.com",
        ValidAudience = "http://www.ApiCreartNino.somee.com",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("7%9@CreaRtN!n0JWTSecUr3#Key2025*+"))
    };
});


// Add services to the container.



builder.Services.AddControllers();
builder.Services.AddSingleton<CorreoService>(); // ✅ singular

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CreartNinoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Conexion")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", policy =>
    {
        policy
            .SetIsOriginAllowed(_ => true)  // ✅ permite todos los orígenes
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();            // ✅ funciona con credenciales
    });
});



builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // 👈
    app.UseSwagger();
    app.UseSwaggerUI();
}
// ⚠️ ORDEN CORRECTO DE MIDDLEWARES (CRÍTICO)
app.UseCors("NuevaPolitica");  // ⬅️ 1. CORS primero
app.UseRouting();               // ⬅️ 2. Routing después
app.UseAuthentication();        // ⬅️ 3. Auth
app.UseAuthorization();         // ⬅️ 4. Authorization
app.MapControllers();



app.Run();


