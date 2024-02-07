using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace NetSchool.Api.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [ApiVersion("1.0")]
        public int Test(int value)
        {
            return value;
        }
    }
}
