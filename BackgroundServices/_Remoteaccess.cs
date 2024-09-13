using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace logservice;


    class Myremotelog : BackgroundService
{
    

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)

    {
        while (!stoppingToken.IsCancellationRequested)
        {
            
            await Task.Delay(1000, stoppingToken);
        }
    } 

}




