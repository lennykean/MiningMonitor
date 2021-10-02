import { Component, Input } from '@angular/core';
import { faEdit, faTimes } from '@fortawesome/free-solid-svg-icons';

import { AlertActionDefinition } from '../../models/AlertActionDefinition';
import { AlertActionType } from '../../models/AlertActionType';

@Component({
  selector: 'mm-alert-actions',
  templateUrl: './alert-actions.component.html',
})
export class AlertActionsComponent {
  readonly faEdit = faEdit;
  readonly faTimes = faTimes;

  @Input()
  actions: AlertActionDefinition[] = [];
  actionTypes = AlertActionType;
  editIndex: number;
  editAction: Partial<AlertActionDefinition>;

  add() {
    this.editIndex = this.actions?.length ?? 0;
    this.editAction = {};
  }

  edit(index: number) {
    this.editIndex = index;
    this.editAction = this.actions[index];
  }

  remove(index: number) {
    const editing = this.actions[this.editIndex];
    this.actions.splice(index, 1);
    this.editIndex = this.actions.findIndex((a) => a === editing);
  }

  done(action: AlertActionDefinition) {
    this.actions[this.editIndex] = action;
    this.editIndex = null;
    this.editAction = null;
  }
}
