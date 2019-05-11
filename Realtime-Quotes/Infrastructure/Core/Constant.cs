using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RealtimeQuotes.Infrastructure.Core
{
    public static class Constant
    {
        static JsonSerializerSettings jss = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatString = "yyyy-MM-ddThh:mm:ssZ",
            DateParseHandling = Newtonsoft.Json.DateParseHandling.None,
            ContractResolver = new DefaultContractResolver() { NamingStrategy = null },

        };
        static JsonSerializer ser = new JsonSerializer()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatString = "yyyy-MM-ddThh:mm:ssZ",
            DateParseHandling = Newtonsoft.Json.DateParseHandling.None,
            ContractResolver = new DefaultContractResolver() { NamingStrategy = null },
        };
        public static JsonSerializerSettings JsonSerializerSetting
        {
            get
            {
                return jss;
            }
        }
        public static JsonSerializer JsonSerializer
        {
            get
            {
                return ser;
            }
        }
    }
}