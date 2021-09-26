import {
  Component,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { Observable, Subscription, timer } from 'rxjs';

import { GpuDataIndex } from '../../models/GpuDataIndex';
import { Miner } from '../../models/Miner';
import { Snapshot } from '../../models/Snapshot';
import { SnapshotService } from '../snapshot.service';

@Component({
  selector: 'mm-miner-chart',
  templateUrl: './miner-chart.component.html',
  styleUrls: ['./miner-chart.component.scss'],
})
export class MinerChartComponent implements OnChanges, OnDestroy {
  @Input()
  public miner: Miner;
  @Input()
  public live: boolean;
  @Input()
  public timeTravelTo: Date;
  public timerSubscription: Subscription;
  public timer: Observable<number>;
  public snapshot: Snapshot;
  public dataSet: { label: string; data: { x: Date; y: number }[] }[][];

  constructor(private snapshotService: SnapshotService) {}

  public async ngOnChanges() {
    this.timer = timer(0, 15000);
    if (this.timerSubscription) {
      this.timerSubscription.unsubscribe();
    }
    if (this.live) {
      this.timerSubscription = this.timer.subscribe(async () => {
        const snapshots = await this.snapshotService.GetByMiner(this.miner.id);
        this.snapshot = snapshots[snapshots.length - 1];
        this.dataSet = this.TransformSnapshots(snapshots);
      });
    } else {
      const snapshots = await this.snapshotService.GetByMiner(
        this.miner.id,
        null,
        this.timeTravelTo
      );
      this.snapshot = snapshots[snapshots.length - 1];
      this.dataSet = this.TransformSnapshots(snapshots);
    }
  }

  public ngOnDestroy() {
    if (this.timerSubscription) {
      this.timerSubscription.unsubscribe();
    }
  }

  private TransformSnapshots(stats: Snapshot[]) {
    const statsByGpu: { label: string; data: { x: Date; y: number }[] }[][] =
      [];

    let maxGpuCount = 0;
    for (const s of stats) {
      if (
        s.minerStatistics &&
        s.minerStatistics.gpus &&
        s.minerStatistics.gpus.length > maxGpuCount
      ) {
        maxGpuCount = s.minerStatistics.gpus.length;
      }
    }
    for (let snapshotIndex = 0; snapshotIndex < stats.length; snapshotIndex++) {
      for (let gpuIndex = 0; gpuIndex < maxGpuCount; gpuIndex++) {
        if (!statsByGpu[gpuIndex]) {
          statsByGpu[gpuIndex] = [
            { label: 'Hashrate', data: [] },
            { label: 'Temp', data: [] },
            { label: 'Fan Speed', data: [] },
          ];
        }
        const snapshotTime = new Date(stats[snapshotIndex].snapshotTime);
        const gpuStats = stats[snapshotIndex].minerStatistics
          ? stats[snapshotIndex].minerStatistics.gpus[gpuIndex]
          : null;

        statsByGpu[gpuIndex][GpuDataIndex.HashRate].data[snapshotIndex] = {
          x: snapshotTime,
          y: gpuStats && gpuStats.ethereumHashrate / 1000,
        };
        statsByGpu[gpuIndex][GpuDataIndex.Temp].data[snapshotIndex] = {
          x: snapshotTime,
          y: gpuStats && gpuStats.temperature,
        };
        statsByGpu[gpuIndex][GpuDataIndex.FanSpeed].data[snapshotIndex] = {
          x: snapshotTime,
          y: gpuStats && gpuStats.fanSpeed,
        };
      }
    }
    return statsByGpu;
  }
}
