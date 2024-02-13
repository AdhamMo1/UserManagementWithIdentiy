using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UserManagementWithIdentiy.Models;
using UserManagementWithIdentiy.ViewModels;

namespace UserManagementWithIdentiy.Controllers
{
    public class RolesController : Controller
    {
        
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager , UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var Roles = await _roleManager.Roles.ToListAsync();
            return View(Roles);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> add(RoleFormViewModel Role)
        {
            if (!ModelState.IsValid)
            {
                return View("Index",await _roleManager.Roles.ToListAsync());
            }
            if(await _roleManager.RoleExistsAsync(Role.Name)) {
                ModelState.AddModelError("Name", "Role is Exist....");
                return View("Index", await _roleManager.Roles.ToListAsync());
            }
            IdentityRole _Role = new IdentityRole();
            _Role.Name= Role.Name;
             await _roleManager.CreateAsync(_Role);
             return RedirectToAction("Index");
        }

       
    }
}
