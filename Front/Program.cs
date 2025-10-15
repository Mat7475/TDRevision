using Front;
using Front.Models;
using Front.Service;
using Front.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Enregistrement des services
builder.Services.AddScoped<IService<Commande, int, string>>(sp =>
{
    var httpClient = sp.GetRequiredService<HttpClient>();
    return new WebServiceCommande(httpClient, "Commande");
});

// Enregistrement des ViewModels
builder.Services.AddScoped<CommandesViewModel>();

await builder.Build().RunAsync();