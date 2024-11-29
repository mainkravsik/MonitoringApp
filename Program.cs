using Microsoft.AspNetCore.SignalR;
using MonitoringApp.Hubs;
using MonitoringApp;

var builder = WebApplication.CreateBuilder(args);

// Добавляем CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});



// Регистрация зависимостей
builder.Services.AddSingleton<MonitoringService>();
builder.Services.AddSignalR();
builder.Services.AddControllers();
var app = builder.Build();
// Разрешаем использование CORS
app.UseCors("AllowAllOrigins");
app.MapControllers();


app.UseRouting();

// Разрешаем обслуживать статические файлы из папки wwwroot
app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<MonitoringHub>("/monitoringHub");
});


// Запуск потока уведомлений
var monitoringService = app.Services.GetRequiredService<MonitoringService>();
var hubContext = app.Services.GetRequiredService<IHubContext<MonitoringHub>>();

monitoringService.StatusStream.Subscribe(async status =>
{
    await hubContext.Clients.All.SendAsync("ReceiveStatus", status);
});

app.Run();
