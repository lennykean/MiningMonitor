import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { timer, Observable, Subscription } from 'rxjs';

import { GpuDataIndex } from '../../../models/GpuDataIndex';
import { Miner } from '../../../models/Miner';
import { Snapshot } from '../../../models/Snapshot';
import { MinerService } from '../miner.service';
import { SnapshotService } from '../snapshot.service';

@Component({
    templateUrl: './monitor.component.html',
    styleUrls: ['./monitor.component.scss']
})
export class MonitorComponent implements OnInit, OnDestroy {
    public timer: Observable<number>;
    public subscription: Subscription;
    public snapshot: Snapshot;
    public miner: Miner;
    public dataSet: { label: string, data: { x: Date, y: number }[]; }[][];

    constructor(
        private route: ActivatedRoute,
        private snapshotService: SnapshotService,
        private minerService: MinerService) {
    }

    public ngOnInit() {
        this.timer = timer(0, 15000);
        this.route.paramMap.subscribe(async paramMap => {
            if (this.subscription) {
                this.subscription.unsubscribe();
            }
            const id = paramMap.get('id');

            this.miner = await this.minerService.Get(id);

            this.subscription = this.timer.subscribe(async () => {
                const snapshots = await this.snapshotService.GetByMiner(id);
                this.snapshot = snapshots[snapshots.length - 1];
                this.dataSet = this.TransformSnapshots(snapshots);
            });
        });
    }

    public ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }

    private TransformSnapshots(stats: Snapshot[]) {
        const statsByGpu: { label: string, data: { x: Date, y: number }[] }[][] = [];

        let maxGpuCount = 0;
        for (const s of stats) {
            if (s.minerStatistics && s.minerStatistics.gpus && s.minerStatistics.gpus.length > maxGpuCount) {
                maxGpuCount = s.minerStatistics.gpus.length;
            }
        }
        for (let snapshotIndex = 0; snapshotIndex < stats.length; snapshotIndex++) {
            for (let gpuIndex = 0; gpuIndex < maxGpuCount; gpuIndex++) {
                if (!statsByGpu[gpuIndex]) {
                    statsByGpu[gpuIndex] = [
                        { label: 'Hashrate', data: [] },
                        { label: 'Temp', data: [] },
                        { label: 'Fan Speed', data: [] }
                    ];
                }
                const snapshotTime = new Date(stats[snapshotIndex].snapshotTime);
                const gpuStats = stats[snapshotIndex].minerStatistics ?
                    stats[snapshotIndex].minerStatistics.gpus[gpuIndex] :
                    null;

                statsByGpu[gpuIndex][GpuDataIndex.HashRate].data[snapshotIndex] = {
                    x: snapshotTime,
                    y: gpuStats && gpuStats.ethereumHashrate / 1000
                };
                statsByGpu[gpuIndex][GpuDataIndex.Temp].data[snapshotIndex] = {
                    x: snapshotTime,
                    y: gpuStats && gpuStats.temperature
                };
                statsByGpu[gpuIndex][GpuDataIndex.FanSpeed].data[snapshotIndex] = {
                    x: snapshotTime,
                    y: gpuStats && gpuStats.fanSpeed
                };
            }
        }
        return statsByGpu;
    }
}
