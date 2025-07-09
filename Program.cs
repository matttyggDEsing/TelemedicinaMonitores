using TelemedicinaMonitores.Hubs;
using TelemedicinaMonitores.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=telemedicina.db"));

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR(); // Agregar SignalR

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Mapear el Hub de SignalR
app.MapHub<MonitorHub>("/monitorHub");


app.Run();