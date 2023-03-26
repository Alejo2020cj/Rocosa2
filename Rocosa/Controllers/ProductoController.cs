using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocosa.Datos;
using Rocosa.Models;
using Rocosa.Models.ViewModels;
using System.IO;

namespace Rocosa.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _db;
        //Para pasar imagenes
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (productoVM.Producto.Id == 0)
                {
                    //crear
                    string upload = webRootPath + Wc.ImegenRuta;
                    string fileName = Guid.NewGuid().ToString();    
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productoVM.Producto.ImageUrl = fileName + extension;
                    _db.Producto.Add(productoVM.Producto);
                
                }
                else
                {
                    //Actualizar
                    var objProducto = _db.Producto.AsNoTracking().FirstOrDefault(P => P.Id == productoVM.Producto.Id);
                    if (files.Count > 0)
                    {
                        //crear
                        string upload = webRootPath + Wc.ImegenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //Borra la imagen anterior
                        var anteriorFile = Path.Combine(upload, objProducto.ImageUrl);
                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }
                        // Fin de borrar de la imagen anterior

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productoVM.Producto.ImageUrl = fileName + extension;

                    }//En caso contrario si no carga la nueva imagen
                    else
                    {
                        productoVM.Producto.ImageUrl = objProducto.ImageUrl;
                    
                    }
                    _db.Producto.Update(productoVM.Producto);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }//if modelIsValid
             //Se llenan nuevamente las listas

            productoVM.CategoriaLista = _db.Categoria.Select(C => new SelectListItem
            {
                Text = C.NombreCategoria,
                Value = C.Id.ToString()

            });
            productoVM.TipoAplicacionLista = _db.TipoAplicacion.Select(C => new SelectListItem
            {
                Text = C.Nombre,
                Value = C.Id.ToString()

            });
            return View(productoVM);
        }

        [HttpGet]
        public IActionResult Eliminar(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            
            }
            Producto producto = _db.Producto.Include(c=>c.Categoria).Include(t=>t.TipoAplicacion).FirstOrDefault();

            if (producto == null)
            { 
             return NotFound();
            
            }
            return View(producto);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(Producto producto)
        { 
            if (ModelState.IsValid) 
            {
              return NotFound();
            
            }
            //Eliminar la imagen

            string upload = _webHostEnvironment.WebRootPath + Wc.ImegenRuta;

            //Borra la imagen anterior
            var anteriorFile = Path.Combine(upload, producto.ImageUrl);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }
            //Borrar el producto
            _db.Remove(producto);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

    }
}
