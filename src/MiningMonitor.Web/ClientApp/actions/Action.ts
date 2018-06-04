import { ActionType } from '.';
import { Collector, Miner, Snapshot, User } from '../models';
import { Busy } from '../store/state';

export type Action =
    {
        type: ActionType.Busy | ActionType.NotBusy,
        busy: Busy,
    } |
    {
        type: ActionType.FetchAllCollectorsSuccess,
        collectors: Collector[],
    } |
    {
        type: ActionType.UpdateCollectorSuccess | ActionType.DeleteCollectorSuccess,
        collector: Collector,
    } |
    {
        type: ActionType.FetchAllMinersSuccess,
        miners: Miner[],
    } |
    {
        type: ActionType.CreateMinerSuccess | ActionType.UpdateMinerSuccess | ActionType.DeleteMinerSuccess,
        miner: Miner,
    } |
    {
        type: ActionType.FetchSnapshotsSuccess,
        snapshots: Snapshot[],
    } |
    {
        type: ActionType.Validation,
        validation: { [key: string]: string[] },
    } |
    {
        type: ActionType.FetchAllUsersSuccess,
        users: User[],
    } |
    {
        type: ActionType.CreateUserSuccess | ActionType.DeleteUserSuccess,
        user: User,
    } | {
        type: ActionType.FetchAllSettingsSuccess | ActionType.UpdateSettingsSuccess,
        settings: { [key: string]: string },
    } | {
        type: ActionType.LoginSuccess,
        token: string,
    };
