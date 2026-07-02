# Api Clean Architecture Sample

Proyecto de ejemplo con Clean Architecture en un solo proyecto API, usando CQRS por feature, `Mediator`, `FluentValidation`, `Entity Framework Core`, `Aspire` y pruebas funcionales con AppHost separado.

## Estructura

- `Api/Domain`: entidades, objetos de valor y reglas de negocio.
- `Api/Application`: features, validators y behaviors.
- `Api/Infrastructure`: persistencia y configuración externa.
- `Api/Controllers`: endpoints HTTP con controladores.
- `Api.AppHost`: AppHost principal de Aspire.
- `Api.Tests.AppHost`: AppHost dedicado para pruebas funcionales.
- `Api.FunctionalTests`: pruebas funcionales y de pipeline.

## Usuario

La entidad `Usuario` implementa:

- Registro
- Obtención por id
- Obtención paginada
- Actualización
- Eliminación lógica (`soft delete`)

## Tecnologías

- .NET 10
- ASP.NET Core Web API
- Mediator
- FluentValidation
- Entity Framework Core SQL Server
- Aspire
- Vogen
- NUnit
- Shouldly
- Bogus
- Respawn

## Comandos útiles

### Compilar solución

```powershell
dotnet build .\Api.slnx
```

### Ejecutar AppHost principal

```powershell
cd .\Api.AppHost
aspire start --non-interactive
```

### Ejecutar pruebas funcionales

```powershell
dotnet test .\Api.FunctionalTests\Api.FunctionalTests.csproj
```

## Estado actual

- La solución compila correctamente.
- Las pruebas funcionales están pasando.
- La suite actual cubre commands, queries, edge cases y pipeline behaviors.