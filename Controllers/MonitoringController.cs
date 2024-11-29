using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MonitoringApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Этот атрибут указывает на общий маршрут контроллера
    public class MonitoringController : ControllerBase
    {
        private readonly MonitoringService _monitoringService;

        public MonitoringController(MonitoringService monitoringService)
        {
            _monitoringService = monitoringService;
        }

        // Этот метод обрабатывает GET-запросы на /api/monitoring/status
        [HttpGet("status")]
        public async Task<IActionResult> GetServiceStatuses()
        {
            var statuses = await _monitoringService.GetServiceStatusesAsync(); // Получаем статусы сервисов
            return Ok(statuses); // Возвращаем результат в формате JSON
        }
    }
}
