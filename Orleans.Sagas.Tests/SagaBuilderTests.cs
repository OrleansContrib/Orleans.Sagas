using System.Threading.Tasks;
using Moq;
using Xunit;
using System;
using System.Linq;

namespace Orleans.Sagas.Tests
{
    public class SagaBuilderTests
    {
        private ISagaBuilder subject;

        private Mock<IGrainFactory> mockGrainFactory;

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
            var builder = subject.AddActivity(new TestActivity());

            Assert.NotNull(builder);
        }

        [Fact]
        public async Task CanExecuteSaga()
        {
            subject.AddActivity(new TestActivity());
            mockGrainFactory
                .Setup(x => x.GetGrain<ISagaGrain>(It.IsAny<Guid>(), null))
                .Returns(new Mock<ISagaGrain>().Object);

            await subject.ExecuteSaga();
        }

        [Fact]
        public async Task ThrowsWhenExecutionAttemptedWithNoActivtiesInSaga()
        {
            mockGrainFactory
                .Setup(x => x.GetGrain<ISagaGrain>(It.IsAny<Guid>(), null))
                .Returns(new Mock<ISagaGrain>().Object);

            await Assert.ThrowsAsync<IndexOutOfRangeException>(() =>
                subject.ExecuteSaga()
            );
        }

        private class TestActivity : Activity
        {
            public override Task Compensate()
            {
                throw new NotImplementedException();
            }

            public override Task Execute()
            {
                throw new NotImplementedException();
            }
        }

        private class TestConfigurableActivity : Activity<int>
        {
            public override Task Compensate()
            {
                throw new NotImplementedException();
            }

            public override Task Execute()
            {
                throw new NotImplementedException();
            }
        }
    }
}
