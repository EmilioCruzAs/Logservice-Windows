
using System;
using System.Diagnostics.Eventing.Reader;
namespace  logservice;
class Program
{
    public static void Eventwatcher()
    {
        
        string logquery= "Microsoft-Windows-TerminalServices-RemoteConnectionManager/Operational";
        string queryId="*[System/EventID=1149]";
        EventLogQuery query = new EventLogQuery(logquery,PathType.LogName, queryId);

        Console.WriteLine("Esperando clientes");

        using(EventLogWatcher watcher = new EventLogWatcher(query))
        {
            watcher.EventRecordWritten+= new EventHandler<EventRecordWrittenEventArgs>(OnEventRecordWritten);
            watcher.Enabled = true;
            Console.ReadKey();
        }

    }

         
     public static  void OnEventRecordWritten(object? sender, EventRecordWrittenEventArgs e)
     {
        if(e.EventRecord !=null)
        {
            object   user =  e.EventRecord.Properties[0].Value;
            object ip = e.EventRecord.Properties[2].Value;
            DateTime time = e.EventRecord.TimeCreated.Value;
            string message= user.ToString() +" " +ip.ToString()+" " + time.ToString();
            Console.WriteLine("EL CLIENTE SE HA CONECTADO");
            BotClient cliente = new BotClient(message);
            cliente.Postasync();
        }

     }

        
    
 static void Main(string[] args)
    {

        Eventwatcher();
        
    }


}