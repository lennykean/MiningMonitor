import { Component, Input } from '@angular/core';

import { AlertParameters } from '../../../models/AlertParameters';
import { AlertType } from '../../../models/AlertType';

@Component({
    selector: 'miningmonitor-alert-definition-parameters',
    templateUrl: './alert-definition-parameters.component.html'
})
export class AlertDefinitionParametersComponent {
    @Input()
    public alertParameters: AlertParameters = {
        alertMessage: null,
        alertType: null
    };
    public alertType = AlertType;
}
