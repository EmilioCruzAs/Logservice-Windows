
using System;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics;
using System.ServiceProcess;
namespace  logservice;

public class Program
{
    
 static void Main(string[] args)
    {

       if(Environment.UserInteractive)
       {

          var service = new Myservice();
          service._is_start();  
          Console.WriteLine("Presione una tecla para salir");
          Console.ReadLine();
          service._is_stop();
       }
       else
       {
          ServiceBase.Run(new Myservice());
       }
    }
}


public class Myservice: ServiceBase
{ 
       // private   Timer timer1 ;
       // private EventLog eventLog;
        public Myservice()
        {

            ServiceName = "LogManagerService";
            //timer1 = new Timer(OnWork,null, Timeout.Infinite, Timeout.Infinite);
            //eventLog = new EventLog();
        }

        public void _is_start()
        {
            OnStart(null);
        }
        public void _is_stop()
        {
            OnStop();
        }      


        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            //eventLog.WriteEntry("In Onstart");
           _Watcher watcher = new _Watcher();
            watcher.Eventwatcher();
        }

       protected override void OnStop()
       {
         Environment.Exit(0);
       }
        public void OnWork(Object? sender)
        {
            Console.WriteLine("Realizando una tarea");
            
        }


}