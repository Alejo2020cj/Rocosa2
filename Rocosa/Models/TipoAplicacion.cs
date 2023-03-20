using System.ComponentModel.DataAnnotations;

namespace Rocosa.Models
{
    public class TipoAplicacion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Ingrese el Nombre")]
        public string Nombre { get; set; }
    }
}
