﻿using FastFood.Modals;
using FastFood.Repository;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;
        public ItemsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var items = _context.Items.Include(x => x.Category).Include(y => y.SubCategory)
                         .Select(model => new ItemViewModel()
                         {
                             Id = model.Id,
                             Title = model.Title,
                             Description = model.Description,
                             Price = model.Price,
                             CategoryId = model.CategoryId,
                             SubCategoryId = model.SubCategoryId,
                             CategoryTitle = model.Category.Title,
                             SubCategoryTitle = model.SubCategory.Title
                         }).ToList();
            return View(items);
        }
        public IActionResult Create()
        {
            ItemViewModel vm = new ItemViewModel();
            ViewBag.Category = new SelectList(_context.Categories, "Id", "Title");
            ViewBag.SubCategory = new SelectList(_context.SubCategories, "Id", "Title");
            return View(vm);
        }
        public IActionResult GetSubCategory(int categoryId)
        {
            var category = _context.SubCategories.Where(x => x.CategoryId == categoryId).FirstOrDefault();
            return Json(category);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ItemViewModel vm)
        {
            Item model = new Item();
            if (vm.ImageUrl != null && vm.ImageUrl.Length > 0)
            {
                var uploadDir = @"Images/Items";
                var filename = Guid.NewGuid().ToString() + "-" + vm.ImageUrl.FileName;
                var filepath = Path.Combine(_webHostEnvironment.WebRootPath, uploadDir, filename);
                await vm.ImageUrl.CopyToAsync(new FileStream(filepath, FileMode.Create));
                model.Image = "/" + uploadDir + "/" + filename;
            }
            model.Price = vm.Price;
            model.Title = vm.Title;
            model.Description = vm.Description;
            model.CategoryId = vm.CategoryId;
            model.SubCategoryId = vm.SubCategoryId;
            _context.Items.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            ItemViewModel model = new ItemViewModel();
            var item =_context.Items.Where(x=>x.Id== id).FirstOrDefault();
            if (item != null)
            {
                model.Title = item.Title;
                model.Description = item.Description;
                model.Price = item.Price;
                model.CategoryId = item.CategoryId;
                ViewBag.Category = new SelectList(_context.Categories, "Id", "Title", item.CategoryId);
                ViewBag.SubCategory = new SelectList(_context.SubCategories, "Id", "Title", item.SubCategoryId);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ItemViewModel vm)
        {
            Item model = new Item();
            if (vm.ImageUrl != null && vm.ImageUrl.Length > 0)
            {
                var uploadDir = @"Images/Items";
                var filename = Guid.NewGuid().ToString() + "-" + vm.ImageUrl.FileName;
                var filepath = Path.Combine(_webHostEnvironment.WebRootPath, uploadDir, filename);
                await vm.ImageUrl.CopyToAsync(new FileStream(filepath, FileMode.Create));
                model.Image = "/" + uploadDir + "/" + filename;
            }
            model.Price = vm.Price;
            model.Title = vm.Title;
            model.Description = vm.Description;
            model.CategoryId = vm.CategoryId;
            model.SubCategoryId = vm.SubCategoryId;

            _context.Items.Update(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
