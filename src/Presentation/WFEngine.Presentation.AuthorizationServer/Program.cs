using WFEngine.Presentation.AuthorizationServer.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddWFEngineAuthorizationDependencies(builder.Configuration);
builder.AddOpenIdDictServer(builder.Configuration);

var app = builder.Build();

app.AddWFEngineAuthorizationDependencies();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
