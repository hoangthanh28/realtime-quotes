using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RealtimeQuotes.Infrastructure.Core;
using RealtimeQuotes.Infrastructure.Models;
using RealtimeQuotes.Infrastructure.Services;
using RealtimeQuotes.Infrastructure.Services.Abstraction;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Realtime_Quotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteRequestController : ControllerBase
    {
        private readonly IBackgroundTaskQueue Queue;
        private readonly IList<IQuoteService> supportServices;
        private IPublisher publisher;
        public QuoteRequestController(IBackgroundTaskQueue taskQueue, IList<IQuoteService> supportServices, IPublisher publisher)
        {
            this.Queue = taskQueue;
            this.supportServices = supportServices;
            this.publisher = publisher;
        }
        [HttpPost]
        public IActionResult PostAsync([FromBody]JObject searchRequest, [FromQuery]string roomId)
        {
            Queue.QueueBackgroundWorkItem(async arg =>
            {
                var services = supportServices.Select(x => x.QuoteRequestAsync(searchRequest)).ToList();
                while (services.Any())
                {
                    var completedTask = await Task.WhenAny(services);
                    services.Remove(completedTask);
                    var result = await completedTask;
                    await publisher.PuslishAsync(roomId, result);
                }
            });
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetCacheAsync([FromQuery]string requestId, [FromServices]ConnectionMultiplexer redisCache)
        {
            var cache = redisCache.GetDatabase();
            var result = await cache.StringGetAsync(requestId);
            if (!string.IsNullOrEmpty(result))
            {
                return Ok(JsonConvert.DeserializeObject<JObject>(result, Constant.JsonSerializerSetting));
            }
            else
            {
                return BadRequest($"RequestId ({requestId}) was not found.");
            }
        }
    }
}