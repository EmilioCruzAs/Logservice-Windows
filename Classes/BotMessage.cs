using System.Runtime.CompilerServices;

namespace logservice;


class BotMessage : IBotMessage
{
   // private Uri url;
    private string _token;    
    private string _chat_id;
    //private string _message;
    private float _time;
    private static readonly HttpClient client = new HttpClient();
   public BotMessage(string token, 
   string chat_id)
   {
        _token=token;
        _chat_id = chat_id;
       // _message = message;
        
   }



    public async Task SendMessageAsync(string message)
    {
            var url = new Uri($"https://api.telegram.org/bot{_token}/sendMessage?{_chat_id}&text={message}");
            var resquest = await client.GetAsync(url);
            var responseContent = await resquest.Content.ReadAsStringAsync();
            Console.WriteLine($"Respuesta del servidor: {responseContent}");
            if(resquest.IsSuccessStatusCode)
            {
                Console.WriteLine("Mensaje Enviado COrrectamente");}else{Console.WriteLine($"que paso {resquest.StatusCode}");
                
            }
    }

}