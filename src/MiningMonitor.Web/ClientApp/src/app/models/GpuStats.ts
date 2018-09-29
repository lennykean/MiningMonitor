import { GpuMode } from './GpuMode';

export interface GpuStats {
    mode: GpuMode;
    ethereumHashrate: number;
    decredHashrate: number;
    temperature: number;
    fanSpeed: number;
}
