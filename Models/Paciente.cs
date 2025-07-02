using System.ComponentModel.DataAnnotations;

namespace TelemedicinaMonitores.Models
{
    public class Paciente
    {
        [Key]
        public string MonitorId { get; set; }

        public string NombreCompleto { get; set; }
        public string Domicilio { get; set; }
        public string Enfermedades { get; set; }
        public string Vacunas { get; set; }
        public string HistorialClinico { get; set; }
    }
}
