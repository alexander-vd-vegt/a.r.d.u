using System;
using Ardu.Common.Interfaces;

namespace mk1_iic_controller.Workers;

public class ServoControlWorker : IHostedService
{
    private readonly IEventbus _events;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

}
