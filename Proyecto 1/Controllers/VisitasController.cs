using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_1.Data;
using Proyecto_1.Models;

namespace Proyecto_1.Controllers
{
    public class VisitasController : Controller
    {
        private readonly SelvaDbContext _context;

        public VisitasController(SelvaDbContext context)
        {
            _context = context;
        }

        // GET: Visitas/Registrar
        public IActionResult Registrar()
        {
            var viewModel = new RegistroVisitaViewModel
            {
                Movimiento = new MovimientoVisitas { FechaVisita = DateTime.Now },
                Visitantes = new SelectList(_context.Visitantes, "IdVisitante", "Nombre"),
                Animales = new SelectList(_context.Animales, "IdAnimal", "Nombre"),
                Cuidadores = new SelectList(_context.Cuidadores, "IdCuidadores", "Nombre")
            };
            return View(viewModel);
        }

        // POST: Visitas/Registrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(RegistroVisitaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(viewModel.Movimiento);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Visita registrada exitosamente";
                return RedirectToAction("Index", "MovimientoVisitas");
            }

            // Recargar listas si hay error
            viewModel.Visitantes = new SelectList(_context.Visitantes, "IdVisitante", "Nombre", viewModel.Movimiento.IdVisitante);
            viewModel.Animales = new SelectList(_context.Animales, "IdAnimal", "Nombre", viewModel.Movimiento.IdAnimal);
            viewModel.Cuidadores = new SelectList(_context.Cuidadores, "IdCuidadores", "Nombre", viewModel.Movimiento.IdCuidador);

            return View(viewModel);
        }

        // GET: Visitas/Eliminar
        public async Task<IActionResult> Eliminar()
        {
            var movimientos = await _context.MovimientoVisitas
                .Include(m => m.Visitante)
                .Include(m => m.Animal)
                .Include(m => m.Cuidador)
                .OrderByDescending(m => m.FechaVisita)
                .Take(50) // Últimas 50 visitas
                .ToListAsync();

            return View(movimientos);
        }

        // POST: Visitas/EliminarConfirmado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var movimiento = await _context.MovimientoVisitas.FindAsync(id);
            if (movimiento != null)
            {
                _context.MovimientoVisitas.Remove(movimiento);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Visita eliminada exitosamente";
            }
            else
            {
                TempData["Error"] = "No se encontró la visita";
            }

            return RedirectToAction(nameof(Eliminar));
        }

        // API para obtener datos del visitante
        [HttpGet]
        public async Task<JsonResult> ObtenerVisitante(int id)
        {
            var visitante = await _context.Visitantes.FindAsync(id);
            if (visitante == null)
            {
                return Json(new { success = false });
            }

            return Json(new
            {
                success = true,
                nombre = visitante.Nombre,
                documento = visitante.Documento,
                telefono = visitante.Telefono
            });
        }

        // API para obtener datos del animal
        [HttpGet]
        public async Task<JsonResult> ObtenerAnimal(int id)
        {
            var animal = await _context.Animales
                .Include(a => a.Cuidador)
                .FirstOrDefaultAsync(a => a.IdAnimal == id);

            if (animal == null)
            {
                return Json(new { success = false });
            }

            return Json(new
            {
                success = true,
                nombre = animal.Nombre,
                especie = animal.Especie,
                habitat = animal.Habitat,
                cuidador = animal.Cuidador?.Nombre
            });
        }
    }
}