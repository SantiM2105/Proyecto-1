using Microsoft.AspNetCore.Mvc.Rendering;

namespace Proyecto_1.Models
{
    public class RegistroVisitaViewModel
    {
        public MovimientoVisitas Movimiento { get; set; }
        public SelectList? Visitantes { get; set; }
        public SelectList? Animales { get; set; }
        public SelectList? Cuidadores { get; set; }

        // Información detallada para mostrar
        public string? NombreVisitante { get; set; }
        public string? DocumentoVisitante { get; set; }
        public string? NombreAnimal { get; set; }
        public string? EspecieAnimal { get; set; }
        public string? NombreCuidador { get; set; }
    }
}