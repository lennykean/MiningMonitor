using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MiningMonitor.Common;
using MiningMonitor.Model;
using MiningMonitor.Service;

namespace MiningMonitor.Web.Controllers
{
    [Route("api/[controller]")]
    public class SnapshotsController : Controller
    {
        private readonly ISnapshotService _snapshotService;
        private readonly ICollectorService _collectorService;

        public SnapshotsController(
            ISnapshotService snapshotService,
            ICollectorService collectorService)
        {
            _snapshotService = snapshotService;
            _collectorService = collectorService;
        }

        [HttpGet("{minerId}"), Authorize(Policy = "Basic")]
        public Task<IEnumerable<Snapshot>> GetAsync(
            [FromRoute]Guid minerId,
            [FromQuery]DateTime? from = null,
            [FromQuery]DateTime? to = null,
            CancellationToken token = default)
        {
            var end = to?.ToUniversalTime() ?? DateTime.UtcNow;
            var start = from?.ToUniversalTime() ?? end.AddHours(-1);
            var period = new ConcretePeriod(start, end);

            return _snapshotService.GetByMinerFillGapsAsync(minerId, period, TimeSpan.FromMinutes(1), token);
        }

        [HttpPost("collector/{collector}/{minerId}"), Authorize(Policy = "Collector")]
        public async Task<StatusCodeResult> PostAsync(string collector, Guid minerId, [FromBody]Snapshot snapshot, CancellationToken token = default)
        {
            var success = await _collectorService.SnapshotSyncAsync(collector, minerId, snapshot, token);

            if (!success)
                return BadRequest();

            return Ok();
        }
    }
}