import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

import { LoginService } from '../../login.service';
import { SettingsService } from '../settings.service';

@Component({
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss'],
})
export class SettingsComponent implements OnInit {
  public settings: { [key: string]: string };
  public validationErrors: { [key: string]: string[] } = {};

  constructor(
    private settingsService: SettingsService,
    private loginService: LoginService
  ) {}

  public async ngOnInit() {
    this.settings = await this.settingsService.GetAll();
  }

  public async SaveSettings() {
    try {
      this.loginService.ClearCachedSettings();
      this.settings = await this.settingsService.Update(this.settings);
      this.validationErrors = {};
    } catch (error) {
      if (error instanceof HttpErrorResponse && error.status === 400) {
        this.validationErrors = error.error;
      }
    }
  }

  enableSecurityCheckChanged(event: Event) {
    this.settings.enableSecurity = (
      event.target as HTMLInputElement
    ).checked.toString();
  }

  enablePurgeCheckChanged(event: Event) {
    this.settings.enablePurge = (
      event.target as HTMLInputElement
    ).checked.toString();
  }

  isDataCollectorCheckChanged(event: Event) {
    this.settings.isDataCollector = (
      event.target as HTMLInputElement
    ).checked.toString();
  }
}
