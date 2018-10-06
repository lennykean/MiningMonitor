using System;
using System.IO;
using System.Linq;

using LiteDB;

using MiningMonitor.Model;
using MiningMonitor.Service;

using NUnit.Framework;

namespace MiningMonitor.Test.Web.Controllers
{
    [TestFixture]
    public class SnapshotsServiceTest
    {
        private MemoryStream _ms;
        private LiteDatabase _memoryDb;
        private LiteCollection<Snapshot> _collection;
        private SnapshotService _subject;

        [SetUp]
        public void Setup()
        {
            _ms = new MemoryStream();
            _memoryDb = new LiteDatabase(_ms);
            _collection = _memoryDb.GetCollection<Snapshot>();
            _subject = new SnapshotService(_collection);
        }

        [TearDown]
        public void TearDown()
        {
            _ms.Dispose();
        }

        [Test]
        public void GetByMinerId()
        {
            // Arrange
            var now = new DateTime(2018, 6, 1);
            var minerId = new Guid("56f5fb3a-4b59-417c-aae0-ace175bb7c5b");
            _collection.InsertBulk(Enumerable.Range(0, 3)
                .Select(i => new Snapshot { Id = Guid.NewGuid(), SnapshotTime = now, MinerId = minerId})
                .ToList());
            
            // Act
            var result = _subject.GetByMinerFillGaps(minerId, new ConcretePeriod(now, now), TimeSpan.FromMinutes(1));

            // Assert
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void GetAndFillsGaps()
        {
            // Arrange
            var minerId = new Guid("56f5fb3a-4b59-417c-aae0-ace175bb7c5b");
            var now = new DateTime(2018, 1, 1, 6, 0, 0);
            var from = now - TimeSpan.FromMinutes(9);
            var to = now;
            var snapshots = Enumerable.Range(0, 3)
                .Select(i => new Snapshot { Id = Guid.NewGuid(), SnapshotTime = now - TimeSpan.FromMinutes(i * 2), MinerId = minerId})
                .ToList();
            _collection.InsertBulk(snapshots);
            
            // Act
            var result = _subject.GetByMinerFillGaps(minerId, new ConcretePeriod(from, to), TimeSpan.FromMinutes(1)).ToList();

            // Assert
            Assert.That(result.Select(s => s.SnapshotTime), Is.SupersetOf(snapshots.Select(s => s.SnapshotTime)));
            Assert.That(result, Has.Count.EqualTo(9));
        }
    }
}