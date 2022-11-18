using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Interfaces;

namespace WebApp.Hubs
{
    public interface IDeliveryHub
    {
        Task UpdatePackages(List<PackageDeviceMovement> packageDeviceMovement);
    }
}
