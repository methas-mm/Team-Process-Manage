using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Features.ST.STRT03;
using Application.Features;
using static Application.Features.ST.STRT03.Detail;

namespace Web.Controllers.ST
{
    public class Strt03Controller : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<PageDto>> Get([FromQuery]List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("detail")]
        public async Task<ActionResult<StProfileDTO>> Get([FromQuery]Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("master")]
        public async Task<ActionResult<PageDto>> Get([FromQuery]Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody]Create.Command model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]Edit.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string profileCode, uint rowVersion)
        {
            await Mediator.Send(new Delete.Command { ProfileCode = profileCode, RowVersion = rowVersion });
            return NoContent();
        }

        [HttpPost("copy")]
        public async Task<ActionResult> Post([FromBody]Copy.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
    }
}