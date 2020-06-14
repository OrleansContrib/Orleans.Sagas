using System;
using System.Threading.Tasks;
using Orleans.Sagas;

namespace Orleans.Sagas
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
        /// <typeparam name="TActivity">The activity to add.</typeparam>
        /// <returns>The ISagaBuilder.</returns>
        ISagaBuilder AddActivity<TActivity>() where TActivity : IActivity;

        /// <summary>
        /// Adds an activity to this saga.
        /// </summary>
        /// <typeparam name="TActivity">The activity to add.</typeparam>
        /// <param name="config">The config for this activity.</param>
        /// <returns>The ISagaBuilder.</returns>
        ISagaBuilder AddActivity<TActivity, TConfig>(TConfig config) where TActivity : IActivity<TConfig>;

        /// <summary>
        /// Adds an activity to this saga.
        /// </summary>
        /// <typeparam name="TActivity">The activity to add.</typeparam>
        /// <param name="configDelegate">The config for this activity.</param>
        /// <returns>The ISagaBuilder.</returns>
        ISagaBuilder AddActivity<TActivity, TConfig>(Action<TConfig> configDelegate) where TActivity : IActivity<TConfig>;

        /// <summary>
        /// Executes this saga and returns once the saga has been registered. Idempotent.
        /// </summary>
        /// <returns>A reference to the saga.</returns>
        Task<ISagaGrain> ExecuteSagaAsync();
    }
}