using System.ComponentModel.DataAnnotations;

namespace Proyecto_1.Models
{
    public class MovimientoVisitas
    {
        [Key]
        public int IdMovimiento { get; set; }

        [Required]
        [Display(Name = "Visitante")]
        public int IdVisitante { get; set; }

        [Required]
        [Display(Name = "Animal Visitado")]
        public int IdAnimal { get; set; }

        [Required]
        [Display(Name = "Cuidador Guía")]
        public int IdCuidador { get; set; }

        [Required]
        [Display(Name = "Fecha de Visita")]
        [DataType(DataType.Date)]
        public DateTime FechaVisita { get; set; }

        [StringLength(200)]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        [Display(Name = "Duración (minutos)")]
        public int? Duracion { get; set; }

        // Navegación
        public Visitantes? Visitante { get; set; }
        public Animal? Animal { get; set; }
        public Cuidadores? Cuidador { get; set; }
    }
}