import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { TokenService } from './token.service';

@Injectable({ providedIn: 'root' })
export class LoginService {
    private static readonly baseUrl = '/api/login';
    private loginRequired: boolean;

    constructor(
        private http: HttpClient,
        private tokenService: TokenService) {
    }

    public async Login(username: string, password: string) {
        try {
            this.tokenService.token = await this.http.post<string>(LoginService.baseUrl, { username, password }).toPromise();
            this.loginRequired = false;
            return true;
        } catch (error) {
            if (error instanceof HttpErrorResponse && error.status === 401) {
                return false;
            }
            throw error;
        }
    }

    public Logout() {
        this.tokenService.token = null;
        this.loginRequired = null;
    }

    public async LoginRequired() {
        if (this.loginRequired == null) {
            this.loginRequired = await this.http.get<boolean>(`${LoginService.baseUrl}/required`).toPromise();
        }
        return this.loginRequired;
    }
}
