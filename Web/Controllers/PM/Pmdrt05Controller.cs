using Application.Features;
using Application.Features.PM.PMRT05;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.PM
{
    [AllowAnonymous]
    public class Pmrt05Controller : BaseController
    {
        public async Task<ActionResult<PageDto>> Get([FromQuery] List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Delete.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

        [HttpGet("master")]
        public async Task<IActionResult> Get([FromQuery] Master.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Create.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Edit.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("detail")]
        public async Task<IActionResult> Get([FromQuery] Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

    }

}
