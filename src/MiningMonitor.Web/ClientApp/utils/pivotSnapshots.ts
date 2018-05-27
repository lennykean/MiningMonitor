import { GpuDataPoint, Snapshot } from '../models';

export function pivotSnapshots(stats: Snapshot[]) {
    const statsByGpu: GpuDataPoint[][] = [];

    let maxGpuCount = 0;
    for (const s of stats) {
        if (s.minerStatistics && s.minerStatistics.gpus && s.minerStatistics.gpus.length > maxGpuCount) {
            maxGpuCount = s.minerStatistics.gpus.length;
        }
    }
    for (let snapshotIndex = 0; snapshotIndex < stats.length; snapshotIndex++) {
        for (let gpuIndex = 0; gpuIndex < maxGpuCount; gpuIndex++) {
            if (!statsByGpu[gpuIndex]) {
                statsByGpu[gpuIndex] = [];
            }
            const gpuStats = stats[snapshotIndex].minerStatistics ?
                stats[snapshotIndex].minerStatistics.gpus[gpuIndex] :
                null;

            statsByGpu[gpuIndex][snapshotIndex] = {
                snapshotTime: new Date(stats[snapshotIndex].snapshotTime).getTime(),
                temperature: gpuStats && gpuStats.temperature,
                fanSpeed: gpuStats && gpuStats.fanSpeed,
                hashRate: gpuStats && gpuStats.ethereumHashrate / 1000,
            };
        }
    }
    return statsByGpu;
}
