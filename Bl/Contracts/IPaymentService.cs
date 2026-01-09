using System.Threading.Tasks;

namespace Bl.Contracts
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "usd", int orderId = 0);
    }
}

