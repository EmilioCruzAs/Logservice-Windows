using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager;


    public class TelegramServiceOptions
    {
        public string? Token { get; set; }
        public string? ChatId { get; set; }
    }
    public class AppSettings
    {
        public TelegramServiceOptions? TelegramServiceOptions { get; set; }
       
    }

