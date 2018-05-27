import { fetch } from 'domain-task';

import { handleResponse } from '.';
import { LoginCredentials } from '../models';

export class LoginApi {
    private static readonly resourceUrl = '/api/login';

    public static async Login(credentials: LoginCredentials) {
        const response = await fetch(`${LoginApi.resourceUrl}`, {
            headers: new Headers({
                'Content-Type': 'application/json',
            }),
            method: 'post',
            body: JSON.stringify(credentials),
        });
        return await handleResponse<string>(response);
    }
}
