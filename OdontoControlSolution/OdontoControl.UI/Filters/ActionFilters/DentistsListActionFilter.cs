using Microsoft.AspNetCore.Mvc.Filters;
using OdontoControl.Controllers;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.DTO.DentistDTO;
using OdontoControl.Core.Enums;
using OdontoControl.UI.Areas.Admin.Controllers;

namespace OdontoControl.UI.Filters.ActionFilters
{
    public class DentistsListActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();

            DentistController dentistController = (DentistController)context.Controller;

            dentistController.ViewBag.SearchFields = new Dictionary<string, string>()
                {
                    { nameof(DentistResponse.DentistName), "Nome da Dentista" },
                    { nameof(DentistResponse.PhoneNumber), "Telefone" },
                    { nameof(DentistResponse.StartTime), "Hora de Entrada" },
                    { nameof(DentistResponse.EndTime), "Hora de Saída" },
                    { nameof(DentistResponse.Manager), "Nome do Gerenciador" },
                };
        }
    }
}
