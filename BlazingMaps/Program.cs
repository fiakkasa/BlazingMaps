using BlazingMaps;
using BlazingMaps.Interfaces;
using BlazingMaps.Models;
using BlazingMaps.Services;
using FisSst.BlazorMaps.DependencyInjection;
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
