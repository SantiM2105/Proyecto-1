using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_1.Data;

namespace Proyecto_1.Controllers
{
    public class InformesController : Controller
    {
        private readonly SelvaDbContext _context;

        public InformesController(SelvaDbContext context)
        {
            _context = context;
        }

        // GET: Informes
        public IActionResult Index()
        {
            CargarParametros();
            return View();
        }

        // POST: Informes/Generar
        [HttpPost]
        public async Task<IActionResult> Generar(string tipoInforme, DateTime? fechaInicio, DateTime? fechaFin, int? idCuidador, int? idAnimal)
        {
            ViewBag.TipoInforme = tipoInforme;

            switch (tipoInforme)
            {
                case "visitas":
                    var visitas = await _context.MovimientoVisitas
                        .Include(m => m.Visitante)
                        .Include(m => m.Animal)
                        .Include(m => m.Cuidador)
                        .Where(m => (!fechaInicio.HasValue || m.FechaVisita >= fechaInicio) &&
                                   (!fechaFin.HasValue || m.FechaVisita <= fechaFin))
                        .OrderByDescending(m => m.FechaVisita)
                        .ToListAsync();

                    ViewBag.TotalVisitas = visitas.Count;
                    ViewBag.FechaInicio = fechaInicio?.ToString("dd/MM/yyyy");
                    ViewBag.FechaFin = fechaFin?.ToString("dd/MM/yyyy");
                    return View("InformeVisitas", visitas);

                case "animales":
                    var animales = await _context.Animales
                        .Include(a => a.Cuidador)
                        .Where(a => !idCuidador.HasValue || a.IdCuidador == idCuidador)
                        .ToListAsync();

                    ViewBag.TotalAnimales = animales.Count;
                    return View("InformeAnimales", animales);

                case "cuidadores":
                    var cuidadorStats = await _context.MovimientoVisitas
                        .Include(m => m.Cuidador)
                        .Where(m => !idCuidador.HasValue || m.IdCuidador == idCuidador)
                        .GroupBy(m => m.Cuidador)
                        .Select(g => new
                        {
                            Cuidador = g.Key,
                            TotalVisitas = g.Count()
                        })
                        .ToListAsync();

                    return View("InformeCuidadores", cuidadorStats);

                case "popularidad":
                    var popularidad = await _context.MovimientoVisitas
                        .Include(m => m.Animal)
                        .Where(m => (!fechaInicio.HasValue || m.FechaVisita >= fechaInicio) &&
                                   (!fechaFin.HasValue || m.FechaVisita <= fechaFin))
                        .GroupBy(m => m.Animal)
                        .Select(g => new
                        {
                            Animal = g.Key,
                            TotalVisitas = g.Count()
                        })
                        .OrderByDescending(x => x.TotalVisitas)
                        .ToListAsync();

                    ViewBag.FechaInicio = fechaInicio?.ToString("dd/MM/yyyy");
                    ViewBag.FechaFin = fechaFin?.ToString("dd/MM/yyyy");
                    return View("InformePopularidad", popularidad);

                default:
                    return RedirectToAction(nameof(Index));
            }
        }

        private void CargarParametros()
        {
            ViewBag.Cuidadores = new SelectList(_context.Cuidadores, "IdCuidadores", "Nombre");
            ViewBag.Animales = new SelectList(_context.Animales, "IdAnimal", "Nombre");
        }
    }
}