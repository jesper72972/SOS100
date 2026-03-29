using Microsoft.AspNetCore.Authentication.Cookies;
using SOS100.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Account/Index");

builder.Services.AddHttpClient<FormanService>((serviceProvider, httpClient) =>
{
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    string adress = config.GetValue<string>("SOS100Adress") ?? "";
    httpClient.BaseAddress = new Uri(adress);
});

builder.Services.AddHttpClient<FormanerStatusService>((serviceProvider, httpClient) =>
{
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    httpClient.BaseAddress = new Uri(config["ApiUrls:StatusFormanerApi"] + "/");
});

builder.Services.AddHttpClient<RapportService>((serviceProvider, httpClient) =>
{
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    httpClient.BaseAddress = new Uri(config["ApiUrls:Rapport"] + "/");
});

builder.Services.AddHttpClient<RapportPostService>((serviceProvider, httpClient) =>
{
    var config = serviceProvider.GetRequiredService<IConfiguration>();
    httpClient.BaseAddress = new Uri(config["ApiUrls:Rapport"] + "/");
});

builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();