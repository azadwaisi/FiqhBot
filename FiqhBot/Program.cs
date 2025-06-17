using FiqhBot.Contexts.MainContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ۱. خواندن Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ۲. ثبت DbContext با فعال‌سازی پشتیبانی از Vector
builder.Services.AddDbContext<ResourceDbContext>(options =>
    options.UseNpgsql(connectionString,o=>o.UseVector())); // <-- این خط کلیدی و اصلاح شده است


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); // برای HttpClientFactory
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession(); // اضافه کردن Session

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Ask}/{action=Index}");

app.Run();
