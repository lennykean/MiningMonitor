using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MiningMonitor.Model;
using MiningMonitor.Service;

namespace MiningMonitor.Web.Controllers
{
    [Route("api/[controller]"), Authorize(Policy = "Basic")]
    public class MinersController : Controller
    {
        private readonly IMinerService _minerService;
        private readonly ICollectorService _collectorService;

        public MinersController(IMinerService minerService, ICollectorService collectorService)
        {
            _minerService = minerService;
            _collectorService = collectorService;
        }

        [HttpGet]
        public IEnumerable<Miner> Get()
        {
            return _minerService.GetAll();
        }

        [HttpGet("{id}")]
        public ObjectResult Get(Guid id)
        {
            var miner = _minerService.GetById(id);

            if (miner == null)
                return NotFound(null);

            return Ok(miner);
        }

        [HttpPost]
        public ObjectResult Post([FromBody]Miner miner)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _minerService.Add(miner);

            return Ok(miner);
        }

        [HttpPut]
        public ObjectResult Put([FromBody]Miner miner)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_minerService.Update(miner))
                return NotFound(null);

            return Ok(miner);
        }

        [HttpDelete("{id}")]
        public StatusCodeResult Delete(Guid id)
        {
            if (!_minerService.Delete(id))
                return NotFound();

            return NoContent();
        }

        [HttpPost("collector/{collector}"), Authorize(Policy = "Collector")]
        public StatusCodeResult Post(string collector, [FromBody]Miner miner)
        {
            var succcess = _collectorService.MinerSync(collector, miner);

            if (!succcess)
                return BadRequest();

            return Ok();
        }
    }
}