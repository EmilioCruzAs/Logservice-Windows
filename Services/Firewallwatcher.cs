using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace EventManager;

 class Firewallwatcher
{

    private string logquery = string.Empty;
    private string queryId = string.Empty;
    private readonly ITelegramService _TelegramMessage;
    private readonly IConfiguration Configuration;
    private readonly ILogger _Logger;

    public string? apikey;

    public Firewallwatcher(ITelegramService botmessage, IConfiguration configuration, ILogger<Firewallwatcher> logger)
    {
        var logoptions = new LogqueryOptions();
        Configuration = configuration;
        Configuration.GetSection("Logquerys:FirewallRules").Bind(logoptions);
        _Logger = logger;
        _TelegramMessage = botmessage;
        logquery = logoptions.logquery;
        queryId = logoptions.QueryId;
        apikey = Configuration["TelegramOptions:ChatId"];
    }

  
 

    public void EventAddRule()
    {
        EventLogQuery query = new EventLogQuery(logquery, PathType.LogName, queryId);
        EventLogWatcher eventLogWatcher = new EventLogWatcher(query);
        _Logger.LogInformation($"Iniciando Monitoreo en {logquery}");
        eventLogWatcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(OnEventRecordWritten);
        eventLogWatcher.Enabled = true;
    }

    public void EventDeleteRule() 
    {
        EventLogQuery query = new EventLogQuery(logquery, PathType.LogName, "*[System/EventID=2006]");
        EventLogWatcher eventLogWatcher = new EventLogWatcher(query);
        eventLogWatcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(OnEventRecordWritten);
        eventLogWatcher.Enabled = true;
    }


    public async void OnEventRecordWritten(object? sender, EventRecordWrittenEventArgs e)
    {
        if (e.EventRecord != null)
        {
            var Machinename = e.EventRecord.MachineName;
            var data = e.EventRecord.FormatDescription();
            string message = $"------EVENTMANAGER ALERT------ \n {Machinename.ToString()} \n {data}";
            _Logger.LogInformation(message);
            await _TelegramMessage.SendMessageAsync(message);
        }
        else { _Logger.LogError($"{e.EventException}"); }

    }

}


 class FirewallManager : BackgroundService
{
    private readonly Firewallwatcher _watcher;
    private readonly ILogger<FirewallManager> _logger;
    public FirewallManager(Firewallwatcher watcher, ILogger<FirewallManager> logger)
    {
        _logger = logger;
        _watcher = watcher;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        _watcher.EventAddRule();
        _watcher.EventDeleteRule();
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

}
