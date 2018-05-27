using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        private readonly SnapshotDataCollectorSchedule _snapshotDataCollectorSchedule;

        public SnapshotsController(
            ISnapshotService snapshotService,
            SnapshotDataCollectorSchedule snapshotDataCollectorSchedule)
        {
            _snapshotService = snapshotService;
            _snapshotDataCollectorSchedule = snapshotDataCollectorSchedule;
        }

        [HttpGet("{minerId}"), Authorize(Policy = "Basic")]
        public async Task<IEnumerable<Snapshot>> Get(
            [FromRoute]Guid minerId,
            [FromQuery]DateTime? from = null,
            [FromQuery]DateTime? to = null)
        {
            return await _snapshotService.GetByMinerAsync(minerId, from, to, _snapshotDataCollectorSchedule.Interval);
        }

        [HttpPost("collector/{collector}/{minerId}"), Authorize(Policy = "Collector")]
        public async Task<StatusCodeResult> Post(string collector, Guid minerId, [FromBody]Snapshot snapshot)
        {
            var success = await _snapshotService.CollectorSyncAsync(collector, minerId, snapshot);

            if (!success)
                return BadRequest();

            return Ok();
        }
    }
}