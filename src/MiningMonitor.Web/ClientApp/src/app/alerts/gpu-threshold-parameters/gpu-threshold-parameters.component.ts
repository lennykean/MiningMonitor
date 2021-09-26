import { Component, Input } from '@angular/core';

import { GpuThresholdAlertParameters } from '../../models/GpuThresholdAlertParameters';
import { Metric } from '../../models/Metric';

@Component({
  selector: 'mm-gpu-threshold-parameters',
  templateUrl: './gpu-threshold-parameters.component.html',
})
export class GpuThresholdParametersComponent {
  @Input()
  public alertParameters: GpuThresholdAlertParameters = {
    alertType: null,
    metric: null,
  };
  public metric = Metric;
}
