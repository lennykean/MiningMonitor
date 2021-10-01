import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import { MinerService } from '../../miner/miner.service';
import { AlertDefinition } from '../../models/AlertDefinition';
import { AlertSeverity } from '../../models/AlertSeverity';
import { Miner } from '../../models/Miner';

@Component({
  selector: 'mm-alert-definition-form',
  templateUrl: './alert-definition-form.component.html',
  styleUrls: ['./alert-definition-form.component.scss'],
})
export class AlertDefinitionFormComponent implements OnInit {
  @Input()
  alertDefinition?: AlertDefinition;
  @Input()
  validationErrors: { [key: string]: string[] } = {};
  @Output()
  save = new EventEmitter<AlertDefinition>();

  miners: Observable<Miner[]>;
  alertSeverity = AlertSeverity;
  alertDefinitionFormGroup: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private minerService: MinerService
  ) {}

  ngOnInit() {
    this.miners = this.minerService.miners;

    this.alertDefinitionFormGroup = this.formBuilder.group({
      displayName: this.alertDefinition?.displayName,
      minerId: [this.alertDefinition?.minerId, Validators.required],
      severity: [this.alertDefinition?.severity, Validators.required],
      enabled: this.alertDefinition?.enabled ?? true,
    });
  }

  isInvalid(fieldName: string) {
    const field = this.alertDefinitionFormGroup.get(fieldName);
    return (
      (field.touched || field.dirty) &&
      (!field.valid || this.validationErrors[fieldName]?.length)
    );
  }

  submit() {
    this.save.emit({
      ...this.alertDefinition,
      ...this.alertDefinitionFormGroup.getRawValue(),
    });
  }
}
