import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { LoginService } from '../../login.service';
import { SettingsService } from '../settings.service';

@Component({
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss'],
})
export class SettingsComponent implements OnInit {
  settings: { [key: string]: string };
  validationErrors: { [key: string]: string[] } = {};
  settingsForm: FormGroup;

  constructor(
    private settingsService: SettingsService,
    private loginService: LoginService,
    private formBuilder: FormBuilder
  ) {}

  async ngOnInit() {
    this.settingsForm = this.formBuilder.group({
      enableSecurity: null,
      enablePurge: null,
      purgeAgeMinutes: null,
      isDataCollector: null,
      serverUrl: null,
      name: null,
    });

    this.initializeValidators();

    this.settings = await this.settingsService.GetAll();
    this.settingsForm.patchValue({
      enableSecurity: this.settings.enableSecurity === 'true',
      enablePurge: this.settings.enablePurge === 'true',
      isDataCollector: this.settings.isDataCollector === 'true',
      purgeAgeMinutes: this.settings.purgeAgeMinutes,
      serverUrl: this.settings.serverUrl,
      name: this.settings.name,
    });
  }

  isInvalid(fieldName: string) {
    const field = this.settingsForm.get(fieldName);
    return (field.touched || field.dirty) && !field.valid;
  }

  async saveSettings() {
    try {
      this.loginService.ClearCachedSettings();
      this.settings = await this.settingsService.Update({
        ...this.settings,
        ...this.settingsForm.value,
      });
      this.validationErrors = {};
    } catch (error) {
      if (error instanceof HttpErrorResponse && error.status === 400) {
        this.validationErrors = error.error;
      }
    }
  }

  private initializeValidators() {
    const enablePurge = this.settingsForm.get('enablePurge');
    enablePurge.valueChanges.subscribe((value) => {
      const purgeAgeMinutes = this.settingsForm.get('purgeAgeMinutes');
      if (value) {
        purgeAgeMinutes.addValidators(Validators.required);
      } else {
        purgeAgeMinutes.clearValidators();
      }
      purgeAgeMinutes.updateValueAndValidity();
    });

    const isDataCollector = this.settingsForm.get('isDataCollector');
    isDataCollector.valueChanges.subscribe((value) => {
      const serverUrl = this.settingsForm.get('serverUrl');
      const name = this.settingsForm.get('name');
      if (value) {
        serverUrl.addValidators(Validators.required);
        name.addValidators(Validators.required);
      } else {
        serverUrl.clearValidators();
        name.clearValidators();
      }
      serverUrl.updateValueAndValidity();
      name.updateValueAndValidity();
    });
  }
}
