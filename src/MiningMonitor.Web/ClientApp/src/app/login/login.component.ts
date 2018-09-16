import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { LoginService } from '../login.service';

@Component({
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent {
    public busy = false;
    public message: string;

    constructor(
        private loginService: LoginService,
        private router: Router) {
    }

    async Login(username: string, password: string) {
        try {
            this.busy = true;
            if (await this.loginService.Login(username, password)) {
                this.router.navigateByUrl('/');
            } else {
                this.message = 'Invalid username or password';
            }
        }
        finally {
            this.busy = false;
        }
    }
}
