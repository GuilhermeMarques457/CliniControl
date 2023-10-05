using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OdontoControl.Controllers;
using OdontoControl.Core.Domain.Entities;
using OdontoControl.Core.DTO.ClinicDTO;
using OdontoControl.Core.Enums;
using OdontoControl.UI.Areas.Admin.Controllers;

namespace OdontoControl.UI.Filters.ActionFilters
{
    public class CheckingTableSortingActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Items["arguments"] = context.ActionArguments;

            await next();

            Controller? matchedControler = null;

            if (context.Controller is ClinicController controllerClinic)
            {
                matchedControler = controllerClinic;
            }

            else if (context.Controller is DentistController controllerDentist)
            {
                matchedControler = controllerDentist;
            }

            else if (context.Controller is PatientController controllerPatient)
            {
                matchedControler = controllerPatient;
            }

            else if (context.Controller is AppointmentController controllerAppointement)
            {
                matchedControler = controllerAppointement;
            }

            IDictionary<string, object?>? parameters = (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];

            if(matchedControler != null)
            {
                if (parameters != null)
                {
                    if (parameters.TryGetValue("searchBy", out var searchBy))
                    {
                        matchedControler.ViewBag.CurrentSearchBy = Convert.ToString(parameters["searchBy"]);
                    }

                    if (parameters.ContainsKey("searchString"))
                    {
                        matchedControler.ViewBag.CurrentSearchString = Convert.ToString(parameters["searchString"]);
                    }

                    if (parameters.ContainsKey("sortBy"))
                    {
                        matchedControler.ViewBag.CurrentSortBy = Convert.ToString(parameters["sortBy"]);
                    }
                    else
                    {
                        matchedControler.ViewBag.CurrentSortBy = "";
                    }

                    if (parameters.ContainsKey("sortOrderOptions"))
                    {
                        matchedControler.ViewBag.CurrentSortOrder = Convert.ToString(parameters["sortOrderOptions"]);
                    }
                    else
                    {
                        matchedControler.ViewBag.CurrentSortOrder = nameof(SortOrderOptions.ASC);
                    }
                }
            }

           

            
        }
    }
}
