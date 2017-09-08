using Orleans.Sagas.Samples.Duke.Activities;
using Orleans.Sagas.Samples.Duke.Interfaces;
using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Duke.Grains
{
    public class DukeGrain : Grain, IDukeGrain
    {
        public async Task Execute()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            AddActivities(sagaBuilder);

            await sagaBuilder.Execute();
        }

        public async Task ExecuteAndAbort()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            AddActivities(sagaBuilder);

            var saga = await sagaBuilder.Execute();

            await saga.Abort();
        }

        public async Task AbortWithoutExecution()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            var saga = GrainFactory.GetSaga(sagaBuilder.Id);

            await saga.Abort();
        }

        public async Task AbortThenExecute()
        {
            var sagaBuilder = GrainFactory.CreateSaga();

            var saga = GrainFactory.GetSaga(sagaBuilder.Id);

            await saga.Abort();

            AddActivities(sagaBuilder);

            await sagaBuilder.Execute();
        }

        private static void AddActivities(ISagaBuilder sagaBuilder)
        {
            sagaBuilder.AddActivity<KickAssActivity>(new KickAssConfig { KickAssCount = 7 });
            sagaBuilder.AddActivity<ChewBubblegumActivity>();
        }
    }
}
