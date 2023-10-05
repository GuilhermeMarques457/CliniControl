using Microsoft.AspNetCore.Identity;
using OdontoControl.Core.Domain.IdentityEntities;
using OdontoControl.Core.DTO.AccountDTO;

namespace OdontoControl.UI.StartupExtensions
{
    public class ConfigureAdminUserExtensions : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public ConfigureAdminUserExtensions(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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

            if (await userManager.FindByNameAsync("guilherme.marques.santos457@gmail.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "guilherme.marques.santos457@gmail.com",
                };

                await userManager.CreateAsync(adminUser, "Gui19982014");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
