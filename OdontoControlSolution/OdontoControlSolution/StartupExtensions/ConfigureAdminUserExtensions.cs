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

            // Verificar se já existe um usuário admin
            string userName = _configuration["AdminUser:UserName"]!;
            var adminUser = await userManager.FindByNameAsync(userName);

            if (adminUser == null)
            {
                // Criar um novo usuário admin
                adminUser = new ApplicationUser
                {
                    UserName = userName
                };

                // Definir outras propriedades do usuário, se necessário

                // Criar o usuário no banco de dados
                await userManager.CreateAsync(adminUser, _configuration["AdminUser:Password"]!);

                // Atribuir a função "Admin" ao usuário
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
