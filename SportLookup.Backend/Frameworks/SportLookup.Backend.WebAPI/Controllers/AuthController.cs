using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportLookup.Backend.WebAPI.Controllers
{

    [ApiController, AllowAnonymous]
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("api/v2/[controller]")]
    public class AuthController : Controller
    {
        [HttpGet("register")]
        public ActionResult Register() { throw new NotImplementedException(); }
    }
}
