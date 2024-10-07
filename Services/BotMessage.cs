using System.Runtime.CompilerServices;

namespace EventManager;

class BotMessage : IBotMessage
{

    private string _token;    
    private string _chat_id;
  
    private float _time;
    private static  HttpClient client = new HttpClient();

   public BotMessage(string token, 
   string chat_id)
   {
        _token=token;
        _chat_id = chat_id;

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