using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features;
using Application.Features.ST.STRT07;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.ST
{
    [AllowAnonymous]
    public class Strt07Controller : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<PageDto>> Get([FromQuery] List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPut]
        public async Task<ActionResult<long>> Put([FromBody] Edit.Command model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("detail")]
        public async Task<ActionResult> Get([FromQuery] Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
    }
}
