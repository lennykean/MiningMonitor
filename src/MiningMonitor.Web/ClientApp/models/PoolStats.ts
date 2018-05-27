export interface PoolStats {
    pool: string;
    hashrate: number;
    shares: number;
    rejectedShares: number;
    invalidShares: number;
    poolSwitches: number;
}
