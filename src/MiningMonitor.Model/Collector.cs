using System;

namespace MiningMonitor.Model
{
    public class Collector
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool? Approved { get; set; }
    }
}