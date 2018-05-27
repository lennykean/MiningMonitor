import { push } from 'react-router-redux';
import { ThunkAction } from 'redux-thunk';

import { Action, ActionType, makeDomainTask } from '.';
import { LoginApi } from '../api';
import { AppState } from '../store';
import { BusyAction } from '../store/state/BusyAction';

export type LoginAction = (username: string, password: string) => ThunkAction<Promise<void>, AppState, any>;

interface LoginActions {
    login: LoginAction;
}

export const loginActions: LoginActions = {
    login(username, password) {
        return async (dispatch, getState) => {
            const busy = {
                action: BusyAction.LoggingIn,
                context: null,
            };

            dispatch<Action>({
                type: ActionType.Busy,
                busy,
            });
            try {
                const token = await makeDomainTask(LoginApi.Login({ username, password }));
                if (typeof localStorage !== 'undefined') {
                    localStorage.setItem('bearerToken', token);
                }
                dispatch<Action>({
                    type: ActionType.LoginSuccess,
                    token,
                });
                dispatch(push('/'));
            } finally {
                dispatch<Action>({
                    type: ActionType.NotBusy,
                    busy,
                });
            }
        };
    },
};
