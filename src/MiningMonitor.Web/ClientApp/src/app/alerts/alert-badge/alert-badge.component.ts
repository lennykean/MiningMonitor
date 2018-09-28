import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { Alert } from '../../../models/Alert';
import { AlertService } from '../../alert.service';

@Component({
    selector: 'miningmonitor-alert-badge',
    templateUrl: './alert-badge.component.html'
})
export class AlertBadgeComponent implements OnInit {
    public alerts: Observable<Alert[]>;

    constructor(
        private alertService: AlertService) {
    }

    ngOnInit() {
        this.alerts = this.alertService.alerts;
    }
}
