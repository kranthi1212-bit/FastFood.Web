using System.Security.Claims;
using System.Security.Policy;
using FastFood.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var claimsidentity =(ClaimsIdentity)this.User.Identity;
            var claim=claimsidentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_context.ApplicationUsers.Where(x => x.Id != claim.Value).ToList());
        }
    }
}
