import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TokenService {
  private static readonly BEARER_TOKEN = 'BEARER_TOKEN';

  public get token(): string {
    return localStorage.getItem(TokenService.BEARER_TOKEN);
  }
  public set token(value: string) {
    localStorage.setItem(TokenService.BEARER_TOKEN, value);
  }

  public DeleteToken() {
    localStorage.removeItem(TokenService.BEARER_TOKEN);
  }
}
