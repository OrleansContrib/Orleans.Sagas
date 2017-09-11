using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public static class ISagaGrainExtensions
    {
        /// <summary>
        /// Waits for a saga to complete by periodically querying it's status.
        /// </summary>
        /// <param name="that"></param>
        /// <param name="queryFrequency">How often to query the saga.</param>
        /// <returns></returns>
        public static async Task Wait(this ISagaGrain that, int queryFrequency = 1000)
        {
            await Wait(new ISagaGrain[] { that }, queryFrequency);
        }

        /// <summary>
        /// Waits for an IEnumerable of sagas to complete by periodically querying
        /// their status.
        /// </summary>
        /// <param name="that"></param>
        /// <param name="queryFrequency">How often to query the sagas.</param>
        /// <returns></returns>
        public static async Task Wait(this IEnumerable<ISagaGrain> that, int queryFrequency = 1000)
        {
            var sagas = that.ToList();
            while (sagas.Count > 0)
            {
                var completed = new List<ISagaGrain>();

                foreach (var saga in sagas)
                {
                    if (await saga.HasCompleted())
                    {
                        completed.Add(saga);
                    }
                }

                sagas.RemoveAll(l => completed.Contains(l));

                await Task.Delay(queryFrequency);
            }
        }
    }
}
