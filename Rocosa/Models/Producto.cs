using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocosa.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Requiere nombre producto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Requiere Descripción corta")]
        public string DescripcionCorta { get; set; }

        [Required(ErrorMessage = "El precio del producto es requerida")]
        public string DescripcionProducto { get; set; }

        [Required(ErrorMessage = "El precio del producto es requerido")]
        public string Precio { get; set; }

        public string ImageUrl { get; set; }

        //Foreing key
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set;}

        public int TipoAplicacionId { get; set; }
        [ForeignKey("TipoAplicacionId")]
        public virtual TipoAplicacion TipoAplicacion { get; set; }

    }
}
