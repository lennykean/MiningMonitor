import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';

import { HashrateAlertParameters } from '../../models/HashrateAlertParameters';

@Component({
  selector: 'mm-hashrate-parameters',
  templateUrl: './hashrate-parameters.component.html',
})
export class HashrateParametersComponent implements OnInit, OnDestroy {
  @Input()
  alertParameters?: HashrateAlertParameters;
  @Input()
  alertDefinitionFormGroup: FormGroup;

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit() {
    const alertParametersGroup = this.alertDefinitionFormGroup.get(
      'parameters'
    ) as FormGroup;

    alertParametersGroup.addControl(
      'minValue',
      this.formBuilder.control(this.alertParameters?.minValue)
    );
  }

  ngOnDestroy() {
    const alertParametersGroup = this.alertDefinitionFormGroup.get(
      'parameters'
    ) as FormGroup;

    alertParametersGroup.removeControl('minValue');
  }
}
