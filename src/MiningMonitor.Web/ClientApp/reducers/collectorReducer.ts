import { Reducer } from 'redux';

import * as _ from 'lodash';

import { Action, ActionType } from '../actions';
import { CollectorState } from '../store';

const initialState = {
    collectors: [],
};

export const collectorReducer: Reducer<CollectorState> = (state: CollectorState = initialState, action: Action) => {
    switch (action.type) {
        case ActionType.FetchAllCollectorsSuccess:
            return {
                ...state,
                collectors: action.collectors,
            };
        case ActionType.UpdateCollectorSuccess:
            return {
                ...state,
                collectors: [..._.reject(state.collectors, { id: action.collector.id }), action.collector],
            };
        case ActionType.DeleteCollectorSuccess:
            return {
                ...state,
                collectors: [..._.reject(state.collectors, { id: action.collector.id })],
            };
        default:
            return state;
    }
};
