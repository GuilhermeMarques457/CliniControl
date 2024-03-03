using Microsoft.AspNetCore.Mvc.Filters;
using CliniControl.Controllers;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.DTO.DentistDTO;
using CliniControl.Core.DTO.PatientDTO;
using CliniControl.Core.Enums;
using CliniControl.UI.Areas.Admin.Controllers;
namespace CliniControl.UI.Filters.ActionFilters
{
    public class PatientsListActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();

            PatientController patientController = (PatientController)context.Controller;

            patientController.ViewBag.SearchFields = new Dictionary<string, string>()
                {
                    { nameof(PatientResponse.PatientName), "Nome do Cliente" },
                    { nameof(PatientResponse.PhoneNumber), "Telefone" },
                    { nameof(PatientResponse.CPF), "CPF" },
                    { nameof(PatientResponse.Manager.PersonName), "Nome do Gerenciador" },
                    { nameof(PatientResponse.Gender), "Gênero" },
                };
        }
    }
}
