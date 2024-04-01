using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.DB.DBRT10;
using Application.Features;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers.DB
{
  
    public class Dbrt10Controller : BaseController
    {
       [HttpGet("master")]
        public async Task<ActionResult<Master.MasterData>> Get([FromQuery] Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

        [HttpGet("masterdependency")]
        public async Task<IActionResult> Get([FromQuery] MasterDependency.Query model)
        {
            return Ok(await Mediator.Send(model));
        }

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
        [HttpDelete]
        public async Task<IActionResult> Delete(string EmployeeCode, uint rowVersion)
        {
            await Mediator.Send(new Delete.Command { EmployeeCode = EmployeeCode, RowVersion = rowVersion });
            return NoContent();
        }
        [HttpGet("detail")]
        public async Task<ActionResult<Master.MasterData>> Get([FromQuery] Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPut]
        public async Task<ActionResult> Put([FromBody] Edit.Command model)
        {
            return Ok(await Mediator.Send(model));
        }


    }
}
