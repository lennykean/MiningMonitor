import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { TokenService } from './token.service';
import { BasePathService } from './base-path.service';

@Injectable({ providedIn: 'root' })
export class LoginService {
    private static readonly baseUrl = 'login';
    private loginRequired: boolean;

    constructor(
        private http: HttpClient,
        private basePathService: BasePathService,
        private tokenService: TokenService) {
    }

    public get isLoggedIn() {
        return this.tokenService.token != null;
    }

    public async Login(username: string, password: string) {
        try {
            const url = `${this.basePathService.apiBasePath}${LoginService.baseUrl}`;
            this.tokenService.token = await this.http.post<string>(url, { username, password }).toPromise();

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
        this.tokenService.DeleteToken();
        this.loginRequired = null;
    }

    public async LoginRequired() {
        if (this.loginRequired == null) {
            const url = `${this.basePathService.apiBasePath}${LoginService.baseUrl}/required`;
            this.loginRequired = await this.http.get<boolean>(url).toPromise();
        }
        return this.loginRequired;
    }

    public ClearCachedSettings() {
        this.loginRequired = null;
    }
}
