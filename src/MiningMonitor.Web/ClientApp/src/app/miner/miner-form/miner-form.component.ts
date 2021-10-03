import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { Miner } from '../../models/Miner';

@Component({
  selector: 'mm-miner-form',
  templateUrl: './miner-form.component.html',
  styleUrls: ['./miner-form.component.scss'],
})
export class MinerFormComponent implements OnInit {
  @Input()
  miner?: Miner;
  @Input()
  validationErrors: { [key: string]: string[] } = {};

  @Output()
  save = new EventEmitter<Miner>();

  minerFormGroup: FormGroup;

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit() {
    this.minerFormGroup = this.formBuilder.group({
      displayName: this.miner?.displayName,
      address: [this.miner?.address, Validators.required],
      port: this.miner?.port,
      collectData: this.miner?.collectData,
    });
    if (this.miner?.collectorId) {
      this.minerFormGroup.disable();
    }
  }

  isInvalid(fieldName: string) {
    const field = this.minerFormGroup.get(fieldName);
    return (
      (field.touched || field.dirty) &&
      (!field.valid || this.validationErrors[fieldName]?.length)
    );
  }

  submit() {
    this.save.emit({ ...this.miner, ...this.minerFormGroup.value });
  }
}
