import { Reducer } from 'redux';

import * as _ from 'lodash';

import { Action, ActionType } from '../actions';
import { LoginState } from '../store';

const initialState = {
    token: null,
};

export const loginReducer: Reducer<LoginState> = (state: LoginState = initialState, action: Action) => {
    switch (action.type) {
        case ActionType.LoginSuccess:
            return {
                ...state,
                token: action.token,
            };
        default:
            return state;
    }
};
