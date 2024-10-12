
using System;
using System.ServiceProcess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace  EventManager;
public class Program
{
     static void Main(string[] args)
     {
        if (Environment.UserInteractive)
        {
            var service = new Myservice();
            service._is_start();  
        }
        else
        {
            ServiceBase.Run(new Myservice());
        }
    }

}


public class Myservice: ServiceBase
{ 
       
        public Myservice()
        {

            ServiceName = "EventManager";
           
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


           Host.CreateDefaultBuilder(args).ConfigureLogging(loggin =>
           {
               loggin.ClearProviders();
               loggin.AddConsole();

           }).
            ConfigureHostConfiguration(Hostconfig =>
           {
               Hostconfig.SetBasePath(Directory.GetCurrentDirectory());
               Hostconfig.AddJsonFile("appsettings.json", optional:false, reloadOnChange: true);
           })
           .ConfigureServices((context, services) =>
           {   

               services.AddSingleton<ITelegramService, TelegramService>();
               services.AddSingleton<RdpWatcher>();
               services.AddSingleton<Firewallwatcher>();
               services.AddHostedService<RemoteaccessManager>();
               services.AddHostedService<FirewallManager>();
               
               
           }).Build().Run();

            //base.OnStart(args);
        
        
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