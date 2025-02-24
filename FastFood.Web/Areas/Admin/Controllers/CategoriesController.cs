using System.Security.Policy;
using FastFood.Modals;
using FastFood.Repository;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        public readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
         var caterylist=_context.Categories.ToList().Select(x => new CategoryViewModel()
            {
                Id = x.Id,
                Title = x.Title,
            }).ToList();
            return View(caterylist);
        }
        public IActionResult Create()
        {
            CategoryViewModel category = new CategoryViewModel();
            return View(category);
        }
        [HttpPost]
        public IActionResult Create(CategoryViewModel vm)
        {
            Category model=new Category();
            model.Title = vm.Title;
            _context.Categories.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var viewmodel = _context.Categories.Where(x => x.Id == id).
                Select(x => new CategoryViewModel()
                {
                    Id = x.Id,
                    Title = x.Title,
                }).FirstOrDefault();
            return View(viewmodel);
        }
        [HttpPost]
        public IActionResult Edit(CategoryViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var categoryFromDb = _context.Categories.FirstOrDefault(x => x.Id == vm.Id);
                if(categoryFromDb != null)
                {
                    categoryFromDb.Title = vm.Title;
                    _context.Categories.Update(categoryFromDb);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if(category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();

            }
            return RedirectToAction("Index");
        }
    }
}
