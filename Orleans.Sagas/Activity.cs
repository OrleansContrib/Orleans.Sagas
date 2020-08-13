using System.Threading.Tasks;

namespace Orleans.Sagas
{
    public abstract class Activity : IActivity
    {
        public string Name { get; }

        public Activity()
        {
            Name = GetType().Name;
        }

        public abstract Task Execute(IActivityContext context);
        public abstract Task Compensate(IActivityContext context);
    }
}
