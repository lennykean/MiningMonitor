import { AlertActionState } from './AlertActionState';

export interface AlertActionResult {
    state: AlertActionState;
    actionName: string;
    message: string;
}
