using Application.Features;
using Application.Features.EA.EART03;
using Domain.Entities.EA;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.EA
{
    public class Eart03Controller : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<PageDto>> Get([FromQuery] List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
        [HttpGet("detail")]
        public async Task<ActionResult<EaCompetitionForm>> Get([FromQuery] Detail.Query model)
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
        //-----------------ModalList------
        [HttpPost("createCompetition")]
        public async Task<ActionResult> Post([FromBody] CUDCompetition.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("delete")]
        public async Task<ActionResult> Get([FromQuery] Delete.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
    }
}
