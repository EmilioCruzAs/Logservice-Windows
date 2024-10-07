using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager;

    internal class Firewallwatcher
    {
        private string logquery = "Microsoft-Windows-Windows Firewall With Advanced Security/Firewall";
        private string queryId = "*[System/EventID=2052]";
        private readonly ITelegramService? botMessage; 

        

    }

