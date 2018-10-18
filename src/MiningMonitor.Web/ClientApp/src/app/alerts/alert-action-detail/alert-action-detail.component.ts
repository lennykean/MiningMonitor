import { Component, EventEmitter, Input, Output } from '@angular/core';

import { AlertActionDefinition } from '../../models/AlertActionDefinition';
import { AlertActionType } from '../../models/AlertActionType';

@Component({
    selector: 'mm-alert-action-detail',
    templateUrl: './alert-action-detail.component.html'
})
export class AlertActionDetailComponent {
    @Input()
    public action: AlertActionDefinition;
    public actionTypes = AlertActionType;
    @Output()
    public done = new EventEmitter();

    public close() {
        this.done.emit();
    }
}
