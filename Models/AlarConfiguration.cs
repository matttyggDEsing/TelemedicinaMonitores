namespace TelemedicinaMonitores.Models
{
    public class AlarmConfiguration
    {
        public string SensorType { get; set; }
        public double Threshold { get; set; }
        public string Condition { get; set; } // ">", "<", "=="
        public string Severity { get; set; } // "low", "medium", "high"
    }
}