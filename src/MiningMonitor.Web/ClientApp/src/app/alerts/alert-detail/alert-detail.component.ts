import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Alert } from '../../models/Alert';
import { AlertActionState } from '../../models/AlertActionState';
import { AlertSeverity } from '../../models/AlertSeverity';
import { AlertService } from '../alert.service';

@Component({
    templateUrl: './alert-detail.component.html'
})
export class AlertDetailComponent implements OnInit {
    public alert: Alert;
    public actionState = AlertActionState;
    public alertSeverity = AlertSeverity;

    constructor(
        private alertService: AlertService,
        private route: ActivatedRoute,
        private router: Router) {
    }

    public ngOnInit() {
        this.route.paramMap.subscribe(async paramMap => {
            this.alertService.alerts.subscribe(alerts => {
                this.alert = alerts.find(alert => alert.id === paramMap.get('id'));
            });
        });
    }

    public async Acknowledge(id: string) {
        await this.alertService.Acknowledge(id);
        this.router.navigateByUrl('/alerts');
    }

    public TimeTravelTo() {
        let timeTravel: Date;
        if (this.alert.end) {
            timeTravel = new Date(this.alert.end);
        } else {
            timeTravel = new Date(this.alert.lastActive);
        }
        timeTravel.setTime(timeTravel.getTime() + (60 * 1000));

        return timeTravel.toISOString();
    }
}
