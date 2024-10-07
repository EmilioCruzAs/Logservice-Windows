using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

namespace EventManager;

class TelegramService : ITelegramService
{

    private string? _token;    
    private string? _chat_id;

  
  
    private static  HttpClient client = new HttpClient();
 
   public IConfiguration Configuration { get; private set; }

    public TelegramService(IConfiguration configuration)
    {
        Configuration = configuration;
        _token = Configuration["TelegramServiceOptions:Token"];
        _chat_id =Configuration["TelegramServiceOptions:ChatId"];

   }



    public async Task SendMessageAsync(string message)
    {
        var url = new Uri($"https://api.telegram.org/bot{_token}/sendMessage?{_chat_id}&text={message}");

        using (client = new HttpClient()) 
        {
            try
            {
                var resquest = await client.GetAsync(url);
                var responseContent = await resquest.Content.ReadAsStringAsync();
                if (resquest.IsSuccessStatusCode)
                {
                    Console.WriteLine("Mensaje Enviado COrrectamente");
                }
                else
                {
                    Console.WriteLine($"Ha ocurrido un error {resquest.StatusCode}");

                }
            }
            catch (HttpRequestException ex) { Console.WriteLine(ex.Message); }
        
        }
              
           
    }

}