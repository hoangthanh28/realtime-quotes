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
        
        public QuoteRequestController(IBackgroundTaskQueue taskQueue, IQuoteService quoteService)
        {
            this.Queue = taskQueue;
            this.quoteService = quoteService;
        } 
        public IActionResult GetQuoteForCityAsync([FromQuery]string city)
        {
            var supportServices = new string[] {
                "https://weather-fa.azurewebsites.net/api/GoogleWeather",
                "https://weather-fa.azurewebsites.net/api/OpenWeather",
                "https://weather-fa.azurewebsites.net/api/WorldWeather",
                "https://weather-fa.azurewebsites.net/api/YahooWeather"
            };
            var taskId = Guid.NewGuid();
            for (int i = 0; i < supportServices.Length; i++)
            {
                Queue.QueueBackgroundWorkItem(async arg =>
                {
                    var result = await quoteService.QuoteRequestForCity(arg);
                    return result;
                }, new QuoteRequest { TaskId = taskId, CityId = city, Url = supportServices[i] });
            }
            Queue.QueueRelease.Invoke(this, EventArgs.Empty);
            return Ok(new { TaskId = taskId, City = city });
        }
    }
}