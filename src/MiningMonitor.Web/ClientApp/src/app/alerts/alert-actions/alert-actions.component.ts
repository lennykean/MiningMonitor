import { Component, Input } from '@angular/core';

import { AlertActionDefinition } from '../../models/AlertActionDefinition';
import { AlertActionType } from '../../models/AlertActionType';

@Component({
    selector: 'mm-alert-actions',
    templateUrl: './alert-actions.component.html'
})
export class AlertActionsComponent {
    @Input()
    public actions: AlertActionDefinition[];
    public actionTypes = AlertActionType;
    public editIndex: number;

    public add() {
        this.editIndex = this.actions.push({ type: null, name: null }) - 1;
    }

    public edit(index: number) {
        this.editIndex = index;
    }

    public remove(index: number) {
        const editing = this.actions[this.editIndex];
        this.actions.splice(index, 1);
        this.editIndex = this.actions.findIndex(a => a === editing);
    }

    public done() {
        this.editIndex = null;
    }
}
