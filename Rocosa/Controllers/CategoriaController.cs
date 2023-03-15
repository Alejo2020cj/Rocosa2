using Microsoft.AspNetCore.Mvc;
using Rocosa.Datos;
using Rocosa.Models;

namespace Rocosa.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ApplicationDbContext _db;
            public CategoriaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Categoria> Lista = _db.Categoria;
            return View(Lista);
        }
    }
}
