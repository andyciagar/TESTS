using Api.Application.Common.Behaviors;
using Api.Application.Features.Usuarios;
using Api.Infrastructure.Data;
using Api.Infrastructure.Data.Initialization;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("bd")));
builder.EnrichSqlServerDbContext<ApplicationDbContext>();
builder.Services.AddValidatorsFromAssemblyContaining<RegistrarUsuarioCommandValidator>();
builder.Services.AddMediator(options =>
{
    options.Assemblies = [typeof(RegistrarUsuarioCommand)];
    options.ServiceLifetime = ServiceLifetime.Scoped;
    options.PipelineBehaviors =
    [
        typeof(LoggingBehavior<,>),
        typeof(TimingBehavior<,>),
        typeof(ValidationBehavior<,>)
    ];
});
builder.Services.AddScoped<DatabaseInitializer>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await initializer.InitializeAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program;
