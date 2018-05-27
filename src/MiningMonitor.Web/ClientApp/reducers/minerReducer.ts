import { Reducer } from 'redux';

import * as _ from 'lodash';

import { Action, ActionType } from '../actions';
import { MinerState } from '../store';

const initialState = {
    miners: [],
};

export const minerReducer: Reducer<MinerState> = (state: MinerState = initialState, action: Action) => {
    switch (action.type) {
        case ActionType.FetchAllMinersSuccess:
            return {
                ...state,
                miners: action.miners,
            };
        case ActionType.CreateMinerSuccess:
            return {
                ...state,
                miners: [...state.miners, action.miner],
            };
        case ActionType.UpdateMinerSuccess:
            return {
                ...state,
                miners: [..._.reject(state.miners, { id: action.miner.id }), action.miner],
            };
        case ActionType.DeleteMinerSuccess:
            return {
                ...state,
                miners: [..._.reject(state.miners, { id: action.miner.id })],
            };
        default:
            return state;
    }
};
