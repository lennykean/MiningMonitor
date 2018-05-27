import { Reducer } from 'redux';

import * as _ from 'lodash';

import { Action, ActionType } from '../actions';
import { UserState } from '../store';

const initialState = {
    users: [],
};

export const userReducer: Reducer<UserState> = (state: UserState = initialState, action: Action) => {
    switch (action.type) {
        case ActionType.FetchAllUsersSuccess:
            return {
                ...state,
                users: action.users,
            };
        case ActionType.CreateUserSuccess:
            const { password, ...user } = action.user;
            return {
                ...state,
                users: [...state.users, user],
            };
        case ActionType.DeleteUserSuccess:
            return {
                ...state,
                users: [..._.reject(state.users, { username: action.user.username })],
            };
        default:
            return state;
    }
};
