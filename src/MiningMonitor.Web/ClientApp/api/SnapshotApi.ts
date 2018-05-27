import { fetch } from 'domain-task';

import { addBearerToken, handleResponse } from '.';
import { Snapshot } from '../models';

export class SnapshotApi {
    private static readonly resourceUrl = '/api/snapshots';

    public static async GetByMiner(id: string) {
        const response = await fetch(`${SnapshotApi.resourceUrl}/${id}`, {
            headers: new Headers(addBearerToken()),
        });
        return await handleResponse<Snapshot[]>(response);
    }
}
