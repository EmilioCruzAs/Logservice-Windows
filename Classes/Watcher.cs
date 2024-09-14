using System;
using System.Diagnostics.Eventing.Reader;
using Microsoft.Extensions.Primitives;
namespace logservice;

class Watcher
{
    private static string logquery;
    private static string queryId;

    static Watcher()
    {
       

        logquery= "Microsoft-Windows-TerminalServices-RemoteConnectionManager/Operational";
        queryId="*[System/EventID=1149]";

    }
    public  void Eventwatcher()
    {
            
            
            
            EventLogQuery query = new EventLogQuery(logquery,PathType.LogName, queryId);

            Console.WriteLine("Esperando clientes");

            using(EventLogWatcher watcher = new EventLogWatcher(query))
            {
                watcher.EventRecordWritten+= new EventHandler<EventRecordWrittenEventArgs>(OnEventRecordWritten);
                watcher.Enabled = true;
            }

    }

         
     public  void OnEventRecordWritten(object? sender, EventRecordWrittenEventArgs e)
     {
        if(e.EventRecord !=null)
        {
            object   user =  e.EventRecord.Properties[0].Value;
            object ip = e.EventRecord.Properties[2].Value;
            object time = e.EventRecord.TimeCreated.Value;
            string message= user.ToString() +" " +ip.ToString()+" " + time.ToString();
            
        }

     }


}