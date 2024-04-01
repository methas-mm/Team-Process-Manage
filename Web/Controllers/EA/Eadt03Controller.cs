using Microsoft.AspNetCore.Mvc;
using Application.Features.EA.EADT03;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.EA
{
    public class Eadt03Controller : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] List.Query model)
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
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("detail")]
        public async Task<ActionResult> Get([FromQuery] Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("master")]
        public async Task<ActionResult> Get([FromQuery] Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("delete")]
        public async Task<ActionResult> Get([FromQuery] Delete.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("form")]
        public async Task<ActionResult> Get([FromQuery] ListForm.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
    }
}
