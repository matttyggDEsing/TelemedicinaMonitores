using System.ComponentModel.DataAnnotations.Schema;
namespace TelemedicinaMonitores.Models
{
    public class PatientData
    {
        [NotMapped]
        public string MonitorId { get; set; }
        public string PatientName { get; set; }
        public string Location { get; set; }
        public List<SensorData> Sensors { get; set; } = new List<SensorData>();
    }
}
