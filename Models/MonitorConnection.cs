namespace TelemedicinaMonitores.Models
{
    public class MonitorConnection
    {
        public string MonitorId { get; set; }
        public string Location { get; set; } // "home" o "institution"
        public List<SensorInfo> Sensors { get; set; }
    }

    public class SensorInfo
    {
        public string Type { get; set; } // "heart_rate", "glucose", etc.
    }
}