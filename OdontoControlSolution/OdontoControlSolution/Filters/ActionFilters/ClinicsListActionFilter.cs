using Microsoft.AspNetCore.Mvc.Filters;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.Enums;
using OdontoControl.UI.Areas.Admin.Controllers;

namespace OdontoControl.UI.Filters.ActionFilters
{
    public class ClinicsListActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();

            ClinicController clinicController = (ClinicController)context.Controller;

            clinicController.ViewBag.SearchFields = new Dictionary<string, string>()
                {
                    { nameof(ClinicResponse.ClinicName), "Nome da Cliníca" },
                    { nameof(ClinicResponse.City), "Cidade da Cliníca" },
                    { nameof(ClinicResponse.CNPJ), "CNPJ" },
                    { nameof(ClinicResponse.StreetName), "Nome da Rua" },
                    { nameof(ClinicResponse.Neighborhood), "Nome do Bairro" },
                    { nameof(ClinicResponse.Phone), "Telefone" },
                };
        }
    }
}
