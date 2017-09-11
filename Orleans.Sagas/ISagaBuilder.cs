using System;
using System.Threading.Tasks;
using Orleans.Sagas;

namespace Orleans
{
    public interface ISagaBuilder
    {
        /// <summary>
        /// Returns the unique identifier for the saga this builder will execute.
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Adds an activity to this saga.
        /// </summary>
        /// <param name="activity">The activity to add.</param>
        /// <returns>The ISagaBuilder.</returns>
        ISagaBuilder AddActivity(IActivity activity);
        /// <summary>
        /// Executes this saga and returns once the saga has been registered. Idempotent.
        /// </summary>
        /// <returns>A reference to the saga.</returns>
        Task<ISagaGrain> ExecuteSaga();
    }
}