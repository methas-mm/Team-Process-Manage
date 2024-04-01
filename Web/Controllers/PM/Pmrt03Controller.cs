using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Features.PM.PMRT03;
using Domain.Entities.PM;

namespace Web.Controllers.PM
{
    [AllowAnonymous]

    public class Pmrt03Controller : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] List.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("detail")]
        public async Task<ActionResult<PmWorkcodeGroup>> Get([FromQuery]Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Create.Command model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Edit.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string workcodeGroupCode, uint rowVersion)
        {
            await Mediator.Send(new Delete.Command { WorkcodeGroupCode = workcodeGroupCode, RowVersion = rowVersion });
            return NoContent();
        }
    }
}
