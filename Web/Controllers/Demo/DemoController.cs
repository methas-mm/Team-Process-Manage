using System;
using System.Threading.Tasks;
using Application.Features.Demo;
using Microsoft.AspNetCore.Mvc;
using Application.Features;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [AllowAnonymous]
    public class DemoController : BaseController
    {
        [HttpGet("master")]
        public async Task<IActionResult> Get([FromQuery]GetMaster.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
    }
}
