using Microsoft.AspNetCore.Mvc;
using RealtimeQuotes.Infrastructure.Models;
using RealtimeQuotes.Infrastructure.Services;
using RealtimeQuotes.Infrastructure.Services.Abstraction;
using System;

namespace Realtime_Quotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteRequestController : ControllerBase
    {
        private readonly IBackgroundTaskQueue Queue;
        private readonly IQuoteService quoteService;
        string[] supportServices = new string[] {
                "https://weather-fa.azurewebsites.net/api/GoogleWeather",
                "https://weather-fa.azurewebsites.net/api/OpenWeather",
                "https://weather-fa.azurewebsites.net/api/WorldWeather",
                "https://weather-fa.azurewebsites.net/api/YahooWeather"
            };
        public QuoteRequestController(IBackgroundTaskQueue taskQueue, IQuoteService quoteService)
        {
            this.Queue = taskQueue;
            this.quoteService = quoteService;
        } 
        [HttpPost]
        public IActionResult PostAsync([FromBody]SearchRequest searchRequest, [FromQuery]string roomId)
        {
            for (int i = 0; i < supportServices.Length; i++)
            {
                Queue.QueueBackgroundWorkItem(async arg =>
                {
                    var result = await quoteService.QuoteRequestForCity(arg);
                    return result;
                }, new QuoteRequest { TaskId = roomId, CityId = searchRequest.PickupLocationCode, Url = supportServices[i] });
            }
            Queue.QueueRelease();
            return Ok(new { TaskId = roomId, City = searchRequest.PickupLocationCode });
        }
    }
}