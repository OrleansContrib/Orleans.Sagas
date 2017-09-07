using System;
using System.Threading.Tasks;
using Orleans.Sagas;

namespace Orleans
{
    public interface ISagaBuilder
    {
        Guid Id { get; }
        void AddActivity<TActivity>() where TActivity : IActivity;
        void AddActivity<TActivity>(object config) where TActivity : IActivity;
        Task Execute();
    }
}