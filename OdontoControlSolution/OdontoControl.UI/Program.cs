using OdontoControl.UI.Middleware;
using OdontoControl.UI.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddHostedService<ConfigureAdminUserExtensions>();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

app.UseHsts();
app.UseHttpsRedirection();


app.UseStaticFiles();
app.UseRouting(); 
app.UseAuthentication(); 
app.UseAuthorization();
app.MapControllers(); 

app.Run();

public partial class Program { }
