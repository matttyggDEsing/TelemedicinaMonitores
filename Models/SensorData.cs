namespace TelemedicinaMonitores.Models
{
    public class SensorData
    {
        public string Type { get; set; }
        public double CurrentValue { get; set; }
        public string Unit { get; set; }
        public List<SensorHistory> History { get; set; } = new List<SensorHistory>();
    }
}