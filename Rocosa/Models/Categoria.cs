using System.ComponentModel.DataAnnotations;

namespace Rocosa.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Ingrese el nombre de la categoria")]
        public string? NombreCategoria { get; set; }

        [Required(ErrorMessage ="Requiere la Orden")]
        [Range(1, int.MaxValue, ErrorMessage ="El orden debe ser mayor a cero")]
        public int MostrarOrden { get; set; }
    }
}
