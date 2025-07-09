using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TelemedicinaMonitores.Hubs;
using TelemedicinaMonitores.Models;
using TelemedicinaMonitores.Data;

namespace TelemedicinaMonitores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MonitorController : ControllerBase
    {
        private readonly IHubContext<MonitorHub> _hubContext;
        private readonly ApplicationDbContext _db;
        private static Dictionary<string, PatientData> _monitors = new();

        public MonitorController(IHubContext<MonitorHub> hubContext, ApplicationDbContext db)
        {
            _hubContext = hubContext;
            _db = db;
        }

        [HttpPost("connect")]
        public IActionResult ConnectMonitor([FromBody] MonitorConnection request)
        {
            // Usar el ID enviado o generar uno nuevo
            var newMonitorId = string.IsNullOrEmpty(request.MonitorId) ? Guid.NewGuid().ToString() : request.MonitorId;

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

            // Asociar ficha clínica en la base de datos si no existe
            var ficha = _db.Pacientes.FirstOrDefault(p => p.MonitorId == newMonitorId);
            if (ficha == null)
            {
                ficha = new Paciente
                {
                    MonitorId = newMonitorId,
                    NombreCompleto = patientData.PatientName,
                    Domicilio = "Sin especificar",
                    Enfermedades = "",
                    Vacunas = "",
                    HistorialClinico = ""
                };
                _db.Pacientes.Add(ficha);
                _db.SaveChanges();
            }

            _hubContext.Clients.All.SendAsync("ReceiveMonitorData", newMonitorId, patientData);

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
            // Lógica de alarma opcional
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

        // Permitir acceso desde otros controladores
        public static bool TryGetMonitor(string id, out PatientData patient)
        {
            return _monitors.TryGetValue(id, out patient);
        }
    }
}