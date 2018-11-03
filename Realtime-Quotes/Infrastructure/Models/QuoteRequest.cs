using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealtimeQuotes.Infrastructure.Models
{
    public class QuoteRequest
    {
        public string Url { get; set; }
        public string RoomId { get; set; }
        public string CityId { get; set; }
    }
}
