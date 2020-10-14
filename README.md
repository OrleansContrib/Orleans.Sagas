# Orleans.Sagas
A distributed saga implementation for Orleans

[![.NET Core](https://github.com/OrleansContrib/Orleans.Sagas/workflows/.NET%20Core/badge.svg)](https://github.com/OrleansContrib/Orleans.Sagas/actions?query=workflow%3A%22.NET+Core%22)
[![NuGet](https://img.shields.io/nuget/v/Orleans.Sagas.svg?style=flat)](https://www.nuget.org/packages/Orleans.Sagas)
[![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/OrleansContrib/Orleans.Sagas?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

# Overview
Orleans.Sagas sits on top of the [Orleans](https://github.com/dotnet/orleans) framework, enabling sagas to scale up across a cluster of machines. However the Orleans.Sagas library does not require any detailed knowledge of Orleans. It is primarily designed to make it effortless to write robust and horizontally scalable sagas (compensatable workflows) for .NET with very minimal knowledge of distributed systems.

If you are familiar with Orleans you can use Orleans.Sagas to orchestrate long running workflows which involve Orleans grains, or a combination of grains and external services.

# Status
The library is currently in user acceptance testing with its first major consumer and is therefore **not recommended for production use** yet and will likely require subsequent API changes prior to a supported release.

The [NuGet package](https://www.nuget.org/packages/Orleans.Sagas) is available as a preview, and in the absence of detailed documentation, the [gitter chat](https://gitter.im/OrleansContrib/Orleans.Sagas) is a good place to start.

# References
- Garcia-Molina, H. - *[Sagas](http://www.cs.cornell.edu/andru/cs711/2002fa/reading/sagas.pdf)* (1987)
- Eldeeb, T. and Bernstein, P. - *[Transactions for Distributed Actors in the Cloud](https://www.microsoft.com/en-us/research/wp-content/uploads/2016/10/EldeebBernstein-TransactionalActors-MSR-TR-1.pdf)* (2016)
- McCaffrey, C. - *[Applying the Saga Pattern](https://www.youtube.com/watch?v=xDuwrtwYHu8)* (2015)

# Usage

## Installing
Install the [NuGet package](https://www.nuget.org/packages/Orleans.Sagas) with a package manager.

## Configuring Orleans for sagas
1. Configure a default storage provider
2. Configure a reminder storage provider
3. Add the following statements to your Orleans silo builder:
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
        await HireCarService.Book(context.SagaId, context.SagaProperties.Get<int>("HireCarModel"));
    }

    public async Task Compensate(IActivityContext context)
    {
        // idempotently cancel a hire car request from a hire car service.
        await HireCarService.Cancel(context.SagaId);
    }
}
```

## Native dependency injection
```csharp
public class BookHireCarActivity : IActivity
{
    public BookHireCarActivity(IWasAddedAsAServiceOnStartUp myDependency)
    {
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
var saga = await sagaBuilder.ExecuteSagaAsync(x => x.Add("HireCarModel", 1));

// abort the saga (idempotent).
await saga.RequestAbort();
```
