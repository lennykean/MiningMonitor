import { Component, OnInit } from '@angular/core';
import { faExclamationTriangle } from "@fortawesome/free-solid-svg-icons";
import { Observable } from 'rxjs';

import { Alert } from '../../models/Alert';
import { AlertSeverity } from '../../models/AlertSeverity';
import { AlertService } from '../alert.service';

@Component({
    templateUrl: './alerts.component.html',
    styleUrls: ['./alerts.component.scss']
})
export class AlertsComponent implements OnInit {
    public readonly faExclamationTriangle = faExclamationTriangle;
    
    public alerts: Observable<Alert[]>;

    public alertSeverity = AlertSeverity;

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
