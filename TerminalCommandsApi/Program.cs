using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using TerminalCommandsApi.Configurations;
using TerminalCommandsApi.Extensions;
using TerminalCommandsApi.Hubs;
using TerminalCommandsApi.Routes;


var builder = WebApplication.CreateBuilder(args);
//services configuration

builder.Services.AddServices(builder.Configuration);


var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TerminalCommandsApi v1"));
}


app.ConfigureCommanderExceptionHandler();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<DataBaseMessageHub>("databaseHub");

app.MapControllers();

app.MapGroup("api/auth").MapAuthEndpoints();

app.MapGroup("api/commands")
    .MapCommandRoutes()
    .RequireAuthorization();


app.Run();