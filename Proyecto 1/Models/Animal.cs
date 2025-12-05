using System.ComponentModel.DataAnnotations;

namespace Proyecto_1.Models
{
    public class Animal
    {
        [Key]
        public int IdAnimal { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre del Animal")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La especie es obligatoria")]
        [StringLength(100)]
        [Display(Name = "Especie")]
        public string Especie { get; set; }

        [Required(ErrorMessage = "El hábitat es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Hábitat")]
        public string Habitat { get; set; }

        [Required(ErrorMessage = "La edad es obligatoria")]
        [Display(Name = "Edad (años)")]
        public int Edad { get; set; }

        [StringLength(50)]
        [Display(Name = "Estado de Salud")]
        public string? EstadoSalud { get; set; }

        [Display(Name = "Cuidador Asignado")]
        public int? IdCuidador { get; set; }

        // Navegación
        public Cuidadores? Cuidador { get; set; }
        public ICollection<MovimientoVisitas>? MovimientoVisitas { get; set; }
    }
}