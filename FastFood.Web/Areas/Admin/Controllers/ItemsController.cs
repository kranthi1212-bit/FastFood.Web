using FastFood.Repository;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.Areas.Admin.Controllers
{
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var items=_context.Items.Include(x=>x.Category).Include(y=>y.SubCategory).ToList(); 
            return View(items);
        }
        public IActionResult Create()
        {
            ItemViewModel vm = new ItemViewModel();
            ViewBag.Category=new SelectList(_context.Categories,"Id","Title");
            return View(vm);
        }

        public IActionResult GetSubCategory(int categoryId)
        {
            var category=_context.SubCategories.Where(x=>x.CategoryId==categoryId).FirstOrDefault();
            return Json(category);
        }
    }
}
