using LoggingExample.Data;
using LoggingExample.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure DbContext with connection string from appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("default")));


// Add FileLogger and SplunkLogger services
builder.Services.AddSingleton(new FileLogger("logs/application.log"));

//builder.Services.AddSingleton(new SplunkLogger("http://127.0.0.1:8000", "fc9e2af8-8c9f-4861-9dd2-67c8be0d3692"));
builder.Services.AddSingleton(new SplunkLogger("http://127.0.0.1:8000/services/collector", "fc9e2af8-8c9f-4861-9dd2-67c8be0d3692"));
//builder.Services.AddSingleton(new SplunkLogger("http://127.0.0.1:8088/services/collector", "fc9e2af8-8c9f-4861-9dd2-67c8be0d3692"));
//builder.Services.AddSingleton(new SplunkLogger("http://127.0.0.1:8000/", "fc9e2af8-8c9f-4861-9dd2-67c8be0d3692"));
//builder.Services.AddSingleton(new SplunkLogger("http://127.0.0.1:8000/services/collector", "fc9e2af8-8c9f-4861-9dd2-67c8be0d3692"));





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
