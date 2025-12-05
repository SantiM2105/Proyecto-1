using System.ComponentModel.DataAnnotations;

namespace Proyecto_1.Models
{
    public class Cuidadores
    {
       
        
            [Key]
            public int IdCuidadores { get; set; }

            [Required, StringLength(50)]
            public string Nombre { get; set; }

            [StringLength(50)]
            public string Apellido { get; set; }

            [StringLength(15)]
            public string Telefono { get; set; }

            
         
    }
}


