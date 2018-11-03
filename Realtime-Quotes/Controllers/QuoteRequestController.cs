using Microsoft.AspNetCore.Mvc;
using RealtimeQuotes.Infrastructure.Models;
using RealtimeQuotes.Infrastructure.Services;
using RealtimeQuotes.Infrastructure.Services.Abstraction;
using System;
using System.Collections.Generic;

namespace Realtime_Quotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteRequestController : ControllerBase
    {
        private readonly IBackgroundTaskQueue Queue;
        private readonly IList<IQuoteService> supportServices;
        public QuoteRequestController(IBackgroundTaskQueue taskQueue, IList<IQuoteService> supportServices)
        {
            this.Queue = taskQueue;
            this.supportServices = supportServices;
        } 
        [HttpPost]
        public IActionResult PostAsync([FromBody]SearchRequest searchRequest, [FromQuery]string roomId)
        {
            foreach (var service in supportServices)
            {
                Queue.QueueBackgroundWorkItem(async arg =>
                {
                    return await service.QuoteRequestForCity(arg);
                }, new QuoteRequest { RoomId = roomId, CityId = searchRequest.PickupLocationCode});
            }
            Queue.QueueRelease();
            return Ok(new { TaskId = roomId, City = searchRequest.PickupLocationCode });
        }
    }
}