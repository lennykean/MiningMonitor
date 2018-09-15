import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TokenService {
    private static readonly BEARER_TOKEN = 'BEARER_TOKEN';

    get token(): string {
        return localStorage.getItem(TokenService.BEARER_TOKEN);
    }
    set token(value: string) {
        localStorage.setItem(TokenService.BEARER_TOKEN, value);
    }
}
