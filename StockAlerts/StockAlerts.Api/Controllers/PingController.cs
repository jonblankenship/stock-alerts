using System;
using Microsoft.AspNetCore.Mvc;

namespace StockAlerts.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/ping
        [HttpGet]
        public ActionResult<string> Get()
        {
            return new OkObjectResult($"StockAlerts API Ping: {DateTime.UtcNow:F}");
        }
    }
}
