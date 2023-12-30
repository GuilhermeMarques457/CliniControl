using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.DTO.AccountDTO;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.Enums;
using OdontoControl.Core.ServiceContracts.ClinicContracts;
using OdontoControl.UI.Filters.ActionFilters;
using OdontoControl.UI.Usefull;
using System;
using System.Data;

namespace OdontoControl.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/[controller]/[action]")]
    [Authorize(Roles = "Admin")]
    public class ClinicController : Controller
    {
        private readonly IClinicAdderService _clinicAdderService;
        private readonly IClinicUpdaterService _clinicUpdaterService;
        private readonly IClinicDeleterService _clinicDeleterService;
        private readonly IClinicGetterService _clinicGetterService;
        private readonly IClinicSorterService _clinicSorterService;

        public ClinicController(IClinicAdderService clinicAdderService,
            IClinicUpdaterService clinicUpdaterService,
            IClinicDeleterService clinicDeleterService,
            IClinicGetterService clinicGetterService,
            IClinicSorterService clinicSorterService)
        {
            _clinicAdderService = clinicAdderService;
            _clinicDeleterService = clinicDeleterService;
            _clinicGetterService = clinicGetterService;
            _clinicUpdaterService = clinicUpdaterService;
            _clinicSorterService = clinicSorterService;
        }

        [HttpGet]
        [TypeFilter(typeof(ClinicsListActionFilter))]
        [TypeFilter(typeof(CheckingTableSortingActionFilter))]
        public async Task<IActionResult> ListClinics(
            string? searchString,
            string? searchBy,
            string? sortBy = nameof(ClinicResponse.ClinicName),
            SortOrderOptions sortOrderOptions = SortOrderOptions.ASC
        )
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "admin.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "modal.js", "pagination.js");

            ViewBag.Success = TempData["Success"];
            ViewBag.Errors = TempData["Errors"];

            List<ClinicResponse>? clinicList = await _clinicGetterService.GetAllClinics();

            if (searchString != null)
            {
                clinicList = await _clinicGetterService.GetFilterdClinics(searchBy, searchString);
            }

            clinicList = _clinicSorterService.GetSortedClinics(clinicList, sortBy, sortOrderOptions);

            ViewBag.TotalRegisters = clinicList?.Count();

            return View(clinicList);


        }


        [HttpGet]
        public IActionResult NewClinic()
        {
            ViewBag.CssFiles = new List<string>();
            ViewBag.CssFiles.Add("form.css");

            List<string> cities = new List<string>(Enum.GetNames(typeof(CitiesOptions)));

            ViewBag.Cities = cities.Select(
               temp => new SelectListItem()
               {
                   Text = temp,
                   Value = temp
               });;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewClinic(ClinicAddRequest clinic)
        {

            ViewBag.CssFiles = new List<string>();
            ViewBag.CssFiles.Add("form.css");

            if (!ModelState.IsValid)
            {
                return View(clinic);
            }

            ClinicResponse clinicResponse = await _clinicAdderService.AddClinic(clinic);
            
            if(clinicResponse == null)
            {
                return View(clinic);
            }

            TempData["Success"] = "Clinica Adicionada com sucesso";

            return RedirectToAction("ListClinics");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClinic(Guid? ID)
        {
            if (ID == null)
            {
                TempData["Errors"] = "Cliníca não encontrada, tente novamente mais tarde";

                return RedirectToAction("ListClinics");
            }

            bool isDeleted = await _clinicDeleterService.DeleteClinic(ID);

            if(isDeleted)
            {
                TempData["Success"] = "Cliníca deletada com sucesso";

                return RedirectToAction("ListClinics");

            }
            else
            {
                TempData["Errors"] = "Ocorreu um erro ao deletar essa cliníca, tente novamente mais tarde";

                return RedirectToAction("ListClinics");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateClinic(Guid? ID)
        {
            ViewBag.CssFiles = new List<string>();
            ViewBag.CssFiles.Add("form.css");

            List<string> cities = new List<string>(Enum.GetNames(typeof(CitiesOptions)));

            ViewBag.Cities = cities.Select(
               temp => new SelectListItem()
               {
                   Text = temp,
                   Value = temp
               }); ;

            ClinicResponse? clinicResponse = await _clinicGetterService.GetClinicById(ID);

            if(clinicResponse == null)
            {
                ViewBag.Errors = "Algo deu errado ao encontrar a cliníca. Tente novamente mais tarde";

                return RedirectToAction("ListClinics");
            }

            return View(clinicResponse.ToClinicUpdateRequest());
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClinic(ClinicUpdateRequest clinic)
        {

            ViewBag.CssFiles = new List<string>();

            if (!ModelState.IsValid)
            {
                return View(clinic);
            }

            ClinicResponse? clinicResponse = await _clinicUpdaterService.UpdateClinic(clinic);

            if (clinicResponse == null)
            {
                TempData["Errors"] = "Algo deu errado ao atualizar a cliníca. Tente novamente mais tarde";
            }

            TempData["Success"] = "Cliníca alterada com sucesso";

            return RedirectToAction("ListClinics");
        }
    }
}
