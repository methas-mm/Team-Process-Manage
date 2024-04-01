using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Features.SU.Menu;
using Domain.Entities.SU;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Controllers.SU
{
    [AllowAnonymous]
    public class MenuController : BaseController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
