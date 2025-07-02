using Microsoft.AspNetCore.Mvc;
using TelemedicinaMonitores.Models;
using TelemedicinaMonitores.Data;
using static TelemedicinaMonitores.Controllers.MonitorController;

namespace TelemedicinaMonitores.Controllers
{
	public class PatientController : Controller
	{
		private readonly ApplicationDbContext _db;

		public PatientController(ApplicationDbContext db)
		{
			_db = db;
		}

		[HttpGet]
		public IActionResult Details(string id)
		{
			if (!MonitorController.TryGetMonitor(id, out var patient))
				return NotFound();

			var ficha = _db.Pacientes.FirstOrDefault(p => p.MonitorId == id);

			var model = new PacienteDetailsViewModel
			{
				Monitor = patient,
				Ficha = ficha
			};

			return View(model);
		}
	}
}
