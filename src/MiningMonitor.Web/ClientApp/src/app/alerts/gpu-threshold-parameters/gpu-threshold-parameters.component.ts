import { Component, Input } from '@angular/core';

import { GpuThresholdParameters } from '../../models/GpuThresholdParameters';
import { Metric } from '../../models/Metric';

@Component({
    selector: 'mm-gpu-threshold-parameters',
    templateUrl: './gpu-threshold-parameters.component.html'
})
export class GpuThresholdParametersComponent {
    @Input()
    public alertParameters: GpuThresholdParameters = {
        alertMessage: null,
        alertType: null,
        metric: null,
        minValue: null,
        maxValue: null,
    };
    public metric = Metric;
}
