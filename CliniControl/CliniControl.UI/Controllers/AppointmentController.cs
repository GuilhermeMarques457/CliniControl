using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CliniControl.Core.Domain.IdentityEntities;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.Enums;
using CliniControl.Core.ServiceContracts.ClinicContracts;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.UI.Filters.ActionFilters;
using System.Data;
using CliniControl.Core.ServiceContracts.DentistContracts;
using CliniControl.Core.DTO.DentistDTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using CliniControl.Core.ServiceContracts.PatientContracts;
using CliniControl.Core.DTO.PatientDTO;
using CliniControl.Core.Domain.Entities;
using System.Security.Claims;
using System.Security.Principal;
using CliniControl.UI.Usefull;
using Newtonsoft.Json.Linq;
using System;
using CliniControl.Core.ServiceContracts.TwilioContracts;
using CliniControl.Core.DTO.ClinicDTO;
using System.Text.Json;
using Microsoft.AspNetCore.Routing;

namespace CliniControl.Controllers
{

    [Route("[controller]/[action]")]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentAdderService _AppointmentAdderService;
        private readonly IAppointmentUpdaterService _AppointmentUpdaterService;
        private readonly IAppointmentDeleterService _AppointmentDeleterService;
        private readonly IAppointmentGetterService _AppointmentGetterService;
        private readonly IAppointmentSorterService _AppointmentSorterService;
        private readonly IDentistGetterService _dentistGetterService;
        private readonly IPatientGetterService _patientGetterService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITwilioService _twilioService;
        private readonly IClinicGetterService _clinicGetterService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AppointmentController(IAppointmentAdderService AppointmentAdderService,
            IAppointmentUpdaterService AppointmentUpdaterService,
            IAppointmentDeleterService AppointmentDeleterService,
            IAppointmentGetterService AppointmentGetterService,
            IAppointmentSorterService AppointmentSorterService,
            IDentistGetterService dentististGetterService,
            IPatientGetterService patientGetterService,
            ITwilioService twilioService,
            IClinicGetterService clinicGetterService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHostEnvironment
        )
        {
            _AppointmentAdderService = AppointmentAdderService;
            _AppointmentDeleterService = AppointmentDeleterService;
            _AppointmentGetterService = AppointmentGetterService;
            _AppointmentUpdaterService = AppointmentUpdaterService;
            _AppointmentSorterService = AppointmentSorterService;
            _dentistGetterService = dentististGetterService;
            _patientGetterService = patientGetterService;
            _userManager = userManager;
            _twilioService = twilioService;
            _clinicGetterService = clinicGetterService;
            _webHostEnvironment = webHostEnvironment;
        }


