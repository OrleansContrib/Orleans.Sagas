using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public interface IActivity<TConfig> : IActivity
    {
        TConfig Config { set; get; }
    }

    public interface IActivity
    {
        string Name { get; }
        Task Execute(IActivityContext context);
        Task Compensate(IActivityContext context);
    }
}
