using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocosa.Datos;
using Rocosa.Models;
using Rocosa.Models.ViewModels;
using System.Diagnostics;

namespace Rocosa.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            HomeVM HomeVM    = new HomeVM()
            { 
              Productos= _db.Producto.Include(c=>c.Categoria).Include(t=>t.TipoAplicacion),
              Categorias = _db.Categoria
            
            };   
            return View(HomeVM);
        }

        [HttpPost]
        public IActionResult Detalle(int Id)
        {
            DetalleVM detalleVM = new DetalleVM()
            {
                Producto = _db.Producto.Include(c => c.Categoria).Include(t => t.TipoAplicacion)
                  .Where(p => p.Id == Id).FirstOrDefault(),

                ExisteEnCarro = false

            };
            return View(detalleVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}