import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { AlertParameters } from 'src/app/models/AlertParameters';

import { GpuThresholdAlertParameters } from '../../models/GpuThresholdAlertParameters';
import { Metric } from '../../models/Metric';

@Component({
  selector: 'mm-gpu-threshold-parameters',
  templateUrl: './gpu-threshold-parameters.component.html',
})
export class GpuThresholdParametersComponent implements OnInit, OnDestroy {
  @Input()
  public alertParameters?: AlertParameters &
    Partial<GpuThresholdAlertParameters>;
  @Input()
  public alertDefinitionFormGroup: FormGroup;

  public metric = Metric;

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit() {
    const alertParametersGroup = this.alertDefinitionFormGroup.get(
      'parameters'
    ) as FormGroup;

    alertParametersGroup.addControl(
      'metric',
      this.formBuilder.control(
        this.alertParameters?.metric,
        Validators.required
      )
    );
    alertParametersGroup.addControl(
      'minValue',
      this.formBuilder.control(this.alertParameters?.minValue)
    );
    alertParametersGroup.addControl(
      'maxValue',
      this.formBuilder.control(this.alertParameters?.maxValue)
    );
  }

  ngOnDestroy() {
    const alertParametersGroup = this.alertDefinitionFormGroup.get(
      'parameters'
    ) as FormGroup;

    alertParametersGroup.removeControl('metric');
    alertParametersGroup.removeControl('minValue');
    alertParametersGroup.removeControl('maxValue');
  }
}
