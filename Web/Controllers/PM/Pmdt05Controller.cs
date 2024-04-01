using Application.Features;
using Application.Features.PM.PMDT05;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Controllers.PM
{
    public class Pmdt05Controller : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<PageDto>> Get([FromQuery] List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
        [HttpGet("detail")]
        public async Task<IActionResult> Get([FromQuery] Detail.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
        [HttpGet("master")]
        public async Task<IActionResult> Get([FromQuery] Master.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
        [HttpGet("masterdependency")]
        public async Task<IActionResult> Get([FromQuery] MasterDepedency.Query model)
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
        [HttpGet("moduleDetailPlaneAssign")]
        public async Task<ActionResult<PageDto>> Get([FromQuery] MasterPlanAssignDetail.Query query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("moduleDetailPlan")]
        public async Task<ActionResult<PageDto>> Get([FromQuery] ModuleDetailPlanList.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
