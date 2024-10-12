using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace EventManager;

class TelegramService : ITelegramService
{

    private string? _token;    
    private string? _chat_id;
    private static  HttpClient client = new HttpClient();
    private readonly IConfiguration Configuration; 
    private readonly ILogger _logger;

    public TelegramService(IConfiguration configuration, ILogger<TelegramService> logger)
    {
        Configuration = configuration;
        var Options = new TelegramServiceOptions();
        Configuration.GetSection(Options.TelegramOptions).Bind(Options);
        _logger = logger;
        _token = Options.Token;
        _chat_id = Options.ChatId;

    }



    public async Task SendMessageAsync(string message)
    {
        var url = new Uri($"https://api.telegram.org/bot{_token}/sendMessage?chat_id={_chat_id}&text={message}");

        using (client = new HttpClient()) 
        {
            try
            {
                var resquest = await client.GetAsync(url);
                var responseContent = await resquest.Content.ReadAsStringAsync();
                if (resquest.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Mensaje env�ado correctamente");
                }
                else
                {
                    _logger.LogWarning(responseContent);

                }
            }catch (HttpRequestException ex) { _logger.LogError($"{ex}"); }
        
        }
              
           
    }

}