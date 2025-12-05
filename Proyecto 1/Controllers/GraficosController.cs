using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_1.Data;

namespace Proyecto_1.Controllers
{
    public class GraficosController : Controller
    {
        private readonly SelvaDbContext _context;

        public GraficosController(SelvaDbContext context)
        {
            _context = context;
        }

        // GET: Graficos
        public IActionResult Index()
        {
            return View();
        }

        // GET: Graficos/VisitasPorAnimal
        public async Task<IActionResult> VisitasPorAnimal(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var datos = await _context.MovimientoVisitas
                .Include(m => m.Animal)
                .Where(m => (!fechaInicio.HasValue || m.FechaVisita >= fechaInicio) &&
                           (!fechaFin.HasValue || m.FechaVisita <= fechaFin))
                .GroupBy(m => m.Animal.Nombre)
                .Select(g => new
                {
                    Animal = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .Take(10)
                .ToListAsync();

            ViewBag.FechaInicio = fechaInicio?.ToString("yyyy-MM-dd");
            ViewBag.FechaFin = fechaFin?.ToString("yyyy-MM-dd");
            return View(datos);
        }

        // GET: Graficos/VisitasPorMes
        public async Task<IActionResult> VisitasPorMes(int? anio)
        {
            var anioSeleccionado = anio ?? DateTime.Now.Year;

            var datos = await _context.MovimientoVisitas
                .Where(m => m.FechaVisita.Year == anioSeleccionado)
                .GroupBy(m => m.FechaVisita.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    Total = g.Count()
                })
                .OrderBy(x => x.Mes)
                .ToListAsync();

            ViewBag.Anio = anioSeleccionado;
            return View(datos);
        }

        // GET: Graficos/AnimalesPorHabitat
        public async Task<IActionResult> AnimalesPorHabitat()
        {
            var datos = await _context.Animales
                .GroupBy(a => a.Habitat)
                .Select(g => new
                {
                    Habitat = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();

            return View(datos);
        }

        // GET: Graficos/CuidadoresActividad
        public async Task<IActionResult> CuidadoresActividad()
        {
            var datos = await _context.MovimientoVisitas
                .Include(m => m.Cuidador)
                .GroupBy(m => m.Cuidador.Nombre)
                .Select(g => new
                {
                    Cuidador = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(x => x.Total)
                .ToListAsync();

            return View(datos);
        }
    }
}