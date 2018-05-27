import { push } from 'react-router-redux';
import { ThunkAction } from 'redux-thunk';

import { Action, ActionType, makeDomainTask } from '.';
import { UserApi } from '../api';
import { User } from '../models';
import { AppState } from '../store';
import { BusyAction } from '../store/state/BusyAction';

export type FetchAllUsersAction = () => ThunkAction<Promise<void>, AppState, any>;
export type CreateUserAction = (user: User, redirectTo?: string) => ThunkAction<Promise<void>, AppState, any>;
export type DeleteUserAction = (user: User, redirectTo?: string) => ThunkAction<Promise<void>, AppState, any>;

interface UserActions {
    fetchAllUsers: FetchAllUsersAction;
    createUser: CreateUserAction;
    deleteUser: DeleteUserAction;
}

export const userActions: UserActions = {
    fetchAllUsers() {
        return async (dispatch, getState) => {
            const busy = {
                action: BusyAction.FetchingUsers,
                context: null,
            };

            dispatch<Action>({
                type: ActionType.Busy,
                busy,
            });
            try {
                dispatch<Action>({
                    type: ActionType.FetchAllUsersSuccess,
                    users: await makeDomainTask(UserApi.GetAll()),
                });
            } catch (error) {
                if (error.unauthorized) {
                    dispatch(push('/login'));
                }
            } finally {
                dispatch<Action>({
                    type: ActionType.NotBusy,
                    busy,
                });
            }
        };
    },
    createUser(user, redirectTo) {
        return async (dispatch, getState) => {
            const busy = {
                action: BusyAction.CreatingUser,
            };

            dispatch<Action>({
                type: ActionType.Busy,
                busy,
            });
            try {
                await makeDomainTask(UserApi.Create(user));
                dispatch<Action>({
                    type: ActionType.CreateUserSuccess,
                    user,
                });
                dispatch<Action>({
                    type: ActionType.Validation,
                    validation: {},
                });
                if (redirectTo) {
                    dispatch(push(redirectTo));
                }
            } catch (error) {
                if (error.validation) {
                    dispatch<Action>({
                        type: ActionType.Validation,
                        validation: error.validation,
                    });
                }
                if (error.unauthorized) {
                    dispatch(push('/login'));
                }
            } finally {
                dispatch<Action>({
                    type: ActionType.NotBusy,
                    busy,
                });
            }
        };
    },
    deleteUser(user, redirectTo) {
        return async (dispatch, getState) => {
            const busy = {
                action: BusyAction.DeleteUser,
                context: user.username,
            };

            dispatch<Action>({
                type: ActionType.Busy,
                busy,
            });
            try {
                await makeDomainTask(UserApi.Delete(user.username));
                dispatch<Action>({
                    type: ActionType.DeleteUserSuccess,
                    user,
                });
                dispatch<Action>({
                    type: ActionType.Validation,
                    validation: {},
                });
                if (redirectTo) {
                    dispatch(push(redirectTo));
                }
            } catch (error) {
                if (error.validation) {
                    dispatch<Action>({
                        type: ActionType.Validation,
                        validation: error.validation,
                    });
                }
                if (error.unauthorized) {
                    dispatch(push('/login'));
                }
            } finally {
                dispatch<Action>({
                    type: ActionType.NotBusy,
                    busy,
                });
            }
        };
    },
};
