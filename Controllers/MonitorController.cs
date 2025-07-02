using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TelemedicinaMonitores.Hubs;
using TelemedicinaMonitores.Models;

namespace TelemedicinaMonitores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonitorController : ControllerBase
    {
        private readonly IHubContext<MonitorHub> _hubContext;
        private static Dictionary<string, PatientData> _monitors = new();

        public MonitorController(IHubContext<MonitorHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("connect")]
        public IActionResult ConnectMonitor([FromBody] MonitorConnection request)
        {
            var newMonitorId = Guid.NewGuid().ToString(); // Generar ID único

            var patientData = new PatientData
            {
                MonitorId = newMonitorId,
                PatientName = $"Paciente {_monitors.Count + 1}",
                Location = request.Location,
                Sensors = request.Sensors.Select(s => new SensorData
                {
                    Type = s.Type,
                    CurrentValue = 0,
                    Unit = GetUnitForSensor(s.Type)
                }).ToList()
            };

            _monitors[newMonitorId] = patientData;

            // Notificar a todos los clientes
            _hubContext.Clients.All.SendAsync("ReceiveMonitorData", newMonitorId, patientData);

            // Devolver el nuevo monitorId al cliente
            return Ok(new { success = true, monitorId = newMonitorId, patient = patientData });
        }

        [HttpGet]
        public IActionResult GetMonitors()
        {
            return Ok(_monitors.Values);
        }

        [HttpPost("{monitorId}/data")]
        public async Task<IActionResult> ReceiveSensorData(string monitorId, [FromBody] SensorUpdate update)
        {
            if (!_monitors.TryGetValue(monitorId, out var patientData))
                return NotFound();

            var sensor = patientData.Sensors.FirstOrDefault(s => s.Type == update.SensorType);
            if (sensor != null)
            {
                sensor.CurrentValue = update.Value;
                sensor.History.Add(new SensorHistory
                {
                    Value = update.Value,
                    Timestamp = DateTime.UtcNow
                });

                CheckAlarms(monitorId, sensor, update.Value);
                
                await _hubContext.Clients.All.SendAsync("ReceiveMonitorData", monitorId, patientData);
            }

            return Ok();
        }

        private void CheckAlarms(string monitorId, SensorData sensor, double value)
        {
            // Implementa tu lógica de alarmas aquí
        }

        private string GetUnitForSensor(string sensorType)
        {
            return sensorType switch
            {
                "Frecuencia_cardíaca" => "bpm",
                "Glucosa" => "mg/dL",
                "Presión_arterial" => "mmHg",
                _ => ""
            };




        }
        public static bool TryGetMonitor(string id, out PatientData patient)
        {
            return _monitors.TryGetValue(id, out patient);
        }
    }


}

