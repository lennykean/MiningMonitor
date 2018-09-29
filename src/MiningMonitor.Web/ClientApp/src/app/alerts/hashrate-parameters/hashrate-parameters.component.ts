import { Component, Input } from '@angular/core';

import { HashrateAlertParameters } from '../../../models/HashrateAlertParameters';

@Component({
    selector: 'mm-hashrate-parameters',
    templateUrl: './hashrate-parameters.component.html'
})
export class HashrateParametersComponent {
    @Input()
    public alertParameters: HashrateAlertParameters = {
        alertMessage: null,
        alertType: null,
        minValue: null
    };
}
