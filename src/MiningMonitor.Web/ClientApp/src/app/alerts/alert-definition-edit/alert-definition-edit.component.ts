import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { AlertDefinition } from '../../models/AlertDefinition';
import { AlertDefinitionsService } from '../alert-definitions.service';

@Component({
    templateUrl: './alert-definition-edit.component.html'
})
export class AlertDefinitionEditComponent implements OnInit {
    public alertDefinition: AlertDefinition;
    public validationErrors: { [key: string]: string[] } = {};

    constructor(
        private alertDefinitionService: AlertDefinitionsService,
        private route: ActivatedRoute,
        private router: Router) {
    }

    public ngOnInit() {
        this.route.paramMap.subscribe(async paramMap => {
            this.alertDefinition = await this.alertDefinitionService.Get(paramMap.get('id'));
            this.validationErrors = {};
        });
    }

    public async Save(alertDefinition: AlertDefinition) {
        try {
            this.alertDefinition = await this.alertDefinitionService.Update(alertDefinition);
            this.validationErrors = {};
        } catch (error) {
            if (error instanceof HttpErrorResponse && error.status === 400) {
                this.validationErrors = error.error;
            }
        }
    }

    public async Delete() {
        await this.alertDefinitionService.Delete(this.alertDefinition.id);
        this.router.navigateByUrl('/alertdefinitions');
    }
}
