import { BusyAction } from './BusyAction';

export interface Busy {
    action: BusyAction;
    context?: string;
}
