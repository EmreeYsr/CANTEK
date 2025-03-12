using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Dil seçenekleri
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Dil destekleme
var supportedCultures = new[] { "en", "tr" }; // Desteklenen diller

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"), // Varsayýlan dil
    SupportedCultures = supportedCultures.Select(x => new CultureInfo(x)).ToArray(),
    SupportedUICultures = supportedCultures.Select(x => new CultureInfo(x)).ToArray()
});

app.UseRouting();

// Add Authorization Middleware
app.UseAuthorization();

// Map the default controller route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=PLC}/{action=Index}/{id?}");

// Register API routes for PLC control
app.MapControllerRoute(
    name: "PLC",
    pattern: "PLC/{action=BaglantiAc}/{id?}",
    defaults: new { controller = "PLC" });

app.Run();
