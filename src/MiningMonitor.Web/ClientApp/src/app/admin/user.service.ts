import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { User, UserListItem } from '../models/User';

@Injectable({
    providedIn: 'root'
})
export class UserService {
    private static readonly baseUrl = '/api/users';

    constructor(
        private http: HttpClient) {
    }

    public async GetAll() {
        return await this.http.get<UserListItem[]>(UserService.baseUrl).toPromise();
    }

    public async Create(user: User) {
        return await this.http.post<User>(UserService.baseUrl, user).toPromise();
    }

    public async Delete(username: string) {
        await this.http.delete(`${UserService.baseUrl}/${username}`).toPromise();
    }
}
