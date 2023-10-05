using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.Domain.IdentityEntities;
using OdontoControl.Core.DTO.AppointmentDTO;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.DTO.ReminderDTO;
using OdontoControl.Core.DTO.RequestedPatientDTO;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.ClinicContracts;
using OdontoControl.Core.ServiceContracts.ReminderContracts;
using OdontoControl.Core.ServiceContracts.RequestedPatientContracts;
using OdontoControl.Core.Services.AppointmentService;
using OdontoControl.Core.Services.RequestedPatientService;
using OdontoControl.UI.Filters.ActionFilters;
using OdontoControl.UI.Usefull;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Principal;

namespace OdontoControl.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        public IAppointmentGetterService _appointmentGetterService;
        public IRequestedPatientAdderService _requestedPatientAdderService;
        public IRequestedPatientGetterService _requestedPatientGetterService;
        public IClinicGetterService _clinicalGetterService;
        public IReminderGetterService _reminderGetterService;
        public UserManager<ApplicationUser> _userManager;

        public HomeController(IAppointmentGetterService appointmentGetterService,
            IRequestedPatientAdderService requestedPatientAdderService,
            IRequestedPatientGetterService requestedPatientGetterService,
            IReminderGetterService reminderGetterService,
            IClinicGetterService clinicGetterService,
            UserManager<ApplicationUser> userManager)
        {
            _appointmentGetterService = appointmentGetterService;
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

                todayAppointments = await _appointmentGetterService.GetFilterdDayAppointments(searchBy, searchString, dateOfAppointments);
            }

            if (todayAppointments == null)
            {
                ViewBag.Errors = "Não há consulta agendadas hoje, agende algum paciente";

                return View(new List<AppointmentResponse>());
            }

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
