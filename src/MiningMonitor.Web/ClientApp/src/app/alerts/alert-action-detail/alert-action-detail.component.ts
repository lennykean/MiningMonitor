import {
  Component,
  EventEmitter,
  Input,
  OnChanges,
  Output,
} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { DisableGpuAlertActionDefinition } from 'src/app/models/DisableGpuAlertActionDefinition';
import { WebHookAlertActionDefinition } from 'src/app/models/WebHookAlertActionDefinition';

import { AlertActionDefinition } from '../../models/AlertActionDefinition';
import { AlertActionType } from '../../models/AlertActionType';

@Component({
  selector: 'mm-alert-action-detail',
  templateUrl: './alert-action-detail.component.html',
})
export class AlertActionDetailComponent implements OnChanges {
  @Input()
  action?: Partial<AlertActionDefinition>;
  @Output()
  done = new EventEmitter<AlertActionDefinition>();

  actionFormGroup: FormGroup;

  actionTypes = AlertActionType;

  constructor(private formBuilder: FormBuilder) {}

  ngOnChanges() {
    this.actionFormGroup = this.formBuilder.group({
      displayName: this.action?.displayName,
      type: [this.action?.type, Validators.required],
      name: this.action?.name,
    });
    this.changeForm(this.action?.type);
  }

  isInvalid(fieldName: string) {
    const field = this.actionFormGroup.get(fieldName);
    return (
      (field.touched || field.dirty) &&
      (!field.valid || this.actionFormGroup[fieldName]?.length)
    );
  }

  changeForm(type: AlertActionType) {
    this.actionFormGroup.removeControl('disableAffected');
    this.actionFormGroup.removeControl('disableAll');
    this.actionFormGroup.removeControl('url');
    this.actionFormGroup.removeControl('body');

    if (type === AlertActionType.DisableGpu) {
      const action = this.action as DisableGpuAlertActionDefinition;
      this.actionFormGroup.addControl(
        'disableAffected',
        this.formBuilder.control(action.disableAffected)
      );
      this.actionFormGroup.addControl(
        'disableAll',
        this.formBuilder.control(action.disableAll)
      );
    }
    if (type === AlertActionType.WebHook) {
      const action = this.action as WebHookAlertActionDefinition;
      this.actionFormGroup.addControl(
        'url',
        this.formBuilder.control(action.url, {
          validators: this.validUrl,
        })
      );
      this.actionFormGroup.addControl(
        'body',
        this.formBuilder.control(action.body)
      );
    }
  }

  close() {
    this.done.emit({ ...this.action, ...this.actionFormGroup.value });
  }

  private validUrl(control: AbstractControl) {
    try {
      new URL(control.value);
    } catch {
      return { validUrl: false };
    }
  }
}
