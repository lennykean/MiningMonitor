import { Reducer } from 'redux';

import * as _ from 'lodash';

import { Action, ActionType } from '../actions';
import { SettingsState } from '../store';

const initialState = {
    settings: null,
};

export const settingsReducer: Reducer<SettingsState> = (state: SettingsState = initialState, action: Action) => {
    switch (action.type) {
        case ActionType.FetchAllSettingsSuccess:
        case ActionType.UpdateSettingsSuccess:
            return {
                ...state,
                settings: action.settings,
            };
        default:
            return state;
    }
};
