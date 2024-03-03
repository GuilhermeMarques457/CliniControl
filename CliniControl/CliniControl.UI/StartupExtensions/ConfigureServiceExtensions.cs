using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CliniControl.Core.Domain.IdentityEntities;
using CliniControl.Core.Domain.RepositoryContracts;
using CliniControl.Core.ServiceContracts.AppointmentContracts;
using CliniControl.Core.ServiceContracts.ClinicContracts;
using CliniControl.Core.ServiceContracts.DentistContracts;
using CliniControl.Core.ServiceContracts.PatientContracts;
using CliniControl.Core.ServiceContracts.ReminderContracts;
using CliniControl.Core.ServiceContracts.RequestedPatientContracts;
using CliniControl.Core.ServiceContracts.TwilioContracts;
using CliniControl.Core.Services;
using CliniControl.Core.Services.AppointmentService;
using CliniControl.Core.Services.ClinicService;
using CliniControl.Core.Services.DentistService;
using CliniControl.Core.Services.PatientService;
using CliniControl.Core.Services.ReminderService;
using CliniControl.Core.Services.RequestedPatientService;
using CliniControl.Infrastructure.DbContext;
using CliniControl.Infrastructure.Repositories;
using CliniControl.UI.Middleware;

namespace CliniControl.UI.StartupExtensions
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
                var connection = configuration.GetConnectionString("DefaultConnection");
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
