using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NetSchool.Web;
using MudBlazor.Services;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using NetSchool.Web.Pages.Auth.Services;
using NetSchool.Web.Providers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(Settings.ApiRoot) });

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

builder.Services.RegisterServices();

await builder.Build().RunAsync();
