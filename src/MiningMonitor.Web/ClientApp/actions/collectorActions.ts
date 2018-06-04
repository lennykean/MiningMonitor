import { push } from 'react-router-redux';
import { ThunkAction } from 'redux-thunk';

import { Action, ActionType, makeDomainTask } from '.';
import { CollectorApi } from '../api';
import { Collector } from '../models';
import { AppState } from '../store';
import { BusyAction } from '../store/state/BusyAction';

export type FetchAllCollectorsAction = () => ThunkAction<Promise<void>, AppState, any>;
export type UpdateCollectorAction = (collector: Collector) => ThunkAction<Promise<void>, AppState, any>;
export type DeleteCollectorAction = (collector: Collector) => ThunkAction<Promise<void>, AppState, any>;

interface CollectorActions {
    fetchAllCollectors: FetchAllCollectorsAction;
    updateCollector: UpdateCollectorAction;
    deleteCollector: DeleteCollectorAction;
}

export const collectorActions: CollectorActions = {
    fetchAllCollectors() {
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
                    type: ActionType.FetchAllCollectorsSuccess,
                    collectors: await makeDomainTask(CollectorApi.GetAll()),
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
    updateCollector(collector) {
        return async (dispatch, getState) => {
            const busy = {
                action: BusyAction.UpdatingCollector,
                context: String(collector.id),
            };

            dispatch<Action>({
                type: ActionType.Busy,
                busy,
            });
            try {
                dispatch<Action>({
                    type: ActionType.UpdateCollectorSuccess,
                    collector: await makeDomainTask(CollectorApi.Update(collector)),
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
    deleteCollector(collector) {
        return async (dispatch, getState) => {
            const busy = {
                action: BusyAction.UpdatingCollector,
                context: String(collector.id),
            };

            dispatch<Action>({
                type: ActionType.Busy,
                busy,
            });
            try {
                await makeDomainTask(CollectorApi.Delete(collector.id));
                dispatch<Action>({
                    type: ActionType.DeleteCollectorSuccess,
                    collector,
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
