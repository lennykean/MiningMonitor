import { fetch } from 'domain-task';

import { addBearerToken, handleResponse } from '.';
import { User } from '../models';

export class UserApi {
    private static readonly resourceUrl = '/api/users';

    public static async GetAll() {
        const response = await fetch(UserApi.resourceUrl, {
            headers: new Headers(addBearerToken()),
        });
        return await handleResponse<User[]>(response);
    }

    public static async Get(id: number) {
        const response = await fetch(`${UserApi.resourceUrl}/${id}`, {
            headers: new Headers(addBearerToken()),
        });
        return await handleResponse<User>(response);
    }

    public static async Create(user: User) {
        const response = await fetch(`${UserApi.resourceUrl}`, {
            headers: new Headers(addBearerToken({
                'Content-Type': 'application/json',
            })),
            method: 'post',
            body: JSON.stringify(user),
        });
        return await handleResponse<void>(response);
    }

    public static async Delete(username: string) {
        const response = await fetch(`${UserApi.resourceUrl}/${username}`, {
            headers: new Headers(addBearerToken()),
            method: 'delete',
        });
        handleResponse<void>(response);
    }
}
