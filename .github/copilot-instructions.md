# Clean Architecture For Single API Project

Este proyecto usa un solo proyecto API, pero debe mantener separacion logica de Clean Architecture mediante carpetas, namespaces y reglas de dependencia.

## Estructura general

- Mantener todo dentro del proyecto `Api`, sin crear proyectos adicionales salvo que se solicite explicitamente.
- Organizar el codigo por responsabilidades internas:
  - `Domain` para reglas de negocio puras.
  - `Application` para casos de uso con CQRS.
  - `Infrastructure` para integraciones y responsabilidades externas.
- La direccion de dependencias siempre debe apuntar hacia adentro:
  - `Infrastructure` puede depender de `Application` y `Domain`.
  - `Application` puede depender de `Domain`.
  - `Domain` no debe depender de `Application`, `Infrastructure`, EF Core, ASP.NET Core ni librerias externas de transporte o persistencia.

## Domain

- Ubicar entidades, objetos de valor, enumeraciones, reglas de negocio, eventos de dominio y excepciones de negocio dentro de `Domain`.
- Las entidades deben proteger sus invariantes y exponer comportamiento, no solo datos.
- Los objetos de valor deben representarse con `Vogen`; no crear objetos de valor manuales cuando el caso encaje en este patron.
- No colocar DTOs, requests HTTP, responses HTTP, controladores, endpoints, DbContext, configuraciones de EF ni clientes externos en `Domain`.

Estructura sugerida:

```text
Api/
  Domain/
    Entities/
    ValueObjects/
    Enums/
    Exceptions/
    Events/
    Common/
```

## Application

- Implementar CQRS basado en features y vertical slice.
- Cada feature debe agrupar su propio comando o consulta con todos los elementos necesarios para el caso de uso.
- Priorizar slices pequenos, cohesionados y orientados al comportamiento.
- No crear capas horizontales de servicios genericos si el comportamiento pertenece claramente a una feature.
- Cada slice puede incluir, segun necesidad:
  - `Command` o `Query`
  - `Handler`
  - `Validator`
  - `Result` o `Response`
  - `Mapping`
  - `Specification` o reglas propias de la feature cuando aplique
- Las features deben depender de abstracciones, no de detalles concretos de infraestructura.

Estructura sugerida:

```text
Api/
  Application/
    Abstractions/
    Behaviors/
    Common/
    Features/
      Customers/
        CreateCustomer/
          CreateCustomerCommand.cs
          CreateCustomerHandler.cs
          CreateCustomerValidator.cs
          CreateCustomerResult.cs
        GetCustomerById/
          GetCustomerByIdQuery.cs
          GetCustomerByIdHandler.cs
          GetCustomerByIdResult.cs
```

## Infrastructure

- `Infrastructure` concentra detalles tecnicos y comunicacion con responsabilidades externas.
- Dividirla al menos en:
  - `Infrastructure/Api` para endpoints, route groups, configuracion HTTP, filtros, autenticacion, versionado y contratos expuestos hacia afuera.
  - `Infrastructure/Data` para persistencia, `DbContext`, configuraciones de entidades, migraciones, seed y acceso a base de datos.
- Tambien pueden agregarse subcarpetas como `Messaging`, `Identity`, `Storage` o `Integrations` cuando existan dependencias externas reales.
- No mover logica de negocio a `Infrastructure`; ahi solo deben vivir adaptadores y detalles de implementacion.

Estructura sugerida:

```text
Api/
  Infrastructure/
    Api/
      Endpoints/
      RouteGroups/
      Filters/
      Contracts/
    Data/
      Context/
      Configurations/
      Migrations/
      Seed/
      Repositories/
```

## Reglas para API

- Usar controladores agrupados por feature si no se indica lo contrario.
- Los endpoints deben ser delgados: reciben request, delegan al caso de uso y traducen el resultado a HTTP.
- Evitar que los endpoints contengan logica de negocio.
- Los contratos HTTP deben representar necesidades de transporte, no exponer directamente entidades de dominio.
- Este proyecto usa controladores; no introducir Minimal APIs ni FastEndpoints salvo instruccion explicita.

## Reglas para Data

- La persistencia debe modelar el dominio sin contaminarlo con detalles de EF Core.
- Mantener configuraciones de EF separadas de las entidades.
- Usar `IEntityTypeConfiguration<T>` para mapeos cuando aplique.
- Preferir `ApplicationDbContext` directo dentro de los handlers cuando el proyecto lo requiera, en vez de abstraer EF Core con repositorios o interfaces artificiales.

## Convenciones de implementacion

- Antes de agregar codigo, decidir si pertenece a `Domain`, `Application` o `Infrastructure`.
- Favorecer nombres explicitos orientados al lenguaje del negocio.
- Mantener una feature completa en una sola carpeta vertical siempre que sea razonable.
- Evitar utilitarios globales si la logica pertenece a una feature concreta.
- No introducir dependencias nuevas sin necesidad clara.
- Mantener cambios pequenos, cohesionados y consistentes con esta estructura.
- La clase base `Entity` debe contener solo campos reutilizables.
- Si un valor requiere tipado fuerte, declararlo con `Vogen` dentro de `Domain/ValueObjects`.
- Despues de cada actualizacion relevante, ejecutar `dotnet build` antes de continuar.
- Los tests funcionales deben vivir en un proyecto separado y no deben invocar handlers directamente.
- Los tests funcionales deben levantar la aplicacion con DI completo, usar otro AppHost de test para infraestructura y poder hablar con la base de datos real de test.
- Para tests usar `NUnit`, `Shouldly`, `Bogus`, `Respawn`, `Aspire.Hosting.Testing` y `Microsoft.AspNetCore.Mvc.Testing` cuando aplique.
- Separar los tests por `Commands`, `Queries` y `EdgeCases` cuando el feature lo justifique.
- Si un test nuevo o existente falla de forma inesperada, no cambiar el comportamiento funcional para esconder el problema sin confirmarlo antes con el usuario.

## Regla de oro

- Si una pieza de codigo expresa reglas del negocio, va en `Domain`.
- Si orquesta un caso de uso, va en `Application`.
- Si habla con HTTP, base de datos o sistemas externos, va en `Infrastructure`.

## Soluciones implementadas

- La unidad `Usuario` se implementa con entidad en `Domain/Entities`, base `Entity` en `Domain/Common` y objeto de valor `Email` generado con `Vogen` en `Domain/ValueObjects`.
- Las features de `Usuario` viven en `Application/Features/Usuarios/<FeatureName>` con `Command` o `Query`, `Handler` y `Result`.
- Los endpoints HTTP de `Usuario` viven en `Infrastructure/Api/Controllers` y solo delegan a handlers de aplicacion.
- `Usuario` utiliza `ApplicationDbContext` en `Infrastructure/Data`, configuracion EF Core separada y listado paginado con `pageNumber` y `pageSize`.
- El listado de usuarios no debe exponer `Email` si la feature no lo necesita.
- La infraestructura de tests funcionales usa `Api.Tests.AppHost` para SQL Server efimero y `Api.FunctionalTests` para pruebas end to end y de pipeline con DI real.
- El CRUD de `Usuario` usa `soft delete`; las consultas deben ignorar usuarios eliminados y la eliminacion solo marca el registro.