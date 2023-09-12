using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterCoding.MessagePubSubCenter.Contracts.Request
{
    public class GetEntryRequest
    {
        public int Id { get; set; }
        public string EntryName { get; set; }
    }
}
