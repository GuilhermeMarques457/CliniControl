using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdontoControl.Core.Domain.IdentityEntities;
using OdontoControl.Core.Domain.RepositoryContracts;
using OdontoControl.Core.ServiceContracts.AppointmentContracts;
using OdontoControl.Core.ServiceContracts.ClinicContracts;
using OdontoControl.Core.ServiceContracts.DentistContracts;
using OdontoControl.Core.ServiceContracts.PatientContracts;
using OdontoControl.Core.ServiceContracts.ReminderContracts;
using OdontoControl.Core.ServiceContracts.RequestedPatientContracts;
using OdontoControl.Core.ServiceContracts.TwilioContracts;
using OdontoControl.Core.Services;
using OdontoControl.Core.Services.AppointmentService;
using OdontoControl.Core.Services.ClinicService;
using OdontoControl.Core.Services.DentistService;
using OdontoControl.Core.Services.PatientService;
using OdontoControl.Core.Services.ReminderService;
using OdontoControl.Core.Services.RequestedPatientService;
using OdontoControl.Infrastructure.DbContext;
using OdontoControl.Infrastructure.Repositories;
using OdontoControl.UI.Middleware;

namespace OdontoControl.UI.StartupExtensions
{
    public static class ConfigureServiceExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddScoped<IAppointmentAdderService, AppointmentAdderService>();
            services.AddScoped<IAppointmentUpdaterService, AppointmentUpdaterService>();
            services.AddScoped<IAppointmentDeleterService, AppointmentDeleterService>();
            services.AddScoped<IAppointmentGetterService, AppointmentGetterService>();
            services.AddScoped<IAppointmentSorterService, AppointmentSorterService>();

            services.AddScoped<IClinicAdderService, ClinicAdderService>();
            services.AddScoped<IClinicUpdaterService, ClinicUpdaterService>();
            services.AddScoped<IClinicDeleterService, ClinicDeleterService>();
            services.AddScoped<IClinicGetterService, ClinicGetterService>();
            services.AddScoped<IClinicSorterService, ClinicSorterService>();

            services.AddScoped<IDentistAdderService, DentistAdderService>();
            services.AddScoped<IDentistUpdaterService, DentistUpdaterService>();
            services.AddScoped<IDentistDeleterService, DentistDeleterService>();
            services.AddScoped<IDentistGetterService, DentistGetterService>();
            services.AddScoped<IDentistSorterService, DentistSorterService>();

            services.AddScoped<IPatientAdderService, PatientAdderService>();
            services.AddScoped<IPatientUpdaterService, PatientUpdaterService>();
            services.AddScoped<IPatientDeleterService, PatientDeleterService>();
            services.AddScoped<IPatientGetterService, PatientGetterService>();
            services.AddScoped<IPatientSorterService, PatientSorterService>();

            services.AddScoped<IRequestedPatientAdderService, RequestedPatientAdderService>();
            services.AddScoped<IRequestedPatientUpdaterService, RequestedPatientUpdaterService>();
            services.AddScoped<IRequestedPatientGetterService, RequestedPatientGetterService>();

            services.AddScoped<IReminderAdderService, ReminderAdderService>();
            services.AddScoped<IReminderUpdaterService, ReminderUpdaterService>();
            services.AddScoped<IReminderGetterService, ReminderGetterService>();

            services.AddScoped<ITwilioService, TwilioService>();

            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IClinicRepository, ClinicRepository>();
            services.AddScoped<IDentistRepository, DentistRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IRequestedPatientRepository, RequestedPatientRepository>();
            services.AddScoped<IReminderRepository, ReminderRepository>();
           
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

          
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 7;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredUniqueChars = 4;
            })
            .AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

            services.AddAuthorization(options =>
            {
                
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build(); 

                options.AddPolicy("OnlyIfNotLogged", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return !context.User.Identity!.IsAuthenticated;
                    });

                });
            });

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Account/Login";
            });

            return services;
        }
    }
}
