using System.ClientModel.Primitives;
using FastFood.Modals;
using FastFood.Repository;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var subcategories = _context.SubCategories.Include(x=>x.Category).ToList();    
            return View(subcategories);
        }
        public IActionResult Create()
        {
            SubCatogoryViewModel vm=new SubCatogoryViewModel();
            ViewBag.category=new SelectList(_context.Categories,"Id","Title");
            return View(vm);
        }
        [HttpPost]
        public IActionResult Create(SubCatogoryViewModel vm)
        {
            SubCategory model = new SubCategory();
            if (ModelState.IsValid)
            {
                model.Title = vm.Title;
                model.CategoryId = vm.CategoryId;
                _context.SubCategories.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(vm);
        }
        public IActionResult Edit(int id)
        {
            SubCatogoryViewModel vm = new SubCatogoryViewModel();
            var subcategory = _context.SubCategories.Where(x => x.Id == id).FirstOrDefault();
            if (subcategory != null)
            {
                vm.Id = subcategory.Id;
                vm.Title = subcategory.Title;
                ViewBag.category = new SelectList(_context.Categories, "Id", "Title", subcategory.CategoryId);
            }
            return View(vm);
        }
        [HttpPost]
        public IActionResult Edit(SubCatogoryViewModel vm)
        {
            SubCategory model = new SubCategory();
            if (ModelState.IsValid)
            {
                model.Title = vm.Title;
                model.CategoryId = vm.CategoryId;
                _context.SubCategories.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }
        public IActionResult Delete(int id)
        {
            var subcategory = _context.SubCategories.Where(x => x.Id == id).FirstOrDefault();
            if (subcategory != null)
            {
               _context.SubCategories.Remove(subcategory);
               _context.SaveChanges();
              
            }
            return RedirectToAction("Index");
        }
    }
}
