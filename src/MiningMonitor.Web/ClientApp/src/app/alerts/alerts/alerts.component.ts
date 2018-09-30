import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { Alert } from '../../models/Alert';
import { AlertService } from '../alert.service';

@Component({
    templateUrl: './alerts.component.html',
    styleUrls: ['./alerts.component.scss']
})
export class AlertsComponent implements OnInit {
    public alerts: Observable<Alert[]>;

    constructor(
        private alertService: AlertService) {
    }

    public ngOnInit() {
        this.alerts = this.alertService.alerts;
    }

    public async Acknowledge(id: string) {
        await this.alertService.Acknowledge(id);
    }
}
