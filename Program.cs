using choosing.Context;
using choosing.Repository.Impl;
using choosing.Repository.Interfaces;
using choosing.Services.Impl;
using choosing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

////// Configuración para escuchar en puerto interno 80 (estándar para contenedores)
//builder.WebHost.UseUrls("http://0.0.0.0:80", "https://0.0.0.0:443");

//// Añadimos detección de HTTPS
//builder.Services.AddHttpsRedirection(options =>
//{
//    options.HttpsPort = 443;  // Puerto interno para HTTPS
//});




builder.Services.AddDbContext<DbHotelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddScoped<IListService, ListService>();
builder.Services.AddScoped<IListRepository, ListRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowNetlify", policy =>
//    {
//        policy.WithOrigins("https://choosing-app.netlify.app/")
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction()) // Habilitamos Swagger en ambos entornos para pruebas
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll"); // <- Agrega esto antes de UseAuthorization()
//app.UseCors("AllowNetlify");



//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API Choosing está funcionando!");

app.Run();
