using cityguide.Data;
using cityguide.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// CORS'u yap�land�r�n ve bir politika ekleyin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://localhost:4200") // Angular uygulaman�z�n URL'si
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// DataContext'e ba�lanma ve yap�land�rma
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper'� yap�land�rma
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Uygulama repository'sini ve servislerini ekleyin
builder.Services.AddScoped<IAppRepository, AppRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Nullable reference types uyar�lar�n� bast�rma
builder.Services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

// JWT Authentication ekleyin
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Appsettings:Token").Value);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS'u Authorization'dan �nce ekleyin
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
