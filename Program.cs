using selenium_web.Adapters;
using selenium_web.Adapters.Interfaces;
using selenium_web.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register adapters
builder.Services.AddScoped<MyWebDriver>();
builder.Services.AddScoped<IScreenshotService, SeleniumScreenshotAdapter>();
builder.Services.AddScoped<IReadingModeService, SeleniumReadingModeAdapter>();
builder.Services.AddScoped<IHeadingsService, SeleniumHeadingsStructureAdapter>();
builder.Services.AddScoped<ILinksService, SeleniumLinksAdapter>();


// Register models
builder.Services.AddScoped<ScreenshotModel>();
builder.Services.AddScoped<ReadingModeModel>();
builder.Services.AddScoped<HeadingsStructureModel>();
builder.Services.AddScoped<LinksModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();