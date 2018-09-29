import { Component, OnInit } from '@angular/core';

import { AlertDefinitionsService } from '../alert-definitions.service';
import { AlertDefinition } from '../../../models/AlertDefinition';

@Component({
    templateUrl: './alert-definitions.component.html',
    styleUrls: ['./alert-definitions.component.scss']
})
export class AlertDefinitionsComponent implements OnInit {
    public alertDefinitions: AlertDefinition[];

    constructor(
        private alertDefinitionService: AlertDefinitionsService) {
    }

    async ngOnInit() {
        this.alertDefinitions = await this.alertDefinitionService.GetAll();
    }
}
