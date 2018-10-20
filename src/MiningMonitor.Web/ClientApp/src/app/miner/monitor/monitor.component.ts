import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription, timer } from 'rxjs';

import { GpuDataIndex } from '../../models/GpuDataIndex';
import { Miner } from '../../models/Miner';
import { Snapshot } from '../../models/Snapshot';
import { MinerService } from '../miner.service';
import { SnapshotService } from '../snapshot.service';

@Component({
    templateUrl: './monitor.component.html',
    styleUrls: ['./monitor.component.scss']
})
export class MonitorComponent implements OnInit, OnDestroy {
    public timer: Observable<number>;
    public timerSubscription: Subscription;
    public snapshot: Snapshot;
    public miner: Miner;
    public dataSet: { label: string, data: { x: Date, y: number }[]; }[][];
    public live: boolean;
    public timeTravelTo: Date;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private snapshotService: SnapshotService,
        private minerService: MinerService) {
    }

    public ngOnInit() {
        this.timer = timer(0, 15000);
        this.route.paramMap.subscribe(async paramMap => {
            const id = paramMap.get('id');
            this.route.queryParamMap.subscribe(async queryParamMap => {
                if (this.timerSubscription) {
                    this.timerSubscription.unsubscribe();
                }
                const timeTravel = queryParamMap.get('timeTravel');
                if (timeTravel) {
                    this.timeTravelTo = new Date(timeTravel);
                    this.live = false;
                    const snapshots = await this.snapshotService.GetByMiner(id, null, this.timeTravelTo);
                    this.snapshot = snapshots[snapshots.length - 1];
                    this.dataSet = this.TransformSnapshots(snapshots);
                } else {
                    this.live = true;
                    this.timeTravelTo = new Date();
                    this.timerSubscription = this.timer.subscribe(async () => {
                        const snapshots = await this.snapshotService.GetByMiner(id);
                        this.snapshot = snapshots[snapshots.length - 1];
                        this.dataSet = this.TransformSnapshots(snapshots);
                    });
                }
            });
            this.miner = await this.minerService.Get(id);
        });
    }

    public ngOnDestroy() {
        if (this.timerSubscription) {
            this.timerSubscription.unsubscribe();
        }
    }

    public TimeTravel(to: string) {
        this.router.navigate([], { queryParams: { timeTravel: new Date(to).toISOString() } });
    }

    public GoLive() {
        this.router.navigate([], { queryParams: {} });
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
