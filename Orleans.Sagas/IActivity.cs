using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public interface IActivity
    {
        Task Execute(IActivityContext context);
        Task Compensate(IActivityContext context);
    }
}
