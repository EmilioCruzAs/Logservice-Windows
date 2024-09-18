using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace logservice;


    class Myremotelog : BackgroundService
    {
        private readonly Watcher _watcher;
        public Myremotelog(Watcher watcher)
        {
            _watcher = watcher;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)

        {
            _watcher.Eventwatcher();
            while (!stoppingToken.IsCancellationRequested)
            {
                
                await Task.Delay(1000, stoppingToken);
            }
        } 

    }




