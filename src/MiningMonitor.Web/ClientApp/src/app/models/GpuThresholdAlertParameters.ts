import { AlertParameters } from './AlertParameters';
import { Metric } from './Metric';

export interface GpuThresholdAlertParameters extends AlertParameters {
    metric: Metric;
    minValue?: number;
    maxValue?: number;
}
