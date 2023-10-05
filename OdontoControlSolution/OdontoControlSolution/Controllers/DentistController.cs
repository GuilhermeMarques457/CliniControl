﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdontoControl.Core.Domain.IdentityEntities;
using OdontoControl.Core.DTO.DentistDTO;
using OdontoControl.Core.Enums;
using OdontoControl.Core.ServiceContracts.ClinicContracts;
using OdontoControl.Core.ServiceContracts.DentistContracts;
using OdontoControl.UI.Filters.ActionFilters;
using OdontoControl.UI.Usefull;
using System.Data;
using System.Security.Principal;

namespace OdontoControl.Controllers
{
    [Route("[controller]/[action]")]
    public class DentistController : Controller
    {
        private readonly IDentistAdderService _DentistAdderService;
        private readonly IDentistUpdaterService _DentistUpdaterService;
        private readonly IDentistDeleterService _DentistDeleterService;
        private readonly IDentistGetterService _DentistGetterService;
        private readonly IClinicGetterService _ClinicGetterService;
        private readonly IDentistSorterService _DentistSorterService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DentistController(IDentistAdderService DentistAdderService,
            IDentistUpdaterService DentistUpdaterService,
            IDentistDeleterService DentistDeleterService,
            IDentistGetterService DentistGetterService,
            IClinicGetterService clinicGetterService,
            IDentistSorterService dentistSorterService,
            UserManager<ApplicationUser> userManager
        )
        {
            _DentistAdderService = DentistAdderService;
            _DentistDeleterService = DentistDeleterService;
            _DentistGetterService = DentistGetterService;
            _DentistUpdaterService = DentistUpdaterService;
            _ClinicGetterService = clinicGetterService;
            _DentistSorterService = dentistSorterService;
            _userManager = userManager;
        }

      
        [TypeFilter(typeof(DentistsListActionFilter))]
        [TypeFilter(typeof(CheckingTableSortingActionFilter))]
        public async Task<IActionResult> ListDentists(
                    string? searchString,
                    string? searchBy,
                    string? sortBy = nameof(DentistResponse.DentistName),
                    SortOrderOptions sortOrderOptions = SortOrderOptions.ASC
        )
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "modal.js", "pagination.js");

            ViewBag.Success = TempData["Success"];
            ViewBag.Errors = TempData["Errors"];

            Guid? userClinicId = new Guid();
            ApplicationUser? currentUser = new ApplicationUser();

            if (User.Identity != null)
            {
                var userName = User.Identity.Name;

                if (userName != null)
                {
                    currentUser = await _userManager.FindByNameAsync(userName);
                    userClinicId = currentUser?.ClinicID;
                }

            }

            List<DentistResponse>? DentistList = await _DentistGetterService.GetDentistsByClinicID(userClinicId);

            if (searchString != null)
            {
                DentistList = await _DentistGetterService.GetFilterdDentists(searchBy, searchString);
            }

            DentistList = _DentistSorterService.GetSortedDentists(DentistList, sortBy, sortOrderOptions);

            if(DentistList == null)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (DentistResponse dentist in DentistList)
            {
                dentist.Manager = await _userManager.FindByIdAsync(dentist.ManagerID.ToString());
            }

            ViewBag.TotalRegisters = DentistList.Count();

            return View(DentistList);
        }

        [HttpGet]
        public async Task<IActionResult> NewDentist()
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "form.css");

            if (User.Identity == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ApplicationUser? user = await GetCurrentUser(User.Identity);

            ViewBag.ManagerID = user?.Id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewDentist(DentistAddRequest Dentist)
        {

            if (!ModelState.IsValid)
            {
                AddCssFilesHelper.AddCssFiles(controller: this, "form.css");

                return View(Dentist);
            }

            DentistResponse DentistResponse = await _DentistAdderService.AddDentist(Dentist);

            if (DentistResponse == null)
            {
                return View(Dentist);
            }

            TempData["Success"] = "Dentista Cadastrado com successo";

            return RedirectToAction("ListDentists");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDentist(Guid? ID)
        {
            if (ID == null)
            {
                TempData["Errors"] = "Dentista não encontrada, tente novamente mais tarde";

                return RedirectToAction("ListDentists");
            }

            await _DentistDeleterService.DeleteDentistAppointments(ID);
            bool isDeleted = await _DentistDeleterService.DeleteDentist(ID);

            if (isDeleted)
            {
                TempData["Success"] = "Dentista deletada com sucesso";

                return RedirectToAction("ListDentists");

            }
            else
            {
                TempData["Errors"] = "Ocorreu um erro ao deletar esse dentista, tente novamente mais tarde";

                return RedirectToAction("ListDentists");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateDentist(Guid? ID)
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "form.css");

            DentistResponse? DentistResponse = await _DentistGetterService.GetDentistById(ID);

            if (DentistResponse == null)
            {
                ViewBag.Errors = "Algo deu errado ao encontrar o dentista. Tente novamente mais tarde";

                return RedirectToAction("ListDentists");
            }

            return View(DentistResponse.ToDentistUpdateRequest());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDentist(DentistUpdateRequest Dentist)
        {

            if (!ModelState.IsValid)
            {
                AddCssFilesHelper.AddCssFiles(controller: this, "form.css");

                return View(Dentist);
            }

            DentistResponse? DentistResponse = await _DentistUpdaterService.UpdateDentist(Dentist);

            if (DentistResponse == null)
            {
                TempData["Errors"] = "Algo deu errado ao atualizar o dentista. Tente novamente mais tarde";
            }

            TempData["Success"] = "Dentista alterada com sucesso";

            return RedirectToAction("ListDentists");
        }

        private async Task<ApplicationUser?> GetCurrentUser(IIdentity? user)
        {
            if (user != null)
            {
                var userName = User.Identity?.Name;

                if (userName != null)
                {
                    var currentUser = await _userManager.FindByNameAsync(userName);
                    return currentUser;
                }

                return null;
            }

            return null;
        }

    }
}