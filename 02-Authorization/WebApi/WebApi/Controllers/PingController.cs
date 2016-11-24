using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class PingController : ApiController
    {
        [Route("ping")]
        [HttpGet]
        public IHttpActionResult Ping()
        {
            return Ok(new
                {
                    Message = "All good. You don't need to be authenticated to call this."
                }
            );
        }

        [Authorize]
        [Route("claims")]
        [HttpGet]
        public object Claims()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            return claimsIdentity.Claims.Select(c =>
            new
            {
                Type = c.Type,
                Value = c.Value
            });
        }

        [Authorize]
        [HttpGet]
        [Route("ping/secure")]
        public IHttpActionResult PingSecured()
        {
            return Ok(new
            {
                Message = "All good. You only get this message if you are authenticated."
            }
            );
        }
    }
}