using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NetSchool.Web;
using MudBlazor.Services;
using Blazored.LocalStorage;
using Microsoft.Extensions.DependencyInjection;
using System;
using NetSchool.Web.DelegatingHandlers;
using System.Net.Http;
using Microsoft.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddAuthorizationCore();

builder.Services.AddTransient<RefreshTokenDelegatingHandler>();

builder.Services.AddScoped(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HttpClient
    {
        BaseAddress = new Uri(navigationManager.BaseUri)
    };
});

builder.Services.AddHttpClient("delegatingClient", (sp, c) =>
{
    var nav = sp.GetRequiredService<NavigationManager>();
    c.BaseAddress = new Uri(nav.BaseUri);
}).AddHttpMessageHandler<RefreshTokenDelegatingHandler>();


builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

builder.Services.RegisterServices();

var app = builder.Build();

await app.RunAsync();
