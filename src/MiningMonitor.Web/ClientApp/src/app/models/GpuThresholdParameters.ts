import { AlertParameters } from './AlertParameters';
import { Metric } from './Metric';

export interface GpuThresholdParameters extends AlertParameters {
    metric: Metric;
    minValue?: number;
    maxValue?: number;
}
