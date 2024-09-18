namespace logservice;
interface IBotMessage
{

    Task SendMessageAsync(string message); 

}