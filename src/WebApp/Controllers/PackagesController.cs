using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Hubs;
using WebApp.Interfaces;

namespace WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PackagesController : ControllerBase
{
    private readonly ILogger<PackagesController> _logger;
    private readonly IHubContext<DeliveryHub, IDeliveryHub> _hub;

    public PackagesController(ILogger<PackagesController> logger, IHubContext<DeliveryHub, IDeliveryHub> hub)
    {
        _logger = logger;
        _hub = hub;
    }

    [HttpGet]
    public ActionResult Get()
    {
        var packages = new List<PackageDeviceMovement>();

        return Ok(packages);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put(int id, [FromBody] PackageDeviceMovement packageDeviceMovement)
    {
        await _hub.Clients.All.UpdatePackages(new List<PackageDeviceMovement>() { packageDeviceMovement });

        return Ok();
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
