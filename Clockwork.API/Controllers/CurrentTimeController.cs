using System;
using Microsoft.AspNetCore.Mvc;
using Clockwork.API.Models;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Clockwork.API.Controllers
{
    
    public class CurrentTimeController : Controller
    {

        // GET api/currenttime/get
        [HttpGet]
        [Route("api/[controller]/get")]
        public IActionResult Get()
        {
            var utcTime = DateTime.UtcNow;
            var serverTime = DateTime.Now;
            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
            
            var returnVal = new CurrentTimeQuery
            {
                UTCTime = utcTime,
                ClientIp = ip,
                Time = serverTime
            };

            using (var db = new ClockworkContext())
            {
                db.CurrentTimeQueries.Add(returnVal);
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);

                Console.WriteLine();
                foreach (var CurrentTimeQuery in db.CurrentTimeQueries)
                {
                    Console.WriteLine(" - {0}", CurrentTimeQuery.UTCTime);
                }
            }

            return Ok(JsonConvert.SerializeObject(returnVal));
        }

        // GET api/currenttime/previous
        [HttpGet]
        [Route("api/[controller]/previous")]
        public IActionResult Previous()
        {
           
            var returnVal = new List<CurrentTimeQuery>();

            using (var db = new ClockworkContext())
            {
                returnVal.AddRange(db.CurrentTimeQueries.ToList());
            }

            return Ok(returnVal);
        }
    }
}
