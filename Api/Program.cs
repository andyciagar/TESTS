using Api.Application.Abstractions;
using Api.Application.Features.Usuarios.CreateUsuario;
using Api.Application.Features.Usuarios.GetUsuarioById;
using Api.Infrastructure.Api.Endpoints;
using Api.Infrastructure.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<IUsuarioRepository, InMemoryUsuarioRepository>();
builder.Services.AddScoped<CreateUsuarioHandler>();
builder.Services.AddScoped<GetUsuarioByIdHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapUsuarioEndpoints();

app.Run();
