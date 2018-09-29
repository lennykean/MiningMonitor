import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { Alert } from '../../../models/Alert';
import { AlertService } from '../alert.service';

@Component({
    templateUrl: './alerts.component.html',
    styleUrls: ['./alerts.component.scss']
})
export class AlertsComponent implements OnInit {
    public alerts: Observable<Alert[]>;

    constructor(
        private alertSerivce: AlertService) {
    }

    public ngOnInit() {
        this.alerts = this.alertSerivce.alerts;
    }

    public async Acknowledge(id: string) {
        await this.alertSerivce.Acknowledge(id);
    }
}
