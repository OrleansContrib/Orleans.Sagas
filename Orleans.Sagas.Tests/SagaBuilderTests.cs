using System.Threading.Tasks;
using Moq;
using Xunit;
using System;
using System.Linq;
using Orleans.Runtime;

namespace Orleans.Sagas.Tests
{
    public class SagaBuilderTests
    {
        private readonly ISagaBuilder subject;

        private readonly Mock<IGrainFactory> mockGrainFactory;

        public SagaBuilderTests()
        {
            mockGrainFactory = new Mock<IGrainFactory>();
            subject = new SagaBuilder(mockGrainFactory.Object);
        }

        [Fact]
        public void InstanceIdsAreUnique()
        {
            var ids = Enumerable.Range(0, 10)
                .Select(x => new SagaBuilder(mockGrainFactory.Object).Id);

            Assert.Equal(ids.Count(), ids.Distinct().Count());
        }

        [Fact]
        public void CanAddActivity()
        {
            var builder = subject.AddActivity<TestActivity>();

            Assert.NotNull(builder);
        }

        [Fact]
        public async Task CanExecuteSaga()
        {
            subject.AddActivity<TestActivity>();
            mockGrainFactory
                .Setup(x => x.GetGrain<ISagaGrain>(It.IsAny<Guid>(), null))
                .Returns(new Mock<ISagaGrain>().Object);

            await subject.ExecuteSagaAsync();
        }

        [Fact]
        public async Task ThrowsWhenExecutionAttemptedWithNoActivtiesInSaga()
        {
            mockGrainFactory
                .Setup(x => x.GetGrain<ISagaGrain>(It.IsAny<Guid>(), null))
                .Returns(new Mock<ISagaGrain>().Object);

            await Assert.ThrowsAsync<IndexOutOfRangeException>(() =>
                subject.ExecuteSagaAsync()
            );
        }

        private class TestActivity : Activity
        {
            public override Task Compensate(IActivityRuntimeContext context)
            {
                throw new NotImplementedException();
            }

            public override Task Execute(IActivityRuntimeContext context)
            {
                throw new NotImplementedException();
            }
        }

        private class TestConfigurableActivity : Activity<int>
        {
            public override Task Compensate(IActivityRuntimeContext context)
            {
                throw new NotImplementedException();
            }

            public override Task Execute(IActivityRuntimeContext context)
            {
                throw new NotImplementedException();
            }
        }
    }
}
