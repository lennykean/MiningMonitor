export interface Miner {
    id?: string;
    name: string;
    displayName: string;
    address: string;
    port: number;
    password: string;
    collectData: boolean;
    collectorId: string;
}
