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
        Task RequestAbort();
        /// <summary>
        /// Executes a saga with the corresponding activities. Activities can only be supplied once,
        /// and will be ignored in subsequent calls to this method.
        /// </summary>
        /// <param name="activities">The activities for this saga.</param>
        /// <param name="sagaProperties">The properties for this saga.</param>
        /// <returns></returns>
        Task Execute(IEnumerable<ActivityDefinition> activities, ISagaPropertyBag sagaProperties);
        /// <summary>
        /// Exposes the status of this saga.
        /// </summary>
        /// <returns>The status of this saga.</returns>
        Task<SagaStatus> GetStatus();
        /// <summary>
        /// Exposes whether this saga has completed (either executed successfully, aborted successfully,
        /// or fully compensated due to failure.
        /// </summary>
        /// <returns>Returns whether this saga has completed.</returns>
        Task<bool> HasCompleted();
        /// <summary>
        /// Resumes the saga. In normal operation this should be automatically called by the
        /// reminder scheduler.
        /// </summary>
        /// <returns></returns>
        Task ResumeAsync();
    }
}