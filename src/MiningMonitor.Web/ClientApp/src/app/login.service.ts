import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { LoginCredentials } from '../models/LoginCredentials';
import { TokenService } from './token.service';

@Injectable({ providedIn: 'root' })
export class LoginService {
    private static readonly baseUrl = '/api/login';

    constructor(
        private http: HttpClient,
        private tokenService: TokenService) {
    }

    public async Login(credentials: LoginCredentials) {
        this.tokenService.token = await this.http.post<string>(LoginService.baseUrl, credentials).toPromise();
    }
}
