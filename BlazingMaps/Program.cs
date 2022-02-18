using BlazingMaps;
using BlazingMaps.Interfaces;
using BlazingMaps.Models;
using BlazingMaps.Services;
using Darnton.Blazor.DeviceInterop.Geolocation;
using FisSst.BlazorMaps.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddRazorPages();
services.AddServerSideBlazor();
services.AddHttpClient(
    Consts.AddressLookupClientName,
    options =>
    {
        options.BaseAddress = new Uri(builder.Configuration[Consts.AddressLookupUrlKey]);
        options.DefaultRequestHeaders.Add("User-Agent", Assembly.GetExecutingAssembly().GetName().Name);
    }
);
services.AddScoped<IAddressLookupService, AddressLookupService>();
services
    .AddOptions<MapOptionsEx>()
    .Bind(builder.Configuration.GetRequiredSection(Consts.MapOptionsKey));
services.AddBlazorLeafletMaps();
services.AddScoped<IGeolocationService, GeolocationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
