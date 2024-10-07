﻿
using System;
using System.ServiceProcess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

       
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.Configure<AppSettings>(context.Configuration.GetSection("TelegramServiceOptions"));
                services.AddSingleton<ITelegramService, TelegramService>();
                services.AddSingleton<RdpWatcher>();
                services.AddHostedService<Myremotelog>();
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