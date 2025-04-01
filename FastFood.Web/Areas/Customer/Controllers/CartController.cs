using System.Security.Claims;
using FastFood.Modals;
using FastFood.Repository;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {      
        private readonly ApplicationDbContext _context;
        [BindProperty]
        public CartOrderViewModel details { get; set; }
        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            details = new CartOrderViewModel()
            {
               OrderHeader =new OrderHeader()
            };
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            details.ListofCart=_context.Carts.Where(x=>x.ApplicationUserId==claims.Value).ToList();
            if( details.ListofCart!=null)
            {
                foreach(var cart in details.ListofCart)
                {
                    details.OrderHeader.OrderTotal += cart.Item.Price * cart.Count;
                }
            }
            return View();
        }
    }
}
