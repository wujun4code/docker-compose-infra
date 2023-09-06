using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterCoding.MessagePubSubCenter.Contracts.Request
{
    public class LogInRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
