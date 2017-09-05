using Orleans.Sagas.Samples.Duke.Activities;
using System;

namespace Orleans.Sagas.Samples.Duke.Interfaces
{
    public interface IDukeGrain : ISagaGrain, IGrainWithGuidKey
    {
    }
}
