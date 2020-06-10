# Orleans.Sagas
A distributed saga implementation for Orleans

![.NET Core](https://github.com/OrleansContrib/Orleans.Sagas/workflows/.NET%20Core/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/Orleans.Sagas.svg?style=flat)](https://www.nuget.org/packages/Orleans.Sagas)
[![Gitter](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/OrleansContrib/Orleans.Sagas?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

# Project Status
The code has not been load tested and is therefore **not recommended for production use** yet.

# Usage

## Designing activity configs
```csharp
public class BookHireCarConfig
{
    public bool IsClownCar { get; set; }
    public Guid HireCarRequestGuid { get; set; }
}
```

## Designing activities
```csharp
public class BookHireCarActivity : Activity<BookHireCarConfig>
{
    public override async Task Execute()
    {
        // idempotently request a hire car from a hire car service.
        await HireCarService.Book(Config.HireCarRequestGuid, Config.IsClownCar);
    }

    public override async Task Compensate()
    {
        // idempotently cancel a hire car request from a hire car service.
        await HireCarService.Cancel(Config.HireCarRequestGuid);
    }
}
```

## Executing a saga
```csharp
// create a saga builder.
var sagaBuilder = GrainFactory.CreateSaga();

// add activities to your saga, and optional associated configuration.
sagaBuilder
    .AddActivity(new BookHireCarActivity{Config = new BookHireCarConfig { HireCarRequestGuid = Guid.NewGuid(); }})
    .AddActivity(new BookHotelActivity())
    .AddActivity(new BookPlaneActivity())

// execute the saga (idempotent).
var saga = await sagaBuilder.ExecuteSaga();

// abort the saga (idempotent).
await saga.RequestAbort();
```
