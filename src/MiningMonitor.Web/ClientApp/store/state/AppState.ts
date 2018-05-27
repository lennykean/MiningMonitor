import { RouterState } from 'react-router-redux';

import { CollectorState, MinerState, SettingsState, SnapshotState, UserState, ValidationState } from '.';
import { BusyState } from './BusyState';

export interface AppState {
    busy: BusyState;
    miner: MinerState;
    router: RouterState;
    snapshot: SnapshotState;
    settings: SettingsState;
    user: UserState;
    collector: CollectorState;
    validation: ValidationState;
}
