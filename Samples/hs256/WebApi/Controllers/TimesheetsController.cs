using System;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class Message
    {
        public DateTime Date { get; set; }
        public string Subject { get; set; }
    }

    [RoutePrefix("api/messages")]
    public class MessagesController : ApiController
    {
        [ScopeAuthorize("read:messages")]
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Json(new Message[]
            {
                new Message
                {
                    Date = DateTime.Now,
                    Subject = "Confirm Newsletter subscription"
                },
                new Message
                {
                    Date = DateTime.Now.AddDays(-1),
                    Subject = "Annual increase"
                }
            });
        }

        [ScopeAuthorize("create:messages")]
        [Route("")]
        [HttpPost]
        public IHttpActionResult Create(Message message)
        {
            return Created("http://localhost:5000/api/messages/1", message);
        }
    }
}