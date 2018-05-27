using System;

using ClaymoreMiner.RemoteManagement.Models;

using LiteDB;

namespace MiningMonitor.Model
{
    public class Snapshot
    {
        [BsonId(false)]
        public Guid Id { get; set; }
        public Guid MinerId { get; set; }
        public DateTime SnapshotTime { get; set; }
        public TimeSpan RetrievalElapsedTime { get; set; }
        public MinerStatistics MinerStatistics { get; set; }
    }
}