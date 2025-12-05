using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_1.Data;
using Proyecto_1.Models;

namespace Proyecto_1.Controllers
{
    public class CuidadoresController : Controller
    {
        private readonly SelvaDbContext _context;

        public CuidadoresController(SelvaDbContext context)
        {
            _context = context;
        }

        // GET: Cuidadores
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cuidadores.ToListAsync());
        }

        // GET: Cuidadores/Detalles/5
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuidador = await _context.Cuidadores
                .FirstOrDefaultAsync(m => m.IdCuidadores == id);

            if (cuidador == null)
            {
                return NotFound();
            }

            return View(cuidador);
        }

        // GET: Cuidadores/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Cuidadores/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("IdCuidadores,Nombre,Apellido,Telefono")] Cuidadores cuidador)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cuidador);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Cuidador creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            return View(cuidador);
        }

        // GET: Cuidadores/Editar/5
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuidador = await _context.Cuidadores.FindAsync(id);
            if (cuidador == null)
            {
                return NotFound();
            }
            return View(cuidador);
        }

        // POST: Cuidadores/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("IdCuidadores,Nombre,Apellido,Telefono")] Cuidadores cuidador)
        {
            if (id != cuidador.IdCuidadores)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cuidador);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Cuidador actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuidadorExists(cuidador.IdCuidadores))
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
            return View(cuidador);
        }

        // GET: Cuidadores/Eliminar/5
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuidador = await _context.Cuidadores
                .FirstOrDefaultAsync(m => m.IdCuidadores == id);

            if (cuidador == null)
            {
                return NotFound();
            }

            return View(cuidador);
        }

        // POST: Cuidadores/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var cuidador = await _context.Cuidadores.FindAsync(id);
            if (cuidador != null)
            {
                _context.Cuidadores.Remove(cuidador);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Cuidador eliminado exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CuidadorExists(int id)
        {
            return _context.Cuidadores.Any(e => e.IdCuidadores == id);
        }
    }
}