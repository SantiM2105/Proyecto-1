using System.ComponentModel.DataAnnotations;

namespace Proyecto_1.Models
{
    public class Visitantes
    {
        [Key]
        public int IdVisitante { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre Completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El documento es obligatorio")]
        [StringLength(20)]
        [Display(Name = "Documento de Identidad")]
        public string Documento { get; set; }

        [StringLength(50)]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(15)]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        // Navegación
        public ICollection<MovimientoVisitas>? MovimientoVisitas { get; set; }
    }
}