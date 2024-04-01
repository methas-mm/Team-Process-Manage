using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features;
using Application.Features.PM.PMRT10;
using Domain.Entities.PM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.PM
{
    [AllowAnonymous]
    public class Pmrt10Controller : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<PageDto>> Get([FromQuery] List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Create.Command model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Edit.Command model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string CustomerCode, uint rowVersion)
        {
            await Mediator.Send(new Delete.Command { CustomerCode = CustomerCode, RowVersion = rowVersion });
            return NoContent();
        }

        [HttpGet("detail")]
        public async Task<ActionResult> Get([FromQuery] Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
    }
}
