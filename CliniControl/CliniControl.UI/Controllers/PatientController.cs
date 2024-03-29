﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CliniControl.Core.Domain.Entities;
using CliniControl.Core.Domain.IdentityEntities;
using CliniControl.Core.DTO.AppointmentDTO;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.DTO.PatientDTO;
using CliniControl.Core.Enums;
using CliniControl.Core.Helpers;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.ClinicContracts;
using CliniControl.Core.ServiceContracts.PatientContracts;
using CliniControl.Core.ServiceContracts.RequestedPatientContracts;
using CliniControl.Core.Services.ClinicService;
using CliniControl.UI.Filters.ActionFilters;
using CliniControl.UI.Usefull;
using System.Data;
using System.Security.Principal;

namespace CliniControl.Controllers
{
    [Route("[controller]/[action]")]
    public class PatientController : Controller
    {
        private readonly IPatientAdderService _PatientAdderService;
        private readonly IPatientUpdaterService _PatientUpdaterService;
        private readonly IPatientDeleterService _PatientDeleterService;
        private readonly IPatientGetterService _PatientGetterService;
        private readonly IRequestedPatientGetterService _RequestedPatientGetterService;
        private readonly IRequestedPatientUpdaterService _RequestedPatientUpdaterService;
        private readonly IClinicGetterService _ClinicGetterService;
        private readonly IPatientSorterService _PatientSorterService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAppointmentGetterService _AppointmentGetterService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientController(IPatientAdderService PatientAdderService,
            IPatientUpdaterService PatientUpdaterService,
            IPatientDeleterService PatientDeleterService,
            IPatientGetterService PatientGetterService,
            IClinicGetterService clinicGetterService,
            IPatientSorterService PatientSorterService,
            IRequestedPatientGetterService requestedPatientGetterService,
            IRequestedPatientUpdaterService requestedPatientUpdaterService,
            IAppointmentGetterService appointmentGetterService,
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager
        )
        {
            _PatientAdderService = PatientAdderService;
            _PatientDeleterService = PatientDeleterService;
            _PatientGetterService = PatientGetterService;
            _PatientUpdaterService = PatientUpdaterService;
            _ClinicGetterService = clinicGetterService;
            _PatientSorterService = PatientSorterService;
            _RequestedPatientGetterService = requestedPatientGetterService;
            _RequestedPatientUpdaterService = requestedPatientUpdaterService;
            _AppointmentGetterService = appointmentGetterService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }


        [TypeFilter(typeof(PatientsListActionFilter))]
        [TypeFilter(typeof(CheckingTableSortingActionFilter))]
        public async Task<IActionResult> ListPatients(
                    string? searchString,
                    string? searchBy,
                    string? sortBy = nameof(PatientResponse.PatientName),
                    SortOrderOptions sortOrderOptions = SortOrderOptions.ASC
        )
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "modal.js", "pagination.js");

            ViewBag.Success = TempData["Success"];
            ViewBag.Errors = TempData["Errors"];

            List<PatientResponse>? PatientList = await _PatientGetterService.GetAllPatients();

            if (searchString != null)
            {
                PatientList = await _PatientGetterService.GetFilterdPatients(searchBy, searchString);
            }

            PatientList = _PatientSorterService.GetSortedPatients(PatientList, sortBy, sortOrderOptions);

            if (PatientList == null)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (PatientResponse Patient in PatientList)
            {
                Patient.Manager = await _userManager.FindByIdAsync(Patient.ManagerID.ToString());
            }

            ViewBag.TotalRegisters = PatientList.Count();

