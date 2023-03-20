using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocosa.Datos;
using Rocosa.Models;
using Rocosa.Models.ViewModels;

namespace Rocosa.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductoController(ApplicationDbContext db)
        {
            _db = db;
        }
    
        public IActionResult Index()
        {
            IEnumerable<Producto> Lista = _db.Producto.Include(c => c.Categoria).Include(c => c.TipoAplicacion);
            return View(Lista);
        }

        //Get
        public IActionResult Upsert(int? Id) 
        {
            //  IEnumerable<SelectListItem> categoriaDropDown = _db.Categoria.Select(c => new SelectListItem
            //  {
            //      Text = c.NombreCategoria,
            //      Value = c.Id.ToString(),

            //  });
            //  ViewBag.categoriaDropDown = categoriaDropDown;

            //Producto producto = new Producto();
            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                CategoriaLista = _db.Categoria.Select(C => new SelectListItem
                {
                    Text = C.NombreCategoria,
                    Value = C.Id.ToString()

                }),
                TipoAplicacionLista = _db.TipoAplicacion.Select(C => new SelectListItem
                {
                    Text = C.Nombre,
                    Value = C.Id.ToString()

                }),
            };

            if (Id == null)
            {
                //Crear Nuevo Producto
                return View(productoVM);

            }
            else
            {
                productoVM.Producto = _db.Producto.Find(Id) ;
                if (productoVM.Producto == null)
                {
                    return NotFound();
                
                }
                return View(productoVM);
            }
        }



    }
}
