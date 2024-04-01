using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features;
using Application.Features.ST.STRT05;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.ST
{
    public class Strt05Controller : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<PageDto>> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("master")]
        public async Task<ActionResult<PageDto>> Get([FromQuery]Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("detail")]
        public async Task<IActionResult> Get([FromQuery] Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        public async Task<ActionResult<long>> Post([FromBody] Create.Command model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPut]
        public async Task<ActionResult<long>> Put([FromBody] Edit.Command model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Delete.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }
    }
}