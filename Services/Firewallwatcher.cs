using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.Eventing.Reader;

namespace EventManager;

 class Firewallwatcher
{

    private string logquery = string.Empty;
    private string queryId = string.Empty;
    private readonly ITelegramService _TelegramMessage;
    private readonly IConfiguration Configuration;
    private readonly ILogger _Logger;


    public Firewallwatcher(ITelegramService botmessage, IConfiguration configuration, ILogger<Firewallwatcher> logger)
    {
        var logoptions = new LogqueryOptions();
        Configuration = configuration;
        Configuration.GetSection("Logquerys:FirewallRules").Bind(logoptions);
        _Logger = logger;
        _TelegramMessage = botmessage;
        logquery = logoptions.logquery;
        queryId = logoptions.QueryId;

    }

    public void EventRecord()
    {
        EventLogQuery query = new EventLogQuery(logquery, PathType.LogName, queryId);
        EventLogWatcher eventLogWatcher = new EventLogWatcher(query);
        _Logger.LogInformation(logquery + " " + queryId);
        eventLogWatcher.EventRecordWritten += new EventHandler<EventRecordWrittenEventArgs>(OnEventRecordWritten);
    }

    public async void OnEventRecordWritten(object? sender, EventRecordWrittenEventArgs e)
    {
        if (e.EventRecord != null)
        {
            var Machinename = e.EventRecord.MachineName;
            var user = e.EventRecord.Properties[0].Value;
            var ip = e.EventRecord.Properties[2].Value;
            var time = e.EventRecord.TimeCreated.Value;
            string message = user.ToString() + " " + ip.ToString() + " " + time.ToString() + " " + Machinename.ToString();
            _Logger.LogInformation(message);
            await _TelegramMessage.SendMessageAsync(message);
        }
        else { _Logger.LogError($"{e.EventException}"); }

    }

}


 class FirewallManager : BackgroundService
{
    private readonly Firewallwatcher _watcher;

    public FirewallManager(Firewallwatcher watcher)
    {

        _watcher = watcher;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _watcher.EventRecord();
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

}
