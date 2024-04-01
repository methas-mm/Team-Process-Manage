using Application.Features;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.SM.SMDT01;

namespace Web.Controllers.ST
{
    public class Smdt01Controller : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<List.ListData>> Get([FromQuery] List.Query query)
        {
            return Ok(await Mediator.Send(query));
        }
    }
}
