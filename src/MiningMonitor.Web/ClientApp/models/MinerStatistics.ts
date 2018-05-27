import { GpuStats, PoolStats } from '.';

export interface MinerStatistics {
    minerVersion: string;
    uptime: string;
    ethereum: PoolStats;
    decred: PoolStats;
    gpus: GpuStats[];
}