            return View(PatientList);
        }



        [HttpGet]
        public async Task<IActionResult> NewPatient()
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "form.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "showChangedPhoto.js");

            AddPatientGenders();

            ApplicationUser? user = await GetCurrentUser(User.Identity);

            ViewBag.ManagerID = user?.Id;

          
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewPatient(PatientAddRequest Patient, IFormFile? patientImage)
        {
            if (!ModelState.IsValid)
            {
                AddCssFilesHelper.AddCssFiles(controller: this, "form.css");
                AddJsFilesHelper.AddJsFiles(controller: this, "showChangedPhoto.js");

                AddPatientGenders();

                return View(Patient);
            }

            PatientResponse patientResponse = await _PatientAdderService.AddPatient(Patient);

            if (patientResponse == null)
            {
                TempData["Errors"] = "Ocorreu um erro ao adicionar o paciente";
                return RedirectToAction("ListPatients");
            }

            if (patientImage != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;

                await ManageImageProject.AddImage(patientImage, webRootPath, "imgs/patient-photos", patientResponse.ID);

                patientResponse.PhotoPath = $"/imgs/patient-photos/{patientResponse.ID}-{patientImage.FileName}";
            }
            else
            {
                patientResponse.PhotoPath = $"/imgs/default-user.jpg";
               
            }

            PatientResponse patientResponseImageAdded = await _PatientUpdaterService.UpdatePatient(patientResponse.ToPatientUpdateRequest());

            TempData["Success"] = "Paciente adicionado com sucesso";
            return RedirectToAction("ListPatients");
        }


        [HttpGet]
        public async Task<IActionResult> NotContactedPatients()
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "contactPatient.js");

            ViewBag.Success = TempData["Success"];
            ViewBag.Errors = TempData["Errors"];

            ApplicationUser? currentUser = await GetCurrentUser(User.Identity);

            var notContactedPatients = await _RequestedPatientGetterService.GetAllNotContactedPatients(currentUser?.ClinicID);

            return View(notContactedPatients);
        }

        [HttpPost]
        public async Task<IActionResult> ContactedPatient(Guid? PatientID)
        {
            var requestedPatient = await _RequestedPatientGetterService.GetRequestedPatientByID(PatientID);

            if(requestedPatient == null)
            {
                TempData["Errors"] = "Paciente não encontrado(a), tente novamente mais tarde";

                return RedirectToAction("NotContactedPatients");
            }

            var updatedPatient = await _RequestedPatientUpdaterService.UpdateContactedStatusPatient(requestedPatient.ToPatientUpdateRequest());

            if(updatedPatient == null)
            {
                TempData["Errors"] = "Ocorreu um erro ao alterar status de contato do paciente, tente novamente mais tarde";

                return RedirectToAction("NotContactedPatients");
            }

            TempData["Success"] = "Paciente foi contato com sucesso!";

            return RedirectToAction("NotContactedPatients");
        }


       

        [HttpPost]
        public async Task<IActionResult> DeletePatient(Guid? ID)
        {
            if (ID == null)
            {
                TempData["Errors"] = "Paciente não encontrado(a), tente novamente mais tarde";

                return RedirectToAction("ListPatients");
            }

            await _PatientDeleterService.DeletePatientAppointments(ID);
            bool isDeleted = await _PatientDeleterService.DeletePatient(ID);

            if (isDeleted)
            {
                TempData["Success"] = "Paciente deletado(a) com sucesso";

                return RedirectToAction("ListPatients");

            }
            else
            {
                TempData["Errors"] = "Ocorreu um erro ao deletar esse Paciente, tente novamente mais tarde";

                return RedirectToAction("ListPatients");
            }
        }

        [HttpGet("{ID}")]
        public async Task<IActionResult> PatientDetailsAndUpdate(Guid? ID)
        {
            PatientResponse? PatientResponse = await _PatientGetterService.GetPatientById(ID);

            if (PatientResponse == null)
            {
                ViewBag.Errors = "Algo deu errado ao encontrar o Paciente. Tente novamente mais tarde";

                return RedirectToAction("ListPatients");
            }

            await AddPatientUpdateDetailsNeedData(ID);

            return View(PatientResponse.ToPatientUpdateRequest());
        }

        [HttpPost]
        public async Task<IActionResult> PatientDetailsAndUpdate(PatientUpdateRequest Patient, IFormFile? patientImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PatientID = Patient.ID;

                await AddPatientUpdateDetailsNeedData(Patient.ID);

                return View(Patient);
            }

            if (patientImage != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;

                await ManageImageProject.AddImage(patientImage, webRootPath, "imgs/patient-photos", Patient!.ID);

                Patient.PhotoPath = $"/imgs/patient-photos/{Patient.ID}-{patientImage.FileName}";
            }

            PatientResponse? patientResponse = await _PatientUpdaterService.UpdatePatient(Patient);

            if (patientResponse == null)
            {
                TempData["Errors"] = "Algo deu errado ao atualizar o Paciente. Tente novamente mais tarde";
            }

            TempData["Success"] = "Paciente alterada com sucesso";

            return RedirectToAction("ListPatients");
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

        private void AddPatientGenders()
        {
            List<string> genders = new List<string>(Enum.GetNames(typeof(GenderOptions)));

            ViewBag.Genders = genders.Select(
               temp => new SelectListItem()
               {
                   Text = temp,
                   Value = temp
               });
        }

        private async Task AddPatientUpdateDetailsNeedData(Guid? patientID)
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "showChangedPhoto.js");

            AddPatientGenders();

            ViewBag.PatientID = patientID;

            List<AppointmentResponse>? patientAppointmentsResponse = await _AppointmentGetterService.GetAppointmentByPatientId(patientID);

            double? moneyExpeded = 0;

            if(patientAppointmentsResponse != null)
            {
                patientAppointmentsResponse.ForEach(temp => {
                    if (temp.Status != AppointmentStatusOptions.Agendado.ToString() && temp.Price != null)
                    {
                        moneyExpeded += temp.Price;
                    }
                });

                ViewBag.MoneyExpended = moneyExpeded;
            }
           
            ViewBag.PatientAppointments = patientAppointmentsResponse;

        }
    }
}
