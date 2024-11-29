namespace MonitoringApp.Models;

public class ServiceStatus
{
    public required string Name { get; set; } // Имя сервиса (например, "Google")
    public bool IsOnline { get; set; } // Статус доступности (true - доступен, false - недоступен)
    public DateTime LastChecked { get; set; } // Время последней проверки
}