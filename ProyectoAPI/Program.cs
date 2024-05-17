using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoAPI.Helpers;
using ProyectoAPI.Models.Entities;
using ProyectoAPI.Repositories;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string cadena = builder.Configuration.GetConnectionString("Conexion")??"";

builder.Services.AddDbContext<ItesrcneActividadesContext>(options => options.UseMySql(cadena, ServerVersion.AutoDetect(cadena)));

var jwtconfig = new ConfigurationBuilder()
    .AddJsonFile("JwtSettings.json")
    .Build();

builder.Services.AddSingleton(jwtconfig);

var tknValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = jwtconfig["Jwt:Issuer"],
    ValidAudience = jwtconfig["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtconfig["Jwt:Key"]))
};


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(x=>{
    x.TokenValidationParameters = tknValidationParameters;
});

//Automapper como servicio
builder.Services.AddAutoMapper(typeof(Program));

//Servicios
builder.Services.AddTransient<JwtTokenGenerator>();
builder.Services.AddTransient<ItesrcneActividadesContext>();
builder.Services.AddTransient<DepartamentosRepository>();
builder.Services.AddTransient<ActividadesRepository>();
builder.Services.AddSingleton(tknValidationParameters);
//builder.Services.AddSingleton<IWebHostEnvironment>();

// builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
