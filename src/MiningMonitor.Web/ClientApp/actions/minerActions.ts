import { push } from 'react-router-redux';
import { ThunkAction } from 'redux-thunk';

import { Action, ActionType, makeDomainTask } from '.';
import { MinerApi, SnapshotApi } from '../api';
import { Miner } from '../models';
import { AppState } from '../store';
import { BusyAction } from '../store/state/BusyAction';

export type FetchAllMinersAction = () => ThunkAction<Promise<void>, AppState, any>;
export type CreateMinerAction = (miner: Miner, redirectTo?: string) => ThunkAction<Promise<void>, AppState, any>;
export type UpdateMinerAction = (miner: Miner) => ThunkAction<Promise<void>, AppState, any>;
export type DeleteMinerAction = (miner: Miner, redirectTo?: string) => ThunkAction<Promise<void>, AppState, any>;
export type FetchSnapshotsAction = (minerId: string, period?: number) => ThunkAction<Promise<void>, AppState, any>;

interface MinerActions {
    fetchAllMiners: FetchAllMinersAction;
    createMiner: CreateMinerAction;
    updateMiner: UpdateMinerAction;
    deleteMiner: DeleteMinerAction;
    fetchSnapshots: FetchSnapshotsAction;
}

export const minerActions: MinerActions = {
    fetchAllMiners() {
        return async (dispatch, getState) => {
            const busy = {
                action: BusyAction.FetchingMiners,
                context: null,
            };

            dispatch<Action>({
                type: ActionType.Busy,
                busy,
            });
            try {
                dispatch<Action>({
                    type: ActionType.FetchAllMinersSuccess,
                    miners: await makeDomainTask(MinerApi.GetAll()),
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
    createMiner(miner, redirectTo) {
        return async (dispatch, getState) => {
            const busy = {
                action: BusyAction.CreatingMiner,
            };

            dispatch<Action>({
                type: ActionType.Busy,
                busy,
            });
            try {
                dispatch<Action>({
                    type: ActionType.CreateMinerSuccess,
                    miner: await makeDomainTask(MinerApi.Create(miner)),
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
    updateMiner(miner) {
        return async (dispatch, getState) => {
            const busy = {
                action: BusyAction.UpdatingMiner,
                context: String(miner.id),
            };

            dispatch<Action>({
                type: ActionType.Busy,
                busy,
            });
            try {
                dispatch<Action>({
                    type: ActionType.UpdateMinerSuccess,
                    miner: await makeDomainTask(MinerApi.Update(miner)),
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
    deleteMiner(miner, redirectTo) {
        return async (dispatch, getState) => {
            const busy = {
                action: BusyAction.DeleteMiner,
                context: String(miner.id),
            };

            dispatch<Action>({
                type: ActionType.Busy,
                busy,
            });
            try {
                await makeDomainTask(MinerApi.Delete(miner.id));
                dispatch<Action>({
                    type: ActionType.DeleteMinerSuccess,
                    miner,
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
    fetchSnapshots(minerId, period?) {
        return async (dispatch, getState) => {
            const busy = {
                action: BusyAction.FetchingSnapshots,
                context: String(minerId),
            };

            dispatch<Action>({
                type: ActionType.Busy,
                busy,
            });
            try {
                dispatch<Action>({
                    type: ActionType.FetchSnapshotsSuccess,
                    snapshots: await makeDomainTask(SnapshotApi.GetByMiner(minerId)),
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
};
