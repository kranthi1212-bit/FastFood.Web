using System.Security.Cryptography;
using FastFood.Modals;
using FastFood.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CouponController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CouponController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var coupons = _context.Coupons.ToList();
            return View(coupons);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                var files = Request.Form.Files;
                byte[] photo = null;
                using (var filestram = files[0].OpenReadStream())
                {
                    using (var memorystream = new MemoryStream())
                    {
                        filestram.CopyTo(memorystream);
                        photo = memorystream.ToArray();
                    }
                }  
                coupon.CouponPicture = photo;   
                _context.Coupons.Add(coupon);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coupon);
        }
        public IActionResult Delete(int id)
        {
            var coupen=_context.Coupons.Where(x=>x.Id == id).FirstOrDefault();  
            if(coupen == null)
            {
                return NotFound();  
            }
            _context.Coupons.Remove(coupen);    
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var coupen = _context.Coupons.Where(x => x.Id == id).FirstOrDefault();
            if (coupen == null)
            {
                return NotFound();
            }
            return View(coupen);
        }
        [HttpPost]  
        public IActionResult Edit(Coupon model)
        {
            var coupon = _context.Coupons.Where(x=>x.Id== model.Id).FirstOrDefault();   

            if (ModelState.IsValid)
            {
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] photo = null;
                    using (var filestram = files[0].OpenReadStream())
                    {
                        using (var memorystream = new MemoryStream())
                        {
                            filestram.CopyTo(memorystream);
                            photo = memorystream.ToArray();
                        }
                    }
                    coupon.CouponPicture = photo;
                } 
                coupon.MinimumAmount=model.MinimumAmount;
                coupon.Discount=model.Discount;
                coupon.IsActive=model.IsActive; 
                coupon.Title=model.Title;
                coupon.Type=model.Type;
                _context.Coupons.Update(coupon);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
