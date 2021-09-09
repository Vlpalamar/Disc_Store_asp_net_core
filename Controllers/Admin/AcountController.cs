using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Disc_Store.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Disc_Store.Controllers.Admin
{
    public class AccountController : Controller
    {

        [Route("~/Admin/Account/RoleIndex")]
        public IActionResult RoleIndex()
        {
            //ViewBag.AllRoles = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
            return View("~/Views/Admin/Account/RoleIndex.cshtml", _roleManager.Roles.ToList());
        }
        [Route("~/Admin/Account/RoleCreate")]
        public IActionResult RoleCreate()
        {
            //ViewBag.AllRoles = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
            return View("~/Views/Admin/Account/RoleCreate.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleIndex");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }

        public class VM_RegisterUser
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public List<IdentityRole> Roles { get; set; }
        }

        private readonly UserManager<MyUser> _userManager;
        private readonly SignInManager<MyUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AccountController(UserManager<MyUser> userManager, SignInManager<MyUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;

        }
        [Route("~/Admin/Account")]
        public IActionResult Index()
        {
            return View("~/Views/Admin/Account/Index.cshtml",_userManager.Users);
        }
      ///  [HttpGet]
        [Route("~/Admin/Account/Create")]
        public IActionResult Create()
        {
            ViewBag.AllRoles = new SelectList(_roleManager.Roles.ToList(), "Id", "Name");
            return View("~/Views/Admin/Account/Create.cshtml");
        }

        [HttpPost]
        [Route("~/Admin/Account/Create")]
        public async Task<IActionResult> Create([Bind("Email,Password")]VM_RegisterUser NewUser,string[] addRoles)
        {
            if (ModelState.IsValid)
            {
                MyUser user = new MyUser { Email = NewUser.Email, UserName = NewUser.Email };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, NewUser.Password);
                if (result.Succeeded)
                {

                   // addRoles = NewUser.Roles.Select(r => r.Name).ToList();
                    await _userManager.AddToRolesAsync(user, addRoles);


                    return   RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View("~/Views/Admin/Account/Create.cshtml",NewUser);
        }


    }
}
