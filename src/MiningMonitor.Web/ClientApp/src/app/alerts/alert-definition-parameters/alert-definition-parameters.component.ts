import { Component, Input } from '@angular/core';

import { AlertParameters } from '../../models/AlertParameters';
import { AlertType } from '../../models/AlertType';

@Component({
    selector: 'mm-alert-definition-parameters',
    templateUrl: './alert-definition-parameters.component.html'
})
export class AlertDefinitionParametersComponent {
    @Input()
    public isNew: boolean;
    @Input()
    public alertParameters: AlertParameters = {
        alertType: null
    };
    @Input()
    public validationErrors: { [key: string]: string[] } = {};

    public alertType = AlertType;
}
