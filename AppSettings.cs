using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager;


public class TelegramServiceOptions
{
    public string TelegramOptions = "TelegramOptions";
    public string Token { get; set; } = String.Empty;
    public string ChatId { get; set; } = String.Empty;


}

public class LogqueryOptions
{
    public string LogqueryOption = "Logquerys:Remoteaccess";
    public string logquery { get; set; } = String.Empty;
    public string QueryId { get; set; } = String.Empty;

}
public class AppSettings
{
    public TelegramServiceOptions? Telegramservice { get; set; }

}
