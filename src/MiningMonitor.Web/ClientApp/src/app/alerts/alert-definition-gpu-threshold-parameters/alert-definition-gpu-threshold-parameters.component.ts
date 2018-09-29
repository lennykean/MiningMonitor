import { Component, Input } from '@angular/core';

import { GpuThresholdParameters } from '../../../models/GpuThresholdParameters';
import { Metric } from '../../../models/Metric';

@Component({
    selector: 'miningmonitor-alert-definition-gpu-threshold-parameters',
    templateUrl: './alert-definition-gpu-threshold-parameters.component.html'
})
export class AlertDefinitionGpuThresholdParametersComponent {
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
