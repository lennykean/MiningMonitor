import { fetch } from 'domain-task';

import { addBearerToken, handleResponse } from '.';
import { Miner } from '../models';

export class MinerApi {
    private static readonly resourceUrl = '/api/miners';

    public static async GetAll() {
        const response = await fetch(MinerApi.resourceUrl, {
            headers: new Headers(addBearerToken()),
        });
        return await handleResponse<Miner[]>(response);
    }

    public static async Get(id: string) {
        const response = await fetch(`${MinerApi.resourceUrl}/${id}`, {
            headers: new Headers(addBearerToken()),
        });
        return await handleResponse<Miner>(response);
    }

    public static async Create(miner: Miner) {
        const response = await fetch(`${MinerApi.resourceUrl}`, {
            headers: new Headers(addBearerToken({
                'Content-Type': 'application/json',
            })),
            method: 'post',
            body: JSON.stringify(miner),
        });
        return await handleResponse<Miner>(response);
    }

    public static async Update(miner: Miner) {
        const response = await fetch(`${MinerApi.resourceUrl}`, {
            headers: new Headers(addBearerToken({
                'Content-Type': 'application/json',
            })),
            method: 'put',
            body: JSON.stringify(miner),
        });
        return await handleResponse<Miner>(response);
    }

    public static async Delete(id: string) {
        const response = await fetch(`${MinerApi.resourceUrl}/${id}`, {
            method: 'delete',
            headers: new Headers(addBearerToken()),
        });
        handleResponse<void>(response);
    }
}
