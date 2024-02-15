using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

        [HttpGet]
        public async Task<IActionResult> ManageRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _roleManager.Roles.ToListAsync();

            var viewModel = new UserRoleViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = roles.Select(x=> new RoleViewModel() { RoleId = x.Id, RoleName = x.Name ,Selected =_userManager.IsInRoleAsync(user,x.Name).Result }).ToList(),
                
            };
            
            return View("ManageRole",viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveManageRole(UserRoleViewModel _user)
        {
            var user = await _userManager.FindByIdAsync(_user.UserId);

            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in _user.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.Selected)
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (!userRoles.Any(r => r == role.RoleName) && role.Selected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
            }

            return RedirectToAction(nameof(Index),"Users");
        }
       
    }
}
