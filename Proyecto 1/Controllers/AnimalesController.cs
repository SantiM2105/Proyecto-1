using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_1.Data;
using Proyecto_1.Models;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout;
namespace Proyecto_1.Controllers

{
    public class AnimalesController : Controller
    {
        private readonly SelvaDbContext _context;

        public AnimalesController(SelvaDbContext context)
        {
            _context = context;
        }

        // GET: Animales
        public async Task<IActionResult> Index()
        {
            var animales = await _context.Animales
                .Include(a => a.Cuidador)
                .ToListAsync();
            return View(animales);
        }

        // GET: Animales/Detalles/5
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animales
                .Include(a => a.Cuidador)
                .FirstOrDefaultAsync(m => m.IdAnimal == id);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // GET: Animales/Crear
        public IActionResult Crear()
        {
            ViewData["IdCuidador"] = new SelectList(_context.Cuidadores, "IdCuidadores", "Nombre");
            return View();
        }

        // POST: Animales/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear([Bind("IdAnimal,Nombre,Especie,Habitat,Edad,EstadoSalud,IdCuidador")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(animal);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Animal registrado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCuidador"] = new SelectList(_context.Cuidadores, "IdCuidadores", "Nombre", animal.IdCuidador);
            return View(animal);
        }

        // GET: Animales/Editar/5
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animales.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }
            ViewData["IdCuidador"] = new SelectList(_context.Cuidadores, "IdCuidadores", "Nombre", animal.IdCuidador);
            return View(animal);
        }

        // POST: Animales/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("IdAnimal,Nombre,Especie,Habitat,Edad,EstadoSalud,IdCuidador")] Animal animal)
        {
            if (id != animal.IdAnimal)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(animal);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Animal actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnimalExists(animal.IdAnimal))
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
            ViewData["IdCuidador"] = new SelectList(_context.Cuidadores, "IdCuidadores", "Nombre", animal.IdCuidador);
            return View(animal);
        }

        // GET: Animales/Eliminar/5
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var animal = await _context.Animales
                .Include(a => a.Cuidador)
                .FirstOrDefaultAsync(m => m.IdAnimal == id);

            if (animal == null)
            {
                return NotFound();
            }

            return View(animal);
        }

        // POST: Animales/Eliminar/5
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var animal = await _context.Animales.FindAsync(id);
            if (animal != null)
            {
                _context.Animales.Remove(animal);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Animal eliminado exitosamente";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AnimalExists(int id)
        {
            return _context.Animales.Any(e => e.IdAnimal == id);
        }


        public async Task<IActionResult> Reporte()
        {
            return View(await _context.Animales.ToListAsync());
        }
        public async Task<IActionResult> GenerarPdf()
        {
            var animales = await _context.Animales.ToListAsync();

            using (var flujo = new MemoryStream())
            {
                PdfWriter escribir = new PdfWriter(flujo);
                PdfDocument pdf = new PdfDocument(escribir);
                Document documento = new Document(pdf);

                documento.Add(new Paragraph("Reporte de Animales del Zoológico")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20));

                // Tabla con columnas para Animales (ajustadas a TU modelo)
                Table tabla = new Table(new float[] { 1, 2, 2, 2, 1, 2, 2 }).UseAllAvailableWidth();

                // ENCABEZADOS para Animales (NO para Clientes)
                tabla.AddHeaderCell("ID");
                tabla.AddHeaderCell("Nombre");
                tabla.AddHeaderCell("Especie");
                tabla.AddHeaderCell("Hábitat");
                tabla.AddHeaderCell("Edad");
                tabla.AddHeaderCell("Salud");
                tabla.AddHeaderCell("Cuidadores");

                foreach (var animal in animales) 
                {
                    tabla.AddCell(animal.IdAnimal.ToString());
                    tabla.AddCell(animal.Nombre);
                    tabla.AddCell(animal.Especie);
                    tabla.AddCell(animal.Habitat);
                    tabla.AddCell(animal.Edad.ToString());
                    tabla.AddCell(animal.EstadoSalud);
                    tabla.AddCell(animal.Cuidador.ToString());
                }

                documento.Add(tabla);

                documento.Add(new Paragraph("Reporte generado el " + DateTime.Now.ToString("dd/MM/yyyy"))
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetFontSize(12));

                documento.Close();
                byte[] pdfBytes = flujo.ToArray();

                return File(pdfBytes, "application/pdf");

            }
        }
        public IActionResult MostrarPdf()
        {
            return View();
        }





    }
}