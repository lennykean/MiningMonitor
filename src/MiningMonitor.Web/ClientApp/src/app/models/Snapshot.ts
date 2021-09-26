import { MinerStatistics } from './MinerStatistics';

export interface Snapshot {
  id: string;
  minerId: string;
  snapshotTime: string;
  retrievalElapsedTime: string;
  minerStatistics: MinerStatistics;
}
