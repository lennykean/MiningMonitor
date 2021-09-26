import { GpuStats } from './GpuStats';
import { PoolStats } from './PoolStats';

export interface MinerStatistics {
  minerVersion: string;
  uptime: string;
  ethereum: PoolStats;
  decred: PoolStats;
  gpus: GpuStats[];
}
