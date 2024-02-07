using NetSchool.Api;
using NetSchool.Api.Configuration;
using NetSchool.Services.Settings;
using NetSchool.Settings;

var mainSettings = Settings.Load<MainSettings>("Main");
var logSettings = Settings.Load<LogSettings>("Log");
var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");

var builder = WebApplication.CreateBuilder(args);
builder.AddAppLogger(mainSettings, logSettings);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAppAutoMappers();
builder.Services.AddAppValidator();
builder.Services.AddAppControllerAndViews();
builder.Services.AddAppCors();
builder.Services.AddAppHealthChecks();
builder.Services.AddAppVersioning();
builder.Services.AddAppSwagger(mainSettings, swaggerSettings);

builder.Services.RegisterServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseAppCors();
app.UseAppHealthChecks();
app.UseAppControllerAndViews();
app.UseAppSwagger();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
