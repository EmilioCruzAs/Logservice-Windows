using System.Runtime.CompilerServices;

namespace logservice;


class BotMessage
{
    private Uri url;
    private string _token;    
    private string _chat_id;
    private float _time;
    
   public BotMessage(string token, 
   string chat_id,
   float time)
   {
        _token=token;
        _chat_id = chat_id;
        float _time = time;
        url = new Uri($"https://api.telegram.org/bot{_token}/sendMessage");
   }



    public async Task sendMessage()
    {
        using(HttpClient client = new HttpClient())
        {
            try
            {
                var resquest = await client.GetAsync(url);
                var responseContent = await resquest.Content.ReadAsStringAsync();
                Console.WriteLine($"Respuesta del servidor: {responseContent}");

                if(resquest.IsSuccessStatusCode){Console.WriteLine("Mensaje Enviado COrrectamente");}else{Console.WriteLine($"que paso {resquest.StatusCode}");};

            }catch(HttpRequestException  e)
            {
                Console.WriteLine(e);
            }catch(UriFormatException e)
            {
                Console.WriteLine(e);
            }
            
        }
    }

}