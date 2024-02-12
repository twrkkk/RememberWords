using Coravel;
using NetSchool.Api;
using NetSchool.Api.Configuration;
using NetSchool.Context;
using NetSchool.Context.Seeder;
using NetSchool.Services.Logger;
using NetSchool.Services.Settings;
using NetSchool.Settings;

var mainSettings = Settings.Load<MainSettings>("Main");
var logSettings = Settings.Load<LogSettings>("Log");
var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");

var builder = WebApplication.CreateBuilder(args);
builder.AddAppLogger(mainSettings, logSettings);

var services = builder.Services;

services.AddAppDbContext();

services.AddHttpContextAccessor();
services.AddRazorPages();
services.AddAppAutoMappers();
services.AddAppValidator();
services.AddAppControllerAndViews();
services.AddAppCors();
services.AddAppHealthChecks();
services.AddAppVersioning();
services.AddAppSwagger(mainSettings, swaggerSettings);
services.AddScheduler();
services.AddAppDeleteExpiredCollectionsScheduler();

services.RegisterServices();


var app = builder.Build();

app.UseAppCors();
app.UseAppHealthChecks();
app.UseAppControllerAndViews();
app.UseAppSwagger();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.UseAppDeleteExpiredCollectionsScheduler();

DbInitializer.Execute(app.Services);

DbSeeder.Execute(app.Services);

var logger = app.Services.GetRequiredService<IAppLogger>();

logger.Information("The Api has started");

app.Run();

logger.Information("The Api has stopped");