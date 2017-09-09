using System.Threading.Tasks;

namespace Orleans.Sagas.Samples.Interfaces
{
    public interface ITransferGrain : IGrainWithIntegerKey
    {
        Task<ISagaGrain> RequestTransfer(int fromAccount, int toAccount, int amount);
    }
}
