import { Reducer } from 'redux';

import { Action, ActionType } from '../actions';
import { SnapshotState } from '../store';

const initialState = {
    snapshots: [],
};

export const snapshotReducer: Reducer<SnapshotState> = (state: SnapshotState = initialState, action: Action) => {
    switch (action.type) {
        case ActionType.FetchSnapshotsSuccess:
            return {
                ...state,
                snapshots: action.snapshots,
            };
        default:
            return state;
    }
};
