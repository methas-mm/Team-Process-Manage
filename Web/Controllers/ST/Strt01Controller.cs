using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Features;
using Application.Features.ST.STRT01;
using Domain.Entities.ST;

namespace Web.Controllers.ST
{
    public class Strt01Controller : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<PageDto>> Get([FromQuery] List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
        [HttpGet("masterdependency")]
        public async Task<IActionResult> Get([FromQuery] MasterDependency.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Create.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpGet("detail")]
        public async Task<ActionResult<StCompany>> Get([FromQuery] Detail.Query model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Edit.Command model)
        {
            return Ok(await Mediator.Send(model));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string CompanyCode, uint rowVersion)
        {
            await Mediator.Send(new Delete.Command { CompanyCode = CompanyCode, RowVersion = rowVersion });
            return NoContent();
        }

    }
}
