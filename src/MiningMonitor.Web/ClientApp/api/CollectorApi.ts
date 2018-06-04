import { fetch } from 'domain-task';

import { addBearerToken, handleResponse } from '.';
import { Collector } from '../models';

export class CollectorApi {
    private static readonly resourceUrl = '/api/collector';

    public static async GetAll() {
        const response = await fetch(CollectorApi.resourceUrl, {
            headers: new Headers(addBearerToken()),
        });
        return await handleResponse<Collector[]>(response);
    }

    public static async Update(collector: Collector) {
        const response = await fetch(`${CollectorApi.resourceUrl}`, {
            headers: new Headers(addBearerToken({
                'Content-Type': 'application/json',
            })),
            method: 'put',
            body: JSON.stringify(collector),
        });
        return await handleResponse<Collector>(response);
    }

    public static async Delete(id: string) {
        const response = await fetch(`${CollectorApi.resourceUrl}/${id}`, {
            method: 'delete',
            headers: new Headers(addBearerToken()),
        });
        handleResponse<void>(response);
    }
}
