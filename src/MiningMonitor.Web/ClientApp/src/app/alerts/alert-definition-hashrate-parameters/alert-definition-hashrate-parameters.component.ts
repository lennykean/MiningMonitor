import { Component, Input } from '@angular/core';

import { HashrateAlertParameters } from '../../../models/HashrateAlertParameters';

@Component({
    selector: 'miningmonitor-alert-definition-hashrate-parameters',
    templateUrl: './alert-definition-hashrate-parameters.component.html'
})
export class AlertDefinitionHashrateParametersComponent {
    @Input()
    public alertParameters: HashrateAlertParameters = {
        alertMessage: null,
        alertType: null,
        minValue: null
    };
}
