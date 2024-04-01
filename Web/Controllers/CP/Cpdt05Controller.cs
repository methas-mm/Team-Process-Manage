using Application.Features;
using Application.Features.CP.CPDT05;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace Web.Controllers.CP
{
    [AllowAnonymous]
    public class Cpdt05Controller : BaseController
    {
        public async Task<ActionResult<PageDto>> Get([FromQuery] List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Create.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("master")]
        public async Task<ActionResult<Master.MasterData>> Get([FromQuery] Master.Query model)
        {
            return Ok(await Mediator.Send(model));
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
        [HttpPut("update")]
        public async Task<ActionResult> Put([FromBody] Update.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] Delete.Command model)
        {
            await Mediator.Send(model);
            return NoContent();
        }
    }
}
