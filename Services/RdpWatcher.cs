using System;
using System.Diagnostics.Eventing.Reader;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
namespace EventManager;

class RdpWatcher
{
    private static string logquery;
    private static string queryId;
    private static BotMessage botMessage;


    static RdpWatcher()
    {
       
        botMessage = new BotMessage("7341152621:AAEFNOjlxh7yGugbEGBokZjm2KiuZpZCBB0", "7226331689");
        logquery= "Microsoft-Windows-TerminalServices-RemoteConnectionManager/Operational";
        queryId="*[System/EventID=1149]";

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
            await botMessage.SendMessageAsync(message);
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