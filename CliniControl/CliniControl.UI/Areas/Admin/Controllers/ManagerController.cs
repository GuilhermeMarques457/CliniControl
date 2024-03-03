using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.IdentityEntities;
using CliniControl.Core.DTO.AccountDTO.ManagerDTO;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.Enums;
using CliniControl.Core.ServiceContracts.ClinicContracts;
using CliniControl.Core.Services.ClinicService;
using CliniControl.UI.Usefull;
using System;
using System.Data;


namespace CliniControl.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class ManagerController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClinicGetterService _clinicGetterService;

        public ManagerController(RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IClinicGetterService clinicGetterService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _clinicGetterService = clinicGetterService;
        }

        public async Task<IActionResult> ListManagers(string? searchString)
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "modal.js", "pagination.js");

            ViewBag.Success = TempData["Success"];
            ViewBag.Errors = TempData["Errors"];
            ViewBag.CurrentSearchString = searchString;

            ApplicationRole? role = await _roleManager.FindByNameAsync("Manager");

            if(role == null || role.Name == null  )
            {
                throw new Exception("Não existe essa role na aplicação");
            }

            IList<ApplicationUser> managerUsers = await _userManager.GetUsersInRoleAsync(role.Name);

            IList<ManagerDTO> managerDTOs = managerUsers.Select(temp => temp.ToManagerDTO()).ToList();


            if(searchString != null)
            {
                managerDTOs = managerDTOs.Where(temp => temp.PersonName!.Contains(searchString)).ToList();
            }

            foreach (ManagerDTO managerDTO in managerDTOs) 
            {
                ClinicResponse? clinicResponse = await _clinicGetterService.GetClinicById(managerDTO.ClinicID);

                if (clinicResponse == null) continue;

                managerDTO.Clinic = clinicResponse.ToClinic();
            }

            ViewBag.TotalRegisters = managerDTOs.Count();

            return View(managerDTOs);
        }


        public async Task<IActionResult> NewManager()
        {
            ViewBag.CssFiles = new List<string>();
            ViewBag.CssFiles.Add("form.css");

            List<ClinicResponse> clinics = await _clinicGetterService.GetAllClinics();

            ViewBag.Clinics = clinics.Select(
                temp => new SelectListItem()
                {
                    Text = temp.ClinicName,
                    Value = temp.ID.ToString(),
                });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewManager(RegisterManagerDTO registerDTO)
        {
            ViewBag.CssFiles = new List<string>();
            ViewBag.CssFiles.Add("form.css");

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                    .SelectMany(temp => temp.Errors)
                    .Select(temp => temp.ErrorMessage);

                return View(registerDTO);
            }

            registerDTO.UserType = UserTypeOptions.Manager;

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.Phone,
                PersonName = registerDTO.PersonName,
                UserName = registerDTO.Email,
                ClinicID = registerDTO.ClinicID
            };


            if (registerDTO.Password == null)
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

                return RedirectToAction("ListManagers", "Manager");
            }
            else
            {
                ViewBag.Errors = new List<string>();

                List<ClinicResponse> clinics = await _clinicGetterService.GetAllClinics();

                ViewBag.Clinics = clinics.Select(
                temp => new SelectListItem()
                {
                    Text = temp.ClinicName,
                    Value = temp.ID.ToString(),
                });

                foreach (IdentityError error in result.Errors)
                {
                    ViewBag.Errors.Add(error.Description);
                }

                return View(registerDTO);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteManager(Guid? ID)
        {
            if (ID == null)
            {
                TempData["Errors"] = "Gerenciador não encontrada, tente novamente mais tarde";

                return RedirectToAction("ListManagers", "Manager");
            }

            ApplicationUser? matchingUser = await _userManager.FindByIdAsync(ID.Value.ToString());

            if(matchingUser == null)
            {
                TempData["Errors"] = "Ocorreu um erro ao deletar esse gerenciador, tente novamente mais tarde";

                return RedirectToAction("ListManagers", "Manager");
            }

            await _userManager.DeleteAsync(matchingUser);

            TempData["Success"] = "Gerenciador deletada com sucesso";

            return RedirectToAction("ListManagers", "Manager");

        }

        [HttpGet]
        public async Task<IActionResult> UpdateManager(Guid? ID)
        {
            ViewBag.CssFiles = new List<string>();
            ViewBag.CssFiles.Add("form.css");

            List<ClinicResponse> clinics = await _clinicGetterService.GetAllClinics();

            ViewBag.Clinics = clinics.Select(
                temp => new SelectListItem()
                {
                    Text = temp.ClinicName,
                    Value = temp.ID.ToString(),
                });

            ApplicationUser? applicationUser = await _userManager.FindByIdAsync(ID!.Value.ToString());

            if (applicationUser == null)
            {
                TempData["Errors"] = "Algo deu errado ao encontrar o manager. Tente novamente mais tarde";

                return RedirectToAction("ListManagers", "Manager");
            }

            UpdateManagerDTO updateDTO = applicationUser.ToUpdateManagerDTO();

            return View(updateDTO);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateManager(UpdateManagerDTO updateDTO)
        {

            ViewBag.CssFiles = new List<string>();
            ViewBag.CssFiles.Add("form.css");

            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values
                   .SelectMany(temp => temp.Errors)
                   .Select(temp => temp.ErrorMessage);

                return View(updateDTO);
            }

            ApplicationUser? applicationUser = await _userManager.FindByIdAsync(updateDTO!.ID!.Value.ToString());

           

            if(applicationUser == null)
            {
                TempData["Errors"] = "Algo deu errado ao atualizar o Gerenciador. Tente novamente mais tarde";

                return RedirectToAction("ListManagers", "Manager");
            }

            applicationUser.ClinicID = updateDTO.ClinicID;
            applicationUser.Email = updateDTO.Email;
            applicationUser.PhoneNumber = updateDTO.Phone;
            applicationUser.PersonName = updateDTO.PersonName;

            await _userManager.UpdateAsync(applicationUser);

            TempData["Success"] = "Manager alterada com sucesso";

            return RedirectToAction("ListManagers", "Manager");
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
