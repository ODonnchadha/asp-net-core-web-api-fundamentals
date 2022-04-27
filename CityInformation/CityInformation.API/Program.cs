using CityInformation.API.Contexts.DbContexts;
using CityInformation.API.Interfaces.Repositories;
using CityInformation.API.Interfaces.Services;
using CityInformation.API.Repositories;
using CityInformation.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/Serilog.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
    // e.g.: Accept: application/xml. Status: 406 Not Acceptable.
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();//.AddXmlDataContractSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(bearer =>
{
    bearer.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(
                    builder.Configuration["Authentication:SecretKeyFor"]))
    };
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
builder.Services.AddSingleton<IMailService, LocalMailService>();
#else
builder.Services.AddSingleton<IMailService, CloudMailService>();
#endif

builder.Services.AddDbContext<CityInformationContext>(options =>
{
    options.UseSqlite(builder.Configuration["ConnectionStrings:X"]);
});

builder.Services.AddScoped<ICityInformationRepository, CityInformationRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// NOTE: Pipeline order matters greatly. Are we authenticated at all?
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => 
{
    endpoints.MapControllers();
});

app.Run();