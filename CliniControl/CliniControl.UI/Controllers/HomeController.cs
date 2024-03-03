using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.IdentityEntities;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.DTO.ReminderDTO;
using CliniControl.Core.DTO.RequestedPatientDTO;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.ClinicContracts;
using CliniControl.Core.ServiceContracts.ReminderContracts;
using CliniControl.Core.ServiceContracts.RequestedPatientContracts;
using CliniControl.Core.Services.AppointmentService;
using CliniControl.Core.Services.RequestedPatientService;
using CliniControl.UI.Filters.ActionFilters;
using CliniControl.UI.Usefull;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Principal;

namespace CliniControl.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly IAppointmentGetterService _appointmentGetterService;
        private readonly IAppointmentUpdaterService _appointmentUpdaterService;
        private readonly IRequestedPatientAdderService _requestedPatientAdderService;
        private readonly IRequestedPatientGetterService _requestedPatientGetterService;
        private readonly IClinicGetterService _clinicalGetterService;
        private readonly IReminderGetterService _reminderGetterService;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IAppointmentGetterService appointmentGetterService,
            IAppointmentUpdaterService appointmentUpdaterService,
            IRequestedPatientAdderService requestedPatientAdderService,
            IRequestedPatientGetterService requestedPatientGetterService,
            IReminderGetterService reminderGetterService,
            IClinicGetterService clinicGetterService,
            UserManager<ApplicationUser> userManager)
        {
            _appointmentGetterService = appointmentGetterService;
            _appointmentUpdaterService = appointmentUpdaterService;
            _requestedPatientAdderService = requestedPatientAdderService;
            _requestedPatientGetterService = requestedPatientGetterService;
            _clinicalGetterService = clinicGetterService;
            _userManager = userManager;
            _reminderGetterService = reminderGetterService;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css");

            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            ApplicationUser? currentUser = await GetCurrentUser(User.Identity);

            var notContactedPatients = await _requestedPatientGetterService.GetAllNotContactedPatients(currentUser?.ClinicID);
            ViewBag.NotContactedPatientsCount = notContactedPatients?.Count();

            var todayReminders = await _reminderGetterService.GetReminderByDateTime(DateTime.Today);
            ICollection<ReminderResponse> notFinishedReminders = new Collection<ReminderResponse>();

            if (todayReminders != null)
            {
                notFinishedReminders = todayReminders.Where(temp => temp.Finished == false).ToList();
            }

            ViewBag.NotFinishedReminders = notFinishedReminders.Count();

            return View();
        }

        [TypeFilter(typeof(AppointmentsListActionFilter))]
        public async Task<IActionResult> Daily(DateTime? dateOfAppointments = null, string? searchString = null, string? searchBy = null)
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "modal.js", "changeAppointmentStatus.js", "addExam.js");

            ViewBag.AppointmentID = TempData["AppointmentID"];
            ViewBag.Errors = TempData["Errors"];
            ViewBag.Success = TempData["Success"];

            dateOfAppointments = dateOfAppointments.HasValue ? dateOfAppointments : DateTime.Today;

            ViewBag.SelectedDate = dateOfAppointments;

            List<AppointmentResponse>? todayAppointments = await _appointmentGetterService.GetDayAppointments(dateOfAppointments);

            if (searchString != null)
            {
                ViewBag.CurrentSearchString = searchString;

                if(searchBy == nameof(AppointmentResponse.AppointmentTime))
                {
                    searchString = DateTime.Parse(searchString).ToString("yyyy-MM-dd");
                }

                todayAppointments = await _appointmentGetterService.GetFilterdDayAppointments(searchBy, searchString, dateOfAppointments);
            }

            if (todayAppointments == null)
            {
                ViewBag.Errors = "Não há consulta agendadas hoje, agende algum paciente";

                return View(new List<AppointmentResponse>());
            }

            List<AppointmentResponse>? appointmentResponseUpdatedStatusList = await _appointmentUpdaterService.UpdateAppointmentStatus(todayAppointments.Select(temp => temp.ToAppointmentUpdateRequest()).ToList());

            return View(todayAppointments);
        }

        [AllowAnonymous]
        [HttpGet("/GerarSorrisos")]
        public async Task<IActionResult> GerarSorrisos()
        {
            ViewBag.Clinic = await _clinicalGetterService.GetClinicByName("Gerar Sorrisos");

            return View(new RequestedPatientAddRequest());
        }

        [AllowAnonymous]
        [HttpPost("/GerarSorrisos")]
        public async Task<IActionResult> GerarSorrisos(RequestedPatientAddRequest patient)
        {
            if (!ModelState.IsValid)
                return View(patient);

            RequestedPatientResponse PatientResponse = await _requestedPatientAdderService.AddPatient(patient);

            if (PatientResponse == null)
                return View(patient);

            return RedirectToAction("DadosEnviados", "Home");
        }

        [AllowAnonymous]
        [HttpGet("/DadosEnviados")]
        public IActionResult DadosEnviados()
        {
            string referer = Request.Headers["Referer"].ToString();
            ViewBag.PreviousPage = referer;

            return View();
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
