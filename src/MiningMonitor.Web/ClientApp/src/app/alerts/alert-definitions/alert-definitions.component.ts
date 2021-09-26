import { Component, OnInit } from '@angular/core';

import { AlertDefinition } from '../../models/AlertDefinition';
import { AlertDefinitionsService } from '../alert-definitions.service';

@Component({
  templateUrl: './alert-definitions.component.html',
})
export class AlertDefinitionsComponent implements OnInit {
  public alertDefinitions: AlertDefinition[];

  constructor(private alertDefinitionService: AlertDefinitionsService) {}

  async ngOnInit() {
    this.alertDefinitions = await this.alertDefinitionService.GetAll();
  }
}
