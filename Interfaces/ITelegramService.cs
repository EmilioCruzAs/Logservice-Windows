namespace EventManager;
interface ITelegramService
{

    Task SendMessageAsync(string message); 

}