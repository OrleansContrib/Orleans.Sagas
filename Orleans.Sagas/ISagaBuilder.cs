using System;
using System.Threading.Tasks;

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
        /// <param name="propertiesDelegate">The properties for this saga.</param>
        /// <returns>The ISagaBuilder.</returns>
        ISagaBuilder AddActivity<TActivity>(Action<ISagaPropertyBag> propertiesDelegate) where TActivity : IActivity;

        /// <summary>
        /// Executes this saga and returns once the saga has been registered. Idempotent.
        /// </summary>
        /// <returns>A reference to the saga.</returns>
        Task<ISagaGrain> ExecuteSagaAsync();

        /// <summary>
        /// Executes this saga and returns once the saga has been registered. Idempotent.
        /// </summary>
        /// <param name="propertiesDelegate">The properties for this saga.</param>
        /// <returns>A reference to the saga.</returns>
        Task<ISagaGrain> ExecuteSagaAsync(Action<ISagaPropertyBag> propertiesDelegate);
    }
}