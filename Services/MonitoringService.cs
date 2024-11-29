using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using MonitoringApp.Models;

namespace MonitoringApp // Пространство имен для твоего проекта
{
    // Класс для хранения статуса сервиса
    // Класс для мониторинга сервисов
    public class MonitoringService
    {
        private readonly HttpClient httpClient = new(); // Клиент для отправки HTTP-запросов
        private readonly Subject<ServiceStatus> statusStream = new(); // Поток статусов сервисов

        // Свойство для получения статусов сервисов
        public IObservable<ServiceStatus> StatusStream => statusStream.AsObservable();

        // Коллекция для хранения всех сервисов
        private readonly List<ServiceStatus> serviceStatuses = new();

        // Конструктор запускает мониторинг
        public MonitoringService()
        {
            StartMonitoring();
        }

        // Метод для начала мониторинга
        private void StartMonitoring()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    

                    // Проверяем доступность сервисов
                    var statuses = new[] {
                        await CheckServiceAsync("VPNochka", "http://85.192.63.5/"),
                        await CheckServiceAsync("Google", "https://www.google.com/")
                    };

                    // Добавляем статусы в коллекцию
                    foreach (var status in statuses)
                    {
                        serviceStatuses.Add(status); // Добавляем новый статус
                        statusStream.OnNext(status); // Отправляем в поток
                    }
                    await Task.Delay(1000); // Пауза 10 секунд между проверками
                }
            });
        }

        // Метод для проверки доступности сервиса
        private async Task<ServiceStatus> CheckServiceAsync(string name, string url)
        {
            var status = new ServiceStatus
            {
                Name = name,  // Передаем имя сервиса
                LastChecked = DateTime.UtcNow // Время последней проверки
            };

            try
            {
                var response = await httpClient.GetAsync(url); // Отправляем GET-запрос
                status.IsOnline = response.IsSuccessStatusCode; // Если статус успешный, сервис доступен
            }
            catch
            {
                status.IsOnline = false; // Если ошибка при запросе, сервис недоступен
            }

            return status;
        }

        // Метод для получения всех статусов
        public Task<List<ServiceStatus>> GetServiceStatusesAsync()
        {
            return Task.FromResult(serviceStatuses); // Возвращаем список всех статусов
        }
    }
}
