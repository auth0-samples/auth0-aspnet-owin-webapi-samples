using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace WebApi.Controllers
{
    [RoutePrefix("api")]
    public class ApiController : System.Web.Http.ApiController
    {
        [Route("public")]
        [HttpGet]
        public IHttpActionResult Public()
        {
            return Json(new
            {
                Message = "Hello from a public endpoint! You don't need to be authenticated to see this."
            });
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
    }
}