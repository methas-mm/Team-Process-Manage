using Application.Features;
using Application.Features.ST.STRT09;
using Domain.Entities.DB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.ST
{
    public class Strt09Controller : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<PageDto>> Get([FromQuery] List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("master")]
        public async Task<ActionResult<Master.MasterData>> Get([FromQuery] Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Create.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("detail")]
        public async Task<ActionResult<DbListItem>> Get([FromQuery] Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Edit.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string Id, string Groupid, uint rowVersion)
        {
            await Mediator.Send(new Delete.Command { Id = Id, Groupid=Groupid, RowVersion = rowVersion });
            return NoContent();
        }

    }
}
