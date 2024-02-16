using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UserManagementWithIdentiy.Models;
using UserManagementWithIdentiy.ViewModels;

namespace UserManagementWithIdentiy.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(x => new UsersViewModel
            {
                id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                UserName = x.UserName,
                ProfilePicture = x.ProfilePicture,
                RoleName = _userManager.GetRolesAsync(x).Result
            }).ToListAsync();
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            var _roles = await _roleManager.Roles.ToListAsync();
            var viewModel = new AddUserFormViewModel()
            {
                Roles = _roles.Select(x => new RoleViewModel { RoleId = x.Id, RoleName = x.Name }).ToList()
            };
            return View("AddUser",viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAddUser(AddUserFormViewModel _user)
        {
            if (await _userManager.FindByEmailAsync(_user.Email)!=null)
            {
                ModelState.AddModelError("Email","Email is exist, try another...");
                return View("AddUser",_user);
            }
            if(await _userManager.FindByNameAsync(_user.UserName)!=null)
            {
                ModelState.AddModelError("UserName", "User Name is exist, try another...");
                return View("AddUser",_user);
            }
            if(!_user.Roles.Any(x=>x.Selected))
            {
                ModelState.AddModelError("Roles", "choose Role..");
                return View("AddUser", _user);
            }
            
            if (!ModelState.IsValid)
            {
                return View("AddUser", _user);
            }
            var user = new ApplicationUser()
            {
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                Email = _user.Email,
                UserName = _user.UserName,
                
            };
            var result = await _userManager.CreateAsync(user,_user.Password);
            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("Users",error.Description);
                }
            }
            await _userManager.AddToRolesAsync(user,_user.Roles.Where(x=>x.Selected).Select(x=>x.RoleName).ToList());
            return RedirectToAction("Index");
        }
    }
}
