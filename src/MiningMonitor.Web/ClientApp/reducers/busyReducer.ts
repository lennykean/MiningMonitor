import { Reducer } from 'redux';

import * as _ from 'lodash';

import { Action, ActionType } from '../actions';
import { BusyState } from '../store';

const initialState = {
    busy: [],
};

export const busyReducer: Reducer<BusyState> = (state: BusyState = initialState, action: Action) => {
    switch (action.type) {
        case ActionType.Busy:
            return {
                ...state,
                busy: [...state.busy, action.busy],
            };

        case ActionType.NotBusy:
            return {
                ...state,
                busy: _.reject(state.busy, (busy) =>
                    busy.action === action.busy.action && (!busy.context || busy.context === action.busy.context),
                ),
            };

        default:
            return state;
    }
};
