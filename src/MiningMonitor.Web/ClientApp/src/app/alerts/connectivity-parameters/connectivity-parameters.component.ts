import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ConnectivityAlertParameters } from '../../models/ConnectivityAlertParameters';

@Component({
  selector: 'mm-connectivity-parameters',
  templateUrl: './connectivity-parameters.component.html',
})
export class ConnectivityParametersComponent {
  @Input()
  alertParameters?: ConnectivityAlertParameters;
  @Input()
  alertDefinitionFormGroup: FormGroup;
}
