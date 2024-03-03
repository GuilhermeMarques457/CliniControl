using Microsoft.AspNetCore.Mvc.Filters;
using CliniControl.Core.DTO.ClinicDTO;
using CliniControl.Core.Enums;
using CliniControl.UI.Areas.Admin.Controllers;

namespace CliniControl.UI.Filters.ActionFilters
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
