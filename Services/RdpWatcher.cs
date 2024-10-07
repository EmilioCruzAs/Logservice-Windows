using System;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
namespace EventManager;

class RdpWatcher
{
    private string? logquery;
    private string? queryId;
    private readonly ITelegramService _TelegramMessage;
    public IConfiguration Configuration { get; private set; }

    public RdpWatcher(ITelegramService botMessage, IConfiguration configuration)
    {
        _TelegramMessage = botMessage;
        logquery= "Microsoft-Windows-TerminalServices-RemoteConnectionManager/Operational";
        queryId="*[System/EventID=1149]";
        Configuration = configuration;

    }

    public  void Eventwatcher()
    {  
        EventLogQuery query = new EventLogQuery(logquery,PathType.LogName, queryId);
        Console.WriteLine("Esperando clientes");
       
        using (EventLogWatcher watcher = new EventLogWatcher(query)) 
        {
            try
            {
                watcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(OnEventRecordWritten);
                watcher.Enabled = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }

         
     public async void OnEventRecordWritten(object? sender, EventRecordWrittenEventArgs e)
     {
        if(e.EventRecord !=null)
        {
            Console.WriteLine("it works");
            var   user =  e.EventRecord.Properties[0].Value;
            var ip = e.EventRecord.Properties[2].Value;
            var time = e.EventRecord.TimeCreated.Value;
            string message= user.ToString() +" " +ip.ToString()+" " + time.ToString();
            await _TelegramMessage.SendMessageAsync(message);
        }
        
     }


}


class Myremotelog : BackgroundService
{
    private readonly RdpWatcher _watcher;
    public Myremotelog(RdpWatcher watcher)
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