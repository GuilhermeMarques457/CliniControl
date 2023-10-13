using Microsoft.AspNetCore.Identity;
using OdontoControl.Core.Domain.IdentityEntities;
using OdontoControl.Core.DTO.AccountDTO;

namespace OdontoControl.UI.StartupExtensions
{
    public class ConfigureAdminUserExtensions : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public ConfigureAdminUserExtensions(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            if (await roleManager.FindByNameAsync("Admin") is null)
            {
                ApplicationRole applicationRole = new ApplicationRole()
                {
                    Name = "Admin"
                };

                await roleManager.CreateAsync(applicationRole);
            }

            string userName = _configuration["AdminUser:UserName"]!;
            string password = _configuration["AdminUser:Password"]!;

            if (await userManager.FindByNameAsync(userName) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = userName
                };

                await userManager.CreateAsync(adminUser, password);
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
