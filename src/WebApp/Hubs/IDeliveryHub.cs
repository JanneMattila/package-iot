using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Hubs
{
    public interface IDeliveryHub
    {
        Task Echo(DeliveryModel deliveryModel);
    }
}
