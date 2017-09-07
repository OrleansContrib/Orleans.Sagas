using System.Threading.Tasks;
using Moq;
using Xunit;
using System;

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
        public void CanAddActivity()
        {
            subject.AddActivity<TestActivity>();
        }

        [Fact]
        public void CanAddActivityWithConfig()
        {
            subject.AddActivity<TestConfigurableActivity>(1);
        }

        [Fact]
        public async Task CanExecuteSaga()
        {
            mockGrainFactory
                .Setup(x => x.GetGrain<ISagaGrain>(It.IsAny<Guid>(), null))
                .Returns(new Mock<ISagaGrain>().Object);

            await subject.Execute();
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
