using System.Security.Claims;
using AspNetCoreGeneratedDocument;
using FastFood.Modals;
using FastFood.Repository;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace FastFood.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        public readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            //ItemListViewModel vm = new ItemListViewModel()
            //{
            //    Items = await _context.Items.Include(x => x.Category)
            //          .Include(y => y.SubCategory).ToListAsync(),
            //    Categorys = await _context.Categories.ToListAsync(),
            //    Coupons = await _context.Coupons.Where(c => c.IsActive == true).ToListAsync(),
            //};
            return View(_context.Items);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var itemdb = await _context.Items.
                Include(x => x.Category).
                Include(y => y.SubCategory).
                FirstOrDefaultAsync(z => z.Id == id);
            if (itemdb == null)
            {
                return NotFound();
            }
            var cart = new Cart()
            {
                Item=itemdb,
                ItemId=itemdb.Id,
                Count=1
            };

            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(Cart cart)
        {
            cart.Id = 0;
            if (ModelState.IsValid)
            {
                var claimidentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);

                cart.ApplicationUserId = claim.Value;

                cart.Item =await _context.Items.FindAsync(cart.ItemId);

                if (cart.Item == null)
                {
                    return NotFound("Item not found");
                }

                cart.ItemId = cart.Item.Id;  
                
                var cartfromdb = await _context.Carts.Where(x => x.ApplicationUserId == cart.ApplicationUserId
                && x.ItemId == cart.ItemId).FirstOrDefaultAsync();

                if (cartfromdb == null)
                {            
                    _context.Carts.Add(cart);
                }
                else
                {
                    cartfromdb.Count += cart.Count;
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cart);

        }
    }
}
