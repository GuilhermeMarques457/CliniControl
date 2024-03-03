using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CliniControl.Core.Domain.IdentityEntities;
using CliniControl.Core.DTO.ReminderDTO;
using CliniControl.Core.Enums;
using CliniControl.Core.ServiceContracts.ReminderContracts;
using CliniControl.Core.ServiceContracts.ClinicContracts;
using CliniControl.Core.Services.ReminderService;
using CliniControl.UI.Filters.ActionFilters;
using CliniControl.UI.Usefull;
using Microsoft.AspNetCore.Mvc.Rendering;
using CliniControl.Core.Domain.Entities;

namespace CliniControl.UI.Controllers
{
    [Route("[controller]/[action]")]
    public class ReminderController : Controller
    {
        public IReminderAdderService _ReminderAdderService;
        public IReminderGetterService _ReminderGetterService;
        public IReminderUpdaterService _ReminderUpdaterService;

        public ReminderController(IReminderAdderService ReminderAdderService,
            IReminderGetterService ReminderGetterService,
            IReminderUpdaterService ReminderUpdaterService
        )
        {
            _ReminderAdderService = ReminderAdderService;
            _ReminderGetterService = ReminderGetterService;
            _ReminderUpdaterService = ReminderUpdaterService;
        }

        public async Task<IActionResult> ListReminders(DateTime? dateOfReminders = null, string reminderStatus = "all")
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");
            AddJsFilesHelper.AddJsFiles(controller: this, "modal.js");

            ViewBag.ReminderID = TempData["ReminderID"];
            ViewBag.Errors = TempData["Errors"];
            ViewBag.Success = TempData["Success"];

            dateOfReminders = dateOfReminders.HasValue ? dateOfReminders : DateTime.Today;

            ViewBag.SelectedDate = dateOfReminders;

            List<ReminderResponse>? dayReminders = await _ReminderGetterService.GetReminderByDateTime(dateOfReminders);

            if (dayReminders == null)
            {
                ViewBag.Errors = "Não há lembrates registrados para esse dia, tudo em ordem, parabens!!";

                return View(new List<ReminderResponse>());
            }

            if (reminderStatus == "pending")
                dayReminders = dayReminders.Where(temp => temp.Finished == false).ToList();
            else if(reminderStatus == "finished")
                dayReminders = dayReminders.Where(temp => temp.Finished == true).ToList();

            ViewBag.ReminderStatus = reminderStatus;

            return View(dayReminders);
        }

        [HttpGet]
        public IActionResult NewReminder()
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");

            FillViewReminderType();

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> NewReminder(ReminderAddRequest Reminder)
        {
            if (!ModelState.IsValid)
            {
                AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");

                FillViewReminderType();

                return View(Reminder);
            }

            ReminderResponse ReminderResponse = await _ReminderAdderService.AddReminder(Reminder);

            if (ReminderResponse == null)
            {
                return View(Reminder);
            }

            TempData["Success"] = "Lembrete adicionada com sucesso";

            return RedirectToAction("ListReminders");
        }

        [HttpGet("{ID}")]
        public async Task<IActionResult> UpdateReminder(Guid? ID)
        {
            AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");

            ReminderResponse? ReminderResponse = await _ReminderGetterService.GetReminderByID(ID);

            FillViewReminderType();

            if (ReminderResponse == null)
            {
                ViewBag.Errors = "Algo deu errado ao encontrar o lembrete. Tente novamente mais tarde";

                return RedirectToAction("ListReminders");
            }

            return View(ReminderResponse.ToPatientUpdateRequest());
        }

        [HttpPost("{ID}")]
        public async Task<IActionResult> UpdateReminder(ReminderUpdateRequest Reminder)
        {
            if (!ModelState.IsValid)
            {
                AddCssFilesHelper.AddCssFiles(controller: this, "manager.css", "form.css");

                FillViewReminderType();

                return View(Reminder);
            }

            ReminderResponse? ReminderResponse = await _ReminderUpdaterService.UpdateReminder(Reminder);

            if (ReminderResponse == null)
            {
                TempData["Errors"] = "Algo deu errado ao atualizar o lembrete. Tente novamente mais tarde";
            }

            TempData["Success"] = "Lembrete alterado com sucesso";

            return RedirectToAction("ListReminders");
        }

        private void FillViewReminderType()
        {
            List<string> reminders = new List<string>(Enum.GetNames(typeof(ReminderTypeOptions)));
            ViewBag.RemindersType = reminders.Select(
               temp => new SelectListItem()
               {
                   Text = temp.Replace("_", " "),
                   Value = temp
               });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeReminderStatus(Guid? ID)
        {
            if(ID == null)
            {
                TempData["Errors"] = "Erro ao fornecer ID do lembrete. Tente novamente mais tarde";

                return RedirectToAction("ListReminders");
            }

            ReminderResponse? reminderResponse = await _ReminderGetterService.GetReminderByID(ID);

            if (reminderResponse == null)
            {
                TempData["Errors"] = "Erro ao encotrar o lembrete. Tente novamente mais tarde";

                return RedirectToAction("ListReminders");
            }

            reminderResponse.Finished = true;

            ReminderResponse? reminderResponseUpdated = await _ReminderUpdaterService.UpdateReminder(reminderResponse.ToPatientUpdateRequest());

            if (reminderResponseUpdated == null)
            {
                TempData["Errors"] = "Algo deu errado ao atualizar o lembrete. Tente novamente mais tarde";
            }

            TempData["Success"] = "Lembrete alterado com sucesso";

            return RedirectToAction("ListReminders");
        }            
    }
}
