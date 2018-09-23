import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { LoginService } from '../login.service';

@Component({ template: 'Logging out...' })
export class LogoutComponent implements OnInit {
    constructor(
        private loginService: LoginService,
        private router: Router) {

    }

    ngOnInit() {
        this.loginService.Logout();
        this.router.navigateByUrl('/');
    }
}
