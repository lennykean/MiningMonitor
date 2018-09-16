import { Component, OnInit } from '@angular/core';

import { LoginService } from '../../login.service';
import { SettingsService } from '../../settings.service';

@Component({
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {
    private settings: { [key: string]: string } = {};

    constructor(
        private settingsService: SettingsService,
        private loginService: LoginService) {
    }

    public async ngOnInit() {
        this.settings = await this.settingsService.GetAll();
    }

    public async SaveSettings() {
        this.loginService.ClearCachedSettings();
        await this.settingsService.Update(this.settings);
    }
}
