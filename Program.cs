using Microsoft.AspNetCore.SignalR;
using MonitoringApp.Hubs;
using MonitoringApp;

var builder = WebApplication.CreateBuilder(args);

// ��������� CORS
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



// ����������� ������������
builder.Services.AddSingleton<MonitoringService>();
builder.Services.AddSignalR();
builder.Services.AddControllers();
var app = builder.Build();
// ��������� ������������� CORS
app.UseCors("AllowAllOrigins");
app.MapControllers();


app.UseRouting();

// ��������� ����������� ����������� ����� �� ����� wwwroot
app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<MonitoringHub>("/monitoringHub");
});


// ������ ������ �����������
var monitoringService = app.Services.GetRequiredService<MonitoringService>();
var hubContext = app.Services.GetRequiredService<IHubContext<MonitoringHub>>();

monitoringService.StatusStream.Subscribe(async status =>
{
    await hubContext.Clients.All.SendAsync("ReceiveStatus", status);
});

app.Run();
