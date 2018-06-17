using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MiningMonitor.BackgroundWorker.DataCollector;
using MiningMonitor.Model;
using MiningMonitor.Service;

namespace MiningMonitor.Web.Controllers
{
    [Route("api/[controller]")]
    public class SnapshotsController : Controller
    {
        private readonly ISnapshotService _snapshotService;
        private readonly ICollectorService _collectorService;
        private readonly SnapshotDataCollectorSchedule _snapshotDataCollectorSchedule;

        public SnapshotsController(
            ISnapshotService snapshotService,
            ICollectorService collectorService,
            SnapshotDataCollectorSchedule snapshotDataCollectorSchedule)
        {
            _snapshotService = snapshotService;
            _collectorService = collectorService;
            _snapshotDataCollectorSchedule = snapshotDataCollectorSchedule;
        }

        [HttpGet("{minerId}"), Authorize(Policy = "Basic")]
        public IEnumerable<Snapshot> Get(
            [FromRoute]Guid minerId,
            [FromQuery]DateTime? from = null,
            [FromQuery]DateTime? to = null)
        {
            return _snapshotService.GetByMiner(minerId, from, to, _snapshotDataCollectorSchedule.Interval);
        }

        [HttpPost("collector/{collector}/{minerId}"), Authorize(Policy = "Collector")]
        public StatusCodeResult Post(string collector, Guid minerId, [FromBody]Snapshot snapshot)
        {
            var success = _collectorService.SnapshotSync(collector, minerId, snapshot);

            if (!success)
                return BadRequest();

            return Ok();
        }
    }
}