using System.Threading.Tasks;
using Moq;
using Xunit;
using System;
using Orleans.Sagas.Exceptions;
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
            var builder = subject.AddActivity<TestActivity>();

            Assert.NotNull(builder);
        }

        [Fact]
        public void CanAddActivityWithConfig()
        {
            var builder = subject.AddActivity<TestConfigurableActivity>(1);

            Assert.NotNull(builder);
        }

        [Fact]
        public void ThrowsIfActivityIsAddedWithIncompatibleConfig()
        {
            Assert.Throws<IncompatibleActivityAndConfigException>(() =>
                subject.AddActivity<TestConfigurableActivity>(string.Empty)
            );
        }

        [Fact]
        public void ThrowsIfActivityWithConfigIsAddedWithoutAConfig()
        {
            Assert.Throws<ConfigNotProvidedException>(() =>
                subject.AddActivity<TestConfigurableActivity>()
            );
        }

        [Fact]
        public void ThrowsIfActivityWithConfigIsAddedWithANullConfig()
        {
            Assert.Throws<ConfigNotProvidedException>(() =>
                subject.AddActivity<TestConfigurableActivity>(null)
            );
        }

        [Fact]
        public void ThrowsIfActivityWithoutConfigIsAddedWithAConfig()
        {
            Assert.Throws<ConfigNotRequiredException>(() =>
                subject.AddActivity<TestActivity>(1)
            );
        }

        [Fact]
        public async Task CanExecuteSaga()
        {
            subject.AddActivity<TestActivity>();
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

            await Assert.ThrowsAsync<NoActivitiesInSagaException>(() =>
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
