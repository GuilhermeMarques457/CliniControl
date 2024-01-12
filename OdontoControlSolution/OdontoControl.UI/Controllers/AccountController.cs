using ContactsManager.UI.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OdontoControl.Core.Domain.IdentityEntities;
using OdontoControl.Core.DTO.AccountDTO;
using OdontoControl.Core.DTO.AccountDTO.ManagerDTO;
using OdontoControl.Core.Enums;
using OdontoControl.UI.Usefull;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace OdontoControl.UI.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        public readonly UserManager<ApplicationUser> _userManager;
        public readonly SignInManager<ApplicationUser> _signInManager;
        public readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity!.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Home", new { areas = "Admin" });
                }

                return RedirectToAction("Index", "Home");
            }

            AddCssFilesHelper.AddCssFiles(controller: this, "form.css");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterManagerDTO registerDTO)
        {

            if (!ModelState.IsValid)
            {
                AddCssFilesHelper.AddCssFiles(controller: this, "form.css");

                ViewBag.Erros = ModelState.Values
                    .SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                return View(registerDTO);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
                PersonName = registerDTO.PersonName,
                UserName = registerDTO.Email
            };


            if(registerDTO.Password == null)
            {
                return View(user);
            }

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                if (await _roleManager.FindByNameAsync(registerDTO.UserType.ToString()) is null)
                {
                    ApplicationRole applicationRole = new ApplicationRole()
                    {
                        Name = registerDTO.UserType.ToString()
                    };

                    await _roleManager.CreateAsync(applicationRole);
                }

                await _userManager.AddToRoleAsync(user, registerDTO.UserType.ToString());

                await _signInManager.SignInAsync(user, isPersistent: registerDTO.RememberMe);

                return RedirectToAction("GerarSorrisos", "Home");
            }
            else
            {
                ViewBag.Errors = new List<string>();

                foreach (IdentityError error in result.Errors)
                {
                    ViewBag.Errors.Add(error.Description);
                }

                return View(registerDTO);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "form.css");

            if (User.Identity!.IsAuthenticated)
            {
                if(User.IsInRole("Admin")) 
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                AddCssFilesHelper.AddCssFiles(controller: this, "form.css");

                ViewBag.Erros = ModelState.Values
                    .SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                return View(loginDTO);
            }

            if(loginDTO.Email == null || loginDTO.Password == null)
            {
                return View(loginDTO);
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(loginDTO!.Email, loginDTO!.Password, isPersistent: loginDTO.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                ApplicationUser? user = await _userManager.FindByNameAsync(loginDTO.Email);

                var isAuthenticatedAfterSignIn = User.Identity.IsAuthenticated;

                if (user != null)
                {
                    if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }

                    if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Manager.ToString()))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return LocalRedirect(ReturnUrl);
                    }

                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }

            AddCssFilesHelper.AddCssFiles(controller: this, "form.css");

            ViewBag.NotFoundError = "Email ou senha inválidos";

            ModelState.AddModelError("Login", "Email ou senha inválidos");

            return View(loginDTO);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> EmailValidator(string email)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(email);

            if (user == null)
            {
                return Json(true);
            }

            return Json(false);
        }

    }
}
