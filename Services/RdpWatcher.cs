using System;
using System.CodeDom;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace EventManager;

class RdpWatcher
{
    private string _logquery = string.Empty;
    private string _queryId = string.Empty;
    private readonly ITelegramService _TelegramMessage;
    private readonly IConfiguration Configuration;
    private readonly ILogger _Logger;

 
    public RdpWatcher(ITelegramService botMessage, IConfiguration configuration, ILogger<RdpWatcher> logger)
    {
        var Logoptions = new LogqueryOptions();
        Configuration = configuration;
        Configuration.GetSection("Logquerys:Remoteaccess").Bind(Logoptions);
        _Logger = logger;
        _TelegramMessage = botMessage;
        _logquery = Logoptions.logquery;
        _queryId = Logoptions.QueryId;
        
    }

    public  void Eventwatcher()
    {
        
        EventLogQuery query = new EventLogQuery(_logquery,PathType.LogName, _queryId);
        EventLogWatcher watcher = new EventLogWatcher(query);
        _Logger.LogInformation($"Iniciando Monitoreo en {_logquery}");
        watcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(OnEventRecordWritten);
        watcher.Enabled = true;
       
        
    }

         
     public async void OnEventRecordWritten(object? sender, EventRecordWrittenEventArgs e)
     {
        if(e.EventRecord !=null)
        {
            var User = e.EventRecord.MachineName;
            var data = e.EventRecord.FormatDescription();
            string message= $"------EVENTMANAGER ALERT------ \n {User.ToString()} \n {data}";
            _Logger.LogInformation($"{message}");
            await _TelegramMessage.SendMessageAsync(message);
        }
        
     }


}


class RemoteaccessManager : BackgroundService
{
    private readonly RdpWatcher _watcher;
    private readonly ILogger _logger;
    public RemoteaccessManager(RdpWatcher watcher, ILogger<RdpWatcher> logger)
    {
        _watcher = watcher;
        _logger = logger;
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