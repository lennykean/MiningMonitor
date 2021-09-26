import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { AlertDefinition } from '../../models/AlertDefinition';
import { AlertDefinitionsService } from '../alert-definitions.service';

@Component({
  templateUrl: './alert-definition-create.component.html',
})
export class AlertDefinitionCreateComponent {
  public validationErrors: { [key: string]: string[] } = {};

  constructor(
    private alertDefinitionService: AlertDefinitionsService,
    private router: Router
  ) {}

  public async Save(alertDefinition: AlertDefinition) {
    try {
      alertDefinition = await this.alertDefinitionService.Create(
        alertDefinition
      );
      this.router.navigateByUrl(`/alertdefinition/${alertDefinition.id}`);
    } catch (error) {
      if (error instanceof HttpErrorResponse && error.status === 400) {
        this.validationErrors = error.error;
      }
    }
  }
}
