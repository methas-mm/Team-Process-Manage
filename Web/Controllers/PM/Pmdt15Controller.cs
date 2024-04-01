using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.PM.PMDT15;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.PM
{
    [AllowAnonymous]
    public class Pmdt15Controller : BaseController
    {
        [HttpGet("master")]
        public async Task<IActionResult> Get([FromQuery] Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("masterdependency")]
        public async Task<IActionResult> Get([FromQuery] MasterDependency.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("detail")]
        public async Task<IActionResult> Get([FromQuery] Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        public async Task<IActionResult> Get([FromQuery] List.Query model)
        {
            return Ok(await Mediator.Send(model));
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
        [HttpDelete]
        public async Task<ActionResult<long>> Delete([FromQuery] Delete.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
    }
}