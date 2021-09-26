import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

import { LoginService } from './login.service';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private loginService: LoginService, private router: Router) {}
  async canActivate() {
    if (await this.loginService.LoginRequired()) {
      this.router.navigateByUrl('/login');
      return false;
    }
    return true;
  }
}
