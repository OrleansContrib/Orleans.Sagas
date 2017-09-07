using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public interface ISagaGrain : IGrainWithGuidKey, IRemindable
    {
        /// <summary>
        /// Registers an intent to abort the saga.
        /// </summary>
        /// <returns></returns>
        Task Abort();
        /// <summary>
        /// Executes a saga with the corresponding activities. Activities can only be supplied once,
        /// and will be ignored in subsequent calls to this method.
        /// </summary>
        /// <param name="activities">The activities and associated configs for this saga.</param>
        /// <returns></returns>
        Task Execute(IEnumerable<Tuple<Type, object>> activities);
        /// <summary>
        /// Exposes the status of this saga.
        /// </summary>
        /// <returns>The status of this saga.</returns>
        Task<SagaStatus> GetStatus();
        /// <summary>
        /// Resumes the saga. In normal operation this should be automatically called by the
        /// reminder scheduler.
        /// </summary>
        /// <returns></returns>
        Task Resume();
    }
}