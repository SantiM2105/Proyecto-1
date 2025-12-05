using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_1.Data;
using Proyecto_1.Models;

namespace Proyecto_1.Controllers
{
    public class MovimientoVisitasController : Controller
    {
        private readonly SelvaDbContext _context;

        public MovimientoVisitasController(SelvaDbContext context)
        {
            _context = context;
        }

        // GET: MovimientoVisitas
        public async Task<IActionResult> Index()
        {
            var movimientos = await _context.MovimientoVisitas
                .Include(m => m.Visitante)
                .Include(m => m.Animal)
                .Include(m => m.Cuidador)
                .OrderByDescending(m => m.FechaVisita)
                .ToListAsync();

            return View(movimientos);
        }

        // GET: MovimientoVisitas/Detalles/5
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.MovimientoVisitas
                .Include(m => m.Visitante)
                .Include(m => m.Animal)
                .Include(m => m.Cuidador)
                .FirstOrDefaultAsync(m => m.IdMovimiento == id);

            if (movimiento == null)
            {
                return NotFound();
            }

            return View(movimiento);
        }

        // GET: MovimientoVisitas/Crear
        public IActionResult Crear()
        {
            CargarListasDesplegables();
            return View();
        }

        // POST: MovimientoVisitas/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("IdMovimiento,IdVisitante,IdAnimal,IdCuidador,FechaVisita,Observaciones,Duracion")] MovimientoVisitas movimiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movimiento);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Visita registrada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            CargarListasDesplegables(movimiento);
            return View(movimiento);
        }

        // GET: MovimientoVisitas/Editar/5
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.MovimientoVisitas.FindAsync(id);
            if (movimiento == null)
            {
                return NotFound();
            }
            CargarListasDesplegables(movimiento);
            return View(movimiento);
        }

        // POST: MovimientoVisitas/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("IdMovimiento,IdVisitante,IdAnimal,IdCuidador,FechaVisita,Observaciones,Duracion")] MovimientoVisitas movimiento)
        {
            if (id != movimiento.IdMovimiento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movimiento);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Visita actualizada exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovimientoExists(movimiento.IdMovimiento))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            CargarListasDesplegables(movimiento);
            return View(movimiento);
        }

        // GET: MovimientoVisitas/Eliminar/5
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movimiento = await _context.MovimientoVisitas
                .Include(m => m.Visitante)
                .Include(m => m.Animal)
                .Include(m => m.Cuidador)
                .FirstOrDefaultAsync(m => m.IdMovimiento == id);

            if (movimiento == null)
            {
                return NotFound();
            }

            return View(movimiento);
        }

        // POST: MovimientoVisitas/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
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

            return RedirectToAction(nameof(Index));
        }

        private bool MovimientoExists(int id)
        {
            return _context.MovimientoVisitas.Any(e => e.IdMovimiento == id);
        }

        private void CargarListasDesplegables(MovimientoVisitas? movimiento = null)
        {
            ViewData["IdVisitante"] = new SelectList(_context.Visitantes, "IdVisitante", "Nombre", movimiento?.IdVisitante);
            ViewData["IdAnimal"] = new SelectList(_context.Animales, "IdAnimal", "Nombre", movimiento?.IdAnimal);
            ViewData["IdCuidador"] = new SelectList(_context.Cuidadores, "IdCuidadores", "Nombre", movimiento?.IdCuidador);
        }
    }
}