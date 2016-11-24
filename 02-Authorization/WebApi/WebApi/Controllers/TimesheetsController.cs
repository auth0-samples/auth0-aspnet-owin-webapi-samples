using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class Timesheet
    {
        public DateTime Date { get; set; }
        public string Employee { get; set; }
        public float Hours { get; set; }
    }

    [RoutePrefix("api/timesheets")]
    public class TimesheetsController : ApiController
    {
        [ScopeAuthorize("read:timesheets")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Json(new Timesheet[]
            {
                new Timesheet
                {
                    Date = DateTime.Now,
                    Employee = "Peter Parker",
                    Hours = 8.5F
                },
                new Timesheet
                {
                    Date = DateTime.Now.AddDays(-1),
                    Employee = "Peter Parker",
                    Hours = 7.5F
                }
            });
        }

        [ScopeAuthorize("create:timesheets")]
        [Route("")]
        [HttpPost]
        public IHttpActionResult Create(Timesheet timeheet)
        {
            return Created("http://localhost:5000/api/timeheets/1", timeheet);
        }
    }
}