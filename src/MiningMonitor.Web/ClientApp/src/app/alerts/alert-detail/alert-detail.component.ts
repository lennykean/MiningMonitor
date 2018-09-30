import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Alert } from '../../models/Alert';
import { AlertSeverity } from '../../models/AlertSeverity';
import { AlertService } from '../alert.service';

@Component({
    templateUrl: './alert-detail.component.html'
})
export class AlertDetailComponent implements OnInit {
    public alert: Alert;

    public alertSeverity = AlertSeverity;

    constructor(
        private alertService: AlertService,
        private route: ActivatedRoute,
        private router: Router) {
    }

    public ngOnInit() {
        this.route.paramMap.subscribe(async paramMap => {
            this.alert = await this.alertService.Get(paramMap.get('id'));
        });
    }

    public async Acknowledge(id: string) {
        await this.alertService.Acknowledge(id);
        this.router.navigateByUrl('/alerts');
    }
}
