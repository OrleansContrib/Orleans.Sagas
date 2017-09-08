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
        /// <typeparam name="TActivity">The activity type to add.</typeparam>
        void AddActivity<TActivity>() where TActivity : IActivity;
        /// <summary>
        /// Adds an activity to this saga with a corresponding config.
        /// The config must be of the same type as that of TActivityConfig in Activity<TActivityConfig>.
        /// </summary>
        /// <typeparam name="TActivity">The activity type to add.</typeparam>
        /// <param name="config">The corresponding configuration for this activity.</param>
        void AddActivity<TActivity>(object config) where TActivity : IActivity;
        /// <summary>
        /// Executes this saga and returns once the saga has been registered. Idempotent.
        /// </summary>
        /// <returns>A reference to the saga.</returns>
        Task<ISagaGrain> Execute();
    }
}