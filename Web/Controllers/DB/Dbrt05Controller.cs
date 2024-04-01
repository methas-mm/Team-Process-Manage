
using System.Threading.Tasks;
using Application.Features;
using Application.Features.DB.DBRT05;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.DB
{
    [AllowAnonymous]
    public class Dbrt05Controller : BaseController
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
        [HttpGet("detail")]
        public async Task<ActionResult> Get([FromQuery] Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("master")]
        public async Task<ActionResult<Master.MasterList>> Get([FromQuery] Master.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int TeamId, uint rowVersion)
        {
            await Mediator.Send(new Delete.Command { teamId = TeamId, RowVersion = rowVersion });
            return NoContent();
        }

    }
}

