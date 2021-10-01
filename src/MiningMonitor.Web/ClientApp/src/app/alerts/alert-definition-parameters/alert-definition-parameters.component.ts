import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AlertParameters } from '../../models/AlertParameters';
import { AlertType } from '../../models/AlertType';

@Component({
  selector: 'mm-alert-definition-parameters',
  templateUrl: './alert-definition-parameters.component.html',
})
export class AlertDefinitionParametersComponent implements OnInit {
  @Input()
  public alertParameters?: AlertParameters;
  @Input()
  public validationErrors: { [key: string]: string[] } = {};
  @Input()
  public alertDefinitionFormGroup: FormGroup;

  public alertType = AlertType;

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit() {
    this.alertDefinitionFormGroup.addControl(
      'parameters',
      this.formBuilder.group({
        alertType: [
          {
            value: this.alertParameters?.alertType,
            disabled: !!this.alertParameters,
          },
          Validators.required,
        ],
        alertMessage: this.alertParameters?.alertMessage,
        durationMinutes: this.alertParameters?.durationMinutes,
      })
    );
    this.alertDefinitionFormGroup.updateValueAndValidity();
  }

  isInvalid(fieldName: string) {
    const field = this.alertDefinitionFormGroup.get(fieldName);
    return (
      (field.touched || field.dirty) &&
      (!field.valid || this.validationErrors[fieldName]?.length)
    );
  }
}
