# Orleans.Sagas
A distributed saga implementation for Orleans

[![.NET Core](https://github.com/OrleansContrib/Orleans.Sagas/workflows/.NET%20Core/badge.svg)](https://github.com/OrleansContrib/Orleans.Sagas/actions?query=workflow%3A%22.NET+Core%22)
[![NuGet](https://img.shields.io/nuget/v/Orleans.Sagas.svg?style=flat)](https://www.nuget.org/packages/Orleans.Sagas)
[![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/OrleansContrib/Orleans.Sagas?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

# Project Status
The library is currently in user acceptance testing with its first major consumer and is therefore **not recommended for production use** yet and will likely require subsequent API changes prior to a supported release. 

The [NuGet package](https://www.nuget.org/packages/Orleans.Sagas) is available as a preview, and in the absence of detailed documentation, the [gitter chat](https://gitter.im/OrleansContrib/Orleans.Sagas) is a good place to start.

# Usage

## Installing
Install the [NuGet package](https://www.nuget.org/packages/Orleans.Sagas) with a package manager.

## Configuring Orleans for sagas
Add the following statements to your Orleans silo builder, and don't forget **you'll need reminders and a default storage provider** configured for durability.
```csharp
.UseOrleans(siloBuilder =>
{
    siloBuilder
        .UseSagas()
        .ConfigureApplicationParts(parts =>
        {
            parts.AddSagaParts();
        })
        ...
}
```

## Designing activities
```csharp
public class BookHireCarActivity : IActivity
{
    public async Task Execute(IActivityContext context)
    {
        // idempotently request a hire car from a hire car service.
        await HireCarService.Book(context.SagaId, context.SagaProperties.GetInt("HireCarModel"));
    }

    public async Task Compensate(IActivityContext context)
    {
        // idempotently cancel a hire car request from a hire car service.
        await HireCarService.Cancel(context.SagaId);
    }
}
```

## Executing a saga
```csharp
// create a saga builder.
var sagaBuilder = GrainFactory.CreateSaga();

// add activities to your saga, and optional associated configuration.
sagaBuilder
    .AddActivity<BookHireCarActivity>()
    .AddActivity<BookHotelActivity>()
    .AddActivity<BookPlaneActivity>()

// execute the saga (idempotent).
var saga = await sagaBuilder.ExecuteSaga(x => x.Add("HireCarModel", 1));

// abort the saga (idempotent).
await saga.RequestAbort();
```
