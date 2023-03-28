using Microsoft.EntityFrameworkCore.Metadata;

namespace Rocosa.Models.ViewModels
{
    public class DetalleVM
    {

        //Inicializamos aqui para no hacerlo en el controlador  
        public DetalleVM()
        {
            Producto= new Producto();

        }

        public Producto Producto  { get; set; }
        public bool ExisteEnCarro { get; set; }
    }
}
