import { Component, Input } from '@angular/core';
import { ConnectivityAlertParameters } from '../../models/ConnectivityAlertParameters';

@Component({
  selector: 'mm-connectivity-parameters',
  templateUrl: './connectivity-parameters.component.html',
})
export class ConnectivityParametersComponent {
  @Input()
  public alertParameters: ConnectivityAlertParameters = {
    alertType: null,
  };
}
