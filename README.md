# Orleans.Sagas
A distributed saga implementation for Orleans.

# Status
This is a demonstration of a programming model for sagas in Orleans for discussion [here](https://github.com/dotnet/orleans/issues/3378). The code has not been load tested and is therefore **not recommended for production use** yet.

This project uses pre-release .NET Core Orleans libraries so you'll need to add a reference to here:
https://dotnet.myget.org/F/orleans-prerelease/api/v3/index.json

You can install the package with **Install-Package Orleans.Sagas -Pre**

# Usage


## Designing activity configs
```
public class BookHireCarConfig
{
    public bool IsClownCar { get; set; }
    public Guid HireCarRequestGuid { get; set; }
}
```

## Designing activities
```
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
```
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