        [TypeFilter(typeof(AppointmentsListActionFilter))]
        [TypeFilter(typeof(CheckingTableSortingActionFilter))]
        public async Task<IActionResult> ListAppointments(
                    string? searchString,
                    string? searchBy,
                    string? sortBy = nameof(AppointmentResponse.Patient.PatientName),
                    SortOrderOptions sortOrderOptions = SortOrderOptions.ASC
        )
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "modal.js", "pagination.js", "changeAppointmentStatus.js");

            ViewBag.Success = TempData["Success"];
            ViewBag.Errors = TempData["Errors"];

            List<AppointmentResponse>? AppointmentList = await _AppointmentGetterService.GetAllAppointments();

            List<AppointmentResponse>? PossibleChangesAppointments = await _AppointmentGetterService.GetAppointmentsByPossibleStatusChange(null);

            if(PossibleChangesAppointments != null)
            {
                List<AppointmentResponse>? appointmentResponseUpdatedStatusList = await _AppointmentUpdaterService.UpdateAppointmentStatus(PossibleChangesAppointments.Select(temp => temp.ToAppointmentUpdateRequest()).ToList());
            }
               
            if (searchString != null)
            {

                if (searchBy == nameof(AppointmentResponse.AppointmentTime))
                {
                    searchString = DateTime.Parse(searchString).ToString("yyyy-MM-dd");
                }

                AppointmentList = await _AppointmentGetterService.GetFilterdAppointments(searchBy, searchString);

                ViewBag.CurrentSearchString = searchString;
            }

            AppointmentList = _AppointmentSorterService.GetSortedAppointment(AppointmentList, sortBy, sortOrderOptions);

            if (AppointmentList == null)
            {
                TempData["Errors"] = "Não ha consultas agendadas, cadastre uma e tente novamente mais tarde";

                return RedirectToAction("Index", "Home");
            }

            ViewBag.TotalRegisters = AppointmentList.Count();

            return View(AppointmentList);
        }

        [HttpGet("{PatientOrDentistID}")]
        public async Task<IActionResult> NewAppointment(Guid? PatientOrDentistID)
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "availableHours.js");

            ViewBag.PatientOrDentistID = PatientOrDentistID;

            string previousUrl = Request.Headers["Referer"].ToString();
            TempData["previousUrl"] = previousUrl;

            await AddDefaultConfigPostPutActionMethod(PatientOrDentistID);

            return View();
        }

        
        [HttpPost]
        [HttpPost("{PatientOrDentistID}")]
        public async Task<IActionResult> NewAppointment(AppointmentAddRequest Appointment, Guid? PatientOrDentistID)
        {
            if (!ModelState.IsValid)
            {
                AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");
                AddJsFilesHelper.AddJsFiles(controller: this, "availableHours.js");

                await AddDefaultConfigPostPutActionMethod(PatientOrDentistID);

                return View(Appointment);
            }

            string previousUrl = TempData["previousUrl"]!.ToString()!;

            AppointmentResponse AppointmentResponse = await _AppointmentAdderService.AddAppointment(Appointment);

            if (AppointmentResponse == null)
            {
                return View(Appointment);
            }

            TempData["Success"] = "Consulta adicionada com sucesso";

            return Redirect(previousUrl);
        }

        [HttpGet("{ID}")]
        public async Task<IActionResult> UpdateAppointment(Guid? ID)
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "availableHours.js");

            ViewBag.Success = TempData["Success"];
            ViewBag.Errors = TempData["Errors"];

            FillViewProcedure();

            AppointmentResponse? appointmentResponse = await _AppointmentGetterService.GetAppointmentById(ID);

            if (appointmentResponse == null)
            {
                ViewBag.Errors = "Algo deu errado ao encontrar a consulta. Tente novamente mais tarde";

                return RedirectToAction("ListAppointments");
            }

            return View(appointmentResponse);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAppointment(AppointmentUpdateRequest Appointment)
        {
            if (!ModelState.IsValid)
            {
                FillViewProcedure();

                AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");
                AddJsFilesHelper.AddJsFiles(controller: this);

                AppointmentResponse? appointmentResponse = await _AppointmentGetterService.GetAppointmentById(Appointment.ID);

                TempData["Errors"] = "Preencha os dados corretamente";

                return View(Appointment);
            }

            Appointment.Status = AppointmentStatusOptions.Agendado;
            
            AppointmentResponse? AppointmentResponse = await _AppointmentUpdaterService.UpdateAppointment(Appointment);
           
            if (AppointmentResponse == null)
            {
                TempData["Errors"] = "Algo deu errado ao atualizar a consulta. Tente novamente mais tarde";
            }

            TempData["Success"] = "Consulta alterada com sucesso";

            return RedirectToAction("ListAppointments");
        }

        [HttpGet("{ID}")]
        public async Task<IActionResult> UpdateFinishedAppointment(Guid? ID)
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "modal.js", "addExam.js", "simpleModal.js");

            ViewBag.Success = TempData["Success"];
            ViewBag.Errors = TempData["Errors"];

            AppointmentResponse? appointmentResponse = await _AppointmentGetterService.GetAppointmentById(ID);

            if (appointmentResponse == null)
            {
                ViewBag.Errors = "Algo deu errado ao encontrar a consulta. Tente novamente mais tarde";

                return RedirectToAction("ListAppointments");
            }

            return View(appointmentResponse);
        }

        [HttpPost("{ID}")]
        public async Task<IActionResult> UpdateFinishedAppointment(string? Comments, Guid? ID)
        {
            AppointmentResponse? appointmentToAddComments = await _AppointmentGetterService.GetAppointmentById(ID);

            if (appointmentToAddComments == null)
            {
                TempData["Errors"] = "Algo deu errado ao adicionar comentário à consulta. Tente novamente mais tarde";

                return RedirectToAction("ListAppointment", "Appointment");
            }

            if (Comments == null)
            {
                ViewBag.Errors = "Adicione um comentário antes de envio. Tente novamente";

                AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");
                AddJsFilesHelper.AddJsFiles(controller: this, "modal.js");

                return View(appointmentToAddComments);
            }

            appointmentToAddComments.Comments = Comments;

            AppointmentResponse? AppointmentResponse = await _AppointmentUpdaterService.UpdateAppointment(appointmentToAddComments.ToAppointmentUpdateRequest());

            if (AppointmentResponse == null)
            {
                TempData["Errors"] = "Algo deu errado ao atualizar a consulta. Tente novamente mais tarde";
            }

            TempData["Success"] = "Comentário adicionado com sucesso";

            return RedirectToAction("UpdateFinishedAppointment", new { ID = AppointmentResponse?.ID });
        }

        [HttpPost("{appointmentNewStatus}")]
        public async Task<IActionResult> ChangeAppoitmentStatus(Guid? ID, string? appointmentNewStatus)
        {
            AppointmentResponse? appointmentToPay = await _AppointmentGetterService.GetAppointmentById(ID);

            string referer = Request.Headers["Referer"].ToString();

            if (appointmentToPay == null)
            {
                TempData["Errors"] = "Algo deu errado ao pagar a consulta à consulta. Tente novamente mais tarde";

                return RedirectToAction("ListAppointment", "Appointment");
            }

            appointmentToPay.Status = appointmentNewStatus;

            AppointmentResponse? AppointmentResponse = await _AppointmentUpdaterService.UpdateAppointment(appointmentToPay.ToAppointmentUpdateRequest());

            if (AppointmentResponse == null)
            {
                TempData["Errors"] = "Algo deu errado ao atualizar a consulta. Tente novamente mais tarde";
            }

            TempData["Success"] = "Consulta paga com successo com sucesso";

            return Redirect(referer);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(Guid? ID)
        {
            if (ID == null)
            {
                TempData["Errors"] = "Consulta não encontrada, tente novamente mais tarde";

                return RedirectToAction("ListAppointments");
            }

            bool isDeleted = await _AppointmentDeleterService.DeleteAppointment(ID);

            if (isDeleted)
            {
                TempData["Success"] = "Consulta deletada com sucesso";

                return RedirectToAction("ListAppointments");

            }
            else
            {
                TempData["Errors"] = "Ocorreu um erro ao deletar essa consulta, tente novamente mais tarde";

                return RedirectToAction("ListAppointments");
            }
        }

        [HttpGet]
        [HttpGet("{appointmentDay}/{dentistID}")]
        public async Task<IActionResult> GetAvailableHours(DateTime? appointmentDay, Guid? dentistID)
        {
            if (!appointmentDay.HasValue)
            {
                return BadRequest("Data da consulta não fornecida.");
            }

            DentistResponse? dentist = await _dentistGetterService.GetDentistById(dentistID);

            if (dentist == null)
            {
                return BadRequest("Dentisa não fornecido.");
            }

            List<AppointmentResponse>? appointmentsOfTheDay = await _AppointmentGetterService.GetAllDayAppointments(appointmentDay, dentist.ID);

            if (appointmentsOfTheDay == null)
            {
                return BadRequest("Erro ao encontrar Consultas.");
            }

            List<TimeSpan?> availableTimes = new List<TimeSpan?>();
            List<TimeSpan?> notAvailableHours = new List<TimeSpan?>();

            TimeSpan fifiteenMinutes = TimeSpan.FromMinutes(15);

       
            foreach(AppointmentResponse appointment in appointmentsOfTheDay)
            {
                TimeSpan? timeOfDay = appointment.StartTime;

                AppointmentResponse? appointmentWithStartTimeAndEndTimeEquals = appointmentsOfTheDay.FirstOrDefault(temp => temp.EndTime == timeOfDay);

                if (appointmentWithStartTimeAndEndTimeEquals != null)
                {
                    notAvailableHours.Add(timeOfDay);
                }

                if (timeOfDay!.Value == dentist.StartTime)
                {
                    notAvailableHours.Add(timeOfDay.Value);
                }

                timeOfDay = timeOfDay!.Value.Add(fifiteenMinutes);


                bool enterOnce = false;

                while (timeOfDay < appointment.EndTime)
                {
                    enterOnce = true;
                    notAvailableHours.Add(timeOfDay.Value);



                    timeOfDay = timeOfDay.Value.Add(fifiteenMinutes);
                }

                if(!enterOnce)
                {
                    notAvailableHours.Add(appointment.StartTime);
                }
            }

            TimeSpan? dentistTime = dentist.StartTime;

            while (dentistTime <= dentist.EndTime)
            {
                if (!notAvailableHours.Any(temp => temp == dentistTime!.Value))
                {
                    availableTimes.Add(dentistTime);
                }

                dentistTime = dentistTime.Value.Add(fifiteenMinutes);
            }

            return Json(new { AvailableTimes = availableTimes });

        }

        [HttpPost("{appointmentID}")]
        public async Task<IActionResult> AddExamToAppointment(Guid? appointmentID)
        { 
            if(!appointmentID.HasValue)
            {
                TempData["Errors"] = "Consulta não fornecida, tente novamente mais tarde";

                return RedirectToAction("Daily", "Home");
            }

            string previousUrl = Request.Headers["Referer"].ToString();
            string webRootPath = _webHostEnvironment.WebRootPath;

            try
            {
                IFormFile? file = Request.Form.Files["fileName"];

                if (file != null && file.Length > 0)
                {
                    AppointmentResponse? appointmentResponse = await _AppointmentUpdaterService.AddExamToAppointment(appointmentID.Value, file, webRootPath);

                    if (appointmentResponse == null)
                    {
                        TempData["Errors"] = "Algum erro ocorreu, tente novamente mais tarde";

                        return Redirect(previousUrl);
                    }

                    TempData["AppointmentID"] = appointmentID; 
                    TempData["Success"] = $"Um novo anexo foi registrado na consulta do paciente {appointmentResponse?.Patient?.PatientName} com sucesso!";

                    return Redirect(previousUrl);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            TempData["Errors"] = "Algum erro ocorreu, tente novamente mais tarde";

            return RedirectToAction("Daily", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAttachment(string? urlPathImg, Guid? AppointmentID)
        {
            if (!AppointmentID.HasValue)
            {
                TempData["Errors"] = "Consulta não fornecida, tente novamente mais tarde";

                return RedirectToAction("Daily", "Home");
            }

            if (urlPathImg == null)
            {
                TempData["Errors"] = "Imagem não fornecida, tente novamente mais tarde";

                return RedirectToAction("UpdateFinishedAppointment", new { ID = AppointmentID });
            }

            string webRootPath = _webHostEnvironment.WebRootPath;

            bool isDeleted = await _AppointmentDeleterService.DeleteAttachment(urlPathImg, AppointmentID.Value, webRootPath);

            if (isDeleted)
            {
                TempData["Success"] = "Anexo deletado com sucesso";

                return RedirectToAction("UpdateFinishedAppointment", new { ID = AppointmentID });
            }

            TempData["Errors"] = "Algum erro ocorreu ao deletar anexo, tente novamente mais tarde";

            return RedirectToAction("UpdateFinishedAppointment", new { ID = AppointmentID });
        }


        private async Task AddDefaultConfigPostPutActionMethod(Guid? PatientOrDentistID)
        {
            Guid? patientOrDentistID = PatientOrDentistID == null ? Guid.Empty : PatientOrDentistID;
            ApplicationUser? currentUser = await GetCurrentUser(User.Identity);

            PatientResponse? patient = await _patientGetterService.GetPatientById(patientOrDentistID.Value);
            DentistResponse? dentist = await _dentistGetterService.GetDentistById(patientOrDentistID.Value);

            // Verification to add combo-box or names in the forms
            await AddingPatientsAndDentistBasedIfSelected(patient, dentist, currentUser, patientOrDentistID);

            // Adding the available procedures and available hours
            FillViewProcedure();
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

        private void FillViewProcedure()
        {
            List<string> procedures = new List<string>(Enum.GetNames(typeof(ProcedureTypeOptions)));
            ViewBag.Procedures = procedures.Select(
               temp => new SelectListItem()
               {
                   Text = temp,
                   Value = temp
               });
        }

       

        private async Task AddingPatientsAndDentistBasedIfSelected(PatientResponse? patient, DentistResponse? dentist, ApplicationUser? user, Guid? PatientOrDentistID)
        {
            if (dentist == null)
            {
                List<DentistResponse>? dentists = await _dentistGetterService.GetDentistsByClinicID(user?.ClinicID);

                if (dentists == null)
                {
                    ViewBag.Dentists = new SelectListItem()
                    {
                        Text = "Não ha dentistas cadastrados",
                        Value = null,
                    };
                }
                else
                {
                    ViewBag.Dentists = dentists.Select(
                      temp => new SelectListItem()
                      {
                          Text = temp.DentistName,
                          Value = temp.ID.ToString(),
                      });
                }

                if (patient != null)
                {
                    ViewBag.PatientName = patient.PatientName;
                    ViewBag.PatientID = PatientOrDentistID;
                }

            }

            if (patient == null)
            {
                List<PatientResponse>? patients = await _patientGetterService.GetPatientsByManagerID(user?.Id);

                if (patients == null)
                {
                    ViewBag.Patients = new SelectListItem()
                    {
                        Text = "Não ha pacientes cadastrados",
                        Value = null,
                    };
                }
                else
                {
                    ViewBag.Patients = patients.Select(
                      temp => new SelectListItem()
                      {
                          Text = temp.PatientName,
                          Value = temp.ID.ToString(),
                      });
                }

                if (dentist != null)
                {
                    ViewBag.DentistName = dentist.DentistName;
                    ViewBag.DentistID = PatientOrDentistID;
                }
            }
        }

        //[HttpGet("{ID}")]
        //public async Task<IActionResult> SendWhatssapMessage(Guid? ID)
        //{
        //    ApplicationUser? currentUser = await GetCurrentUser(User.Identity);
        //    ClinicResponse? clinic = await _clinicGetterService.GetClinicById(currentUser?.ClinicID);

        //    if (!ID.HasValue)
        //        return BadRequest("ID não fornecido.");

        //    AppointmentResponse? appointment = await _AppointmentGetterService.GetAppointmentById(ID);

        //    if (appointment == null)
        //        return BadRequest("Erro ao encontrar Consulta.");

        //    bool messageSent = _twilioService.SendWhatssapMessage(appointment?.Patient?.PhoneNumber, $"Olá, Bom dia! Passando aqui para lembrar você {appointment?.Patient?.PatientName}, que tens consulta no dia {appointment?.AppointmentTime!.Value.ToShortDateString()}, da hora {appointment?.StartTime} até {appointment?.EndTime} com o dentista {appointment?.Dentist?.DentistName}. Para mais informações ou reagendamento favor contatar {clinic?.Phone}");

        //    if (messageSent)
        //    {
        //        appointment!.Reminded = true;
        //        await _AppointmentUpdaterService.UpdateAppointment(appointment.ToAppointmentUpdateRequest());

        //        return Json(new { success = true, message = "Mensagem enviada com sucesso." });
        //    }  
        //    else
        //        return Json(new { success = false, message = "Falha ao enviar mensagem." });


        //}

    }
}
