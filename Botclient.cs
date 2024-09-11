using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace logservice;

class BotClient
{
    private string  message;
    
    public BotClient ( string _message)
    {
        message =_message??"";
         

    }

    public async void Postasync()
    {   
        string token ="7348852621:AAGSDrs9anwxZ7pvR4S2ASX";
        string url = $"https://api.telegram.org/bot{token}/sendMessage?chat_id={7226331689}&text={Uri.EscapeDataString(message)}";
       
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
