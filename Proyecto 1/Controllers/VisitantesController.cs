using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_1.Data;
using Proyecto_1.Models;

namespace Proyecto_1.Controllers
{
    public class VisitantesController : Controller
    {
        private readonly SelvaDbContext _context;

        public VisitantesController(SelvaDbContext context)
        {
            _context = context;
        }

        // GET: Visitantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Visitantes.ToListAsync());
        }

        // GET: Visitantes/Detalles/5
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitante = await _context.Visitantes
                .FirstOrDefaultAsync(m => m.IdVisitante == id);

            if (visitante == null)
            {
                return NotFound();
            }

            return View(visitante);
        }

        // GET: Visitantes/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Visitantes/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("IdVisitante,Nombre,Documento,Email,Telefono")] Visitantes visitante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(visitante);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Visitante creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            return View(visitante);
        }

        // GET: Visitantes/Editar/5
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitante = await _context.Visitantes.FindAsync(id);
            if (visitante == null)
            {
                return NotFound();
            }
            return View(visitante);
        }

        // POST: Visitantes/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("IdVisitante,Nombre,Documento,Email,Telefono")] Visitantes visitante)
        {
            if (id != visitante.IdVisitante)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visitante);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Visitante actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitanteExists(visitante.IdVisitante))
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
            return View(visitante);
        }

        // GET: Visitantes/Eliminar/5
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitante = await _context.Visitantes
                .FirstOrDefaultAsync(m => m.IdVisitante == id);

            if (visitante == null)
            {
                return NotFound();
            }

            return View(visitante);
        }

        // POST: Visitantes/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var visitante = await _context.Visitantes.FindAsync(id);
            if (visitante != null)
            {
                _context.Visitantes.Remove(visitante);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Visitante eliminado exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool VisitanteExists(int id)
        {
            return _context.Visitantes.Any(e => e.IdVisitante == id);
        }
    }
}