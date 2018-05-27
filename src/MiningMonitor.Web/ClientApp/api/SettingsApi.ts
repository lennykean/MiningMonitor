import { fetch } from 'domain-task';

import { addBearerToken, handleResponse } from '.';

export class SettingsApi {
    private static readonly resourceUrl = '/api/serversettings';

    public static async GetAll() {
        const response = await fetch(SettingsApi.resourceUrl, {
            headers: new Headers(addBearerToken()),
        });
        return await handleResponse<{ [key: string]: string }>(response);
    }

    public static async Update(settings: { [key: string]: string }) {
        const response = await fetch(`${SettingsApi.resourceUrl}`, {
            headers: new Headers(addBearerToken({
                'Content-Type': 'application/json',
            })),
            method: 'put',
            body: JSON.stringify(settings),
        });
        return await handleResponse<{ [key: string]: string }>(response);
    }
}
