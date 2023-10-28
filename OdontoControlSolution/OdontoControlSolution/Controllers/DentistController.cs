using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.IdentityEntities;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.DentistDTO;
using OdontoControl.Core.DTO.PatientDTO;
using OdontoControl.Core.Enums;
using OdontoControl.Core.Helpers;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
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
        private readonly IAppointmentGetterService _appointmentGetterService;
        private readonly IDentistSorterService _DentistSorterService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;

        public DentistController(IDentistAdderService DentistAdderService,
            IDentistUpdaterService DentistUpdaterService,
            IDentistDeleterService DentistDeleterService,
            IDentistGetterService DentistGetterService,
            IAppointmentGetterService appointmentGetterService,
            IClinicGetterService clinicGetterService,
            IDentistSorterService dentistSorterService,
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager
        )
        {
            _appointmentGetterService = appointmentGetterService;
            _DentistAdderService = DentistAdderService;
            _DentistDeleterService = DentistDeleterService;
            _DentistGetterService = DentistGetterService;
            _DentistUpdaterService = DentistUpdaterService;
            _ClinicGetterService = clinicGetterService;
            _DentistSorterService = dentistSorterService;
            _webHostEnvironment = webHostEnvironment;
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
            AddJsFilesHelper.AddJsFiles(controller: this, "showChangedPhoto.js");

            if (User.Identity == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ApplicationUser? user = await GetCurrentUser(User.Identity);

            ViewBag.ManagerID = user?.Id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewDentist(DentistAddRequest Dentist, IFormFile? dentistImage)
        {

            if (!ModelState.IsValid)
            {
                AddCssFilesHelper.AddCssFiles(controller: this, "form.css");
                AddJsFilesHelper.AddJsFiles(controller: this, "showChangedPhoto.js");

                return View(Dentist);
            }

            DentistResponse DentistResponse = await _DentistAdderService.AddDentist(Dentist);

            if (DentistResponse == null)
            {
                TempData["Errors"] = "Ocorreu um erro ao adicionar o paciente";
                return RedirectToAction("ListPatients");
            }

            if (dentistImage != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                await ManageImageProject.AddImage(dentistImage, webRootPath, "imgs/dentist-photos", DentistResponse.ID);

                DentistResponse.PhotoPath = $"/imgs/dentist-photos/{DentistResponse.ID}-{dentistImage.FileName}";
            }
            else
            {
                DentistResponse.PhotoPath = $"/imgs/default-user.png";
            }

            DentistResponse DentistResponseImageAdded = await _DentistUpdaterService.UpdateDentist(DentistResponse.ToDentistUpdateRequest());

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

        [HttpGet("{ID}")]
        public async Task<IActionResult> DentistDetailsAndUpdate(Guid? ID)
        {
            DentistResponse? DentistResponse = await _DentistGetterService.GetDentistById(ID);

            if (DentistResponse == null)
            {
                ViewBag.Errors = "Algo deu errado ao encontrar o Dentista. Tente novamente mais tarde";

                return RedirectToAction("ListDentists");
            }

            await AddDentistUpdateDetailsNeedData(ID);

            return View(DentistResponse.ToDentistUpdateRequest());
        }

        [HttpPost]
        public async Task<IActionResult> DentistDetailsAndUpdate(DentistUpdateRequest Dentist, IFormFile? DentistImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DentistID = Dentist.ID;

                await AddDentistUpdateDetailsNeedData(Dentist.ID);

                return View(Dentist);
            }

            if (DentistImage != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;

                await ManageImageProject.AddImage(DentistImage, webRootPath, "imgs/dentist-photos", Dentist!.ID);

                Dentist.PhotoPath = $"/imgs/dentist-photos/{Dentist.ID}-{DentistImage.FileName}";
            }

            DentistResponse? DentistResponse = await _DentistUpdaterService.UpdateDentist(Dentist);

            if (DentistResponse == null)
            {
                TempData["Errors"] = "Algo deu errado ao atualizar o Dentista. Tente novamente mais tarde";
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

        private async Task AddDentistUpdateDetailsNeedData(Guid? DentistID)
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "showChangedPhoto.js");

            ViewBag.DentistID = DentistID;

            List<AppointmentResponse>? DentistAppointmentsResponse = await _appointmentGetterService.GetAppointmentsByDentistId(DentistID);

            double? moneyExpeded = 0;

            if (DentistAppointmentsResponse != null)
            {
                DentistAppointmentsResponse.ForEach(temp => {
                    if (temp.Status != AppointmentStatusOptions.Agendado.ToString() && temp.Price != null)
                    {
                        moneyExpeded += temp.Price;
                    }
                });

                ViewBag.MoneyExpended = moneyExpeded;
            }

            ViewBag.DentistAppointments = DentistAppointmentsResponse;

        }

    }
}
