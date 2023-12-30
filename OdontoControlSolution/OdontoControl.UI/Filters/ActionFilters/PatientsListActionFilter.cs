using Microsoft.AspNetCore.Mvc.Filters;
using OdontoControl.Controllers;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.DTO.DentistDTO;
using OdontoControl.Core.DTO.PatientDTO;
using OdontoControl.Core.Enums;
using OdontoControl.UI.Areas.Admin.Controllers;
namespace OdontoControl.UI.Filters.ActionFilters
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
