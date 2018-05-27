import { push } from 'react-router-redux';
import { ThunkAction } from 'redux-thunk';

import { AppState } from '../store';

import { Action, ActionType, makeDomainTask } from '.';
import { SettingsApi } from '../api';
import { BusyAction } from '../store/state/BusyAction';

export type FetchAllSettingsAction = () => ThunkAction<Promise<void>, AppState, any>;
export type UpdateSettingsAction = (settings: { [key: string]: string }) => ThunkAction<Promise<void>, AppState, any>;

interface SettingsActions {
    fetchAllSettings: FetchAllSettingsAction;
    updateSettings: UpdateSettingsAction;
}

export const settingsActions: SettingsActions = {
    fetchAllSettings() {
        return async (dispatch, getState) => {
            const busy = {
                action: BusyAction.FetchingSettings,
                context: null,
            };

            dispatch<Action>({
                type: ActionType.Busy,
                busy,
            });
            try {
                dispatch<Action>({
                    type: ActionType.FetchAllSettingsSuccess,
                    settings: await makeDomainTask(SettingsApi.GetAll()),
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
    updateSettings(settings) {
        return async (dispatch, getState) => {
            const busy = {
                action: BusyAction.UpdatingSettings,
                context: null,
            };

            dispatch<Action>({
                type: ActionType.Busy,
                busy,
            });
            try {
                dispatch<Action>({
                    type: ActionType.UpdateSettingsSuccess,
                    settings: await makeDomainTask(SettingsApi.Update(settings)),
                });
                dispatch<Action>({
                    type: ActionType.Validation,
                    validation: {},
                });
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
