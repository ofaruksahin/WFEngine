using WFEngine.Presentation.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddWFEngineApiDependencies(builder.Configuration);

var app = builder.Build();
app.AddWFEngineAuthorizationDependencies();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
