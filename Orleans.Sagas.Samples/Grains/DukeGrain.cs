using Orleans.Sagas.Samples.Activities;
using Orleans.Sagas.Samples.Interfaces;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Grains
{
    public class DukeGrain : Grain, IDukeGrain
    {
        public async Task<ISagaGrain> Execute()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            AddActivities(sagaBuilder);

            return await sagaBuilder.ExecuteSaga();
        }

        public async Task<ISagaGrain> ExecuteAndAbort()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            AddActivities(sagaBuilder);

            var saga = await sagaBuilder.ExecuteSaga();
            
            await saga.RequestAbort();

            return saga;
        }

        public async Task<ISagaGrain> AbortWithoutExecution()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            var saga = GrainFactory.GetSaga(sagaBuilder.Id);

            await saga.RequestAbort();

            return saga;
        }

        public async Task<ISagaGrain> AbortThenExecute()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            var saga = GrainFactory.GetSaga(sagaBuilder.Id);

            await saga.RequestAbort();

            AddActivities(sagaBuilder);

            await sagaBuilder.ExecuteSaga();

            return saga;
        }

        private static void AddActivities(ISagaBuilder sagaBuilder)
        {
            sagaBuilder.AddActivity<KickAssActivity>(new KickAssConfig { KickAssCount = 7 });
            sagaBuilder.AddActivity<ChewBubblegumActivity>();
        }
    }
}
