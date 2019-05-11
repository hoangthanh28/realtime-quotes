using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RealtimeQuotes.Infrastructure.Models;
using RealtimeQuotes.Infrastructure.Services;
using RealtimeQuotes.Infrastructure.Services.Abstraction;
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
        private readonly IQuoteService pwcQuoteService;
        private IPublisher publisher;
        public QuoteRequestController(IBackgroundTaskQueue taskQueue,
                                    IPublisher publisher,
                                    IQuoteService pwcQuoteService)
        {
            this.Queue = taskQueue;
            this.publisher = publisher;
            this.pwcQuoteService = pwcQuoteService;
        }
        [HttpPost]
        public IActionResult PostAsync([FromBody]JObject searchRequest, [FromQuery]string roomId)
        {
            searchRequest["EventId"] = roomId;
            Queue.QueueBackgroundWorkItem(async arg =>
            {
                await pwcQuoteService.QuoteRequestAsync(searchRequest);
            });
            return Ok();
        }
    }
}