using Orleans.Sagas;
using System;

namespace Orleans
{
    public static class IGrainFactorySagaExtensions
    {
        /// <summary>
        /// Provides an ISagaBuilder which can be used to prepare and execute a saga.
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public static ISagaBuilder CreateSaga(this IGrainFactory that)
        {
            return new SagaBuilder(that);
        }

        /// <summary>
        /// Provides an ISagaBuilder which can be used to prepare and execute a saga.
        /// </summary>
        /// <param name="that"></param>
        /// <param name="sagaId"></param>
        /// <returns></returns>
        public static ISagaBuilder CreateSaga(this IGrainFactory that, Guid sagaId)
        {
            return new SagaBuilder(that, sagaId);
        }

        /// <summary>
        /// Returns a saga instance.
        /// </summary>
        /// <param name="that"></param>
        /// <param name="id">The id of the saga, provider by the ISagaBuilder.</param>
        /// <returns></returns>
        public static ISagaGrain GetSaga(this IGrainFactory that, Guid id)
        {
            return that.GetGrain<ISagaGrain>(id);
        }
    }
}
