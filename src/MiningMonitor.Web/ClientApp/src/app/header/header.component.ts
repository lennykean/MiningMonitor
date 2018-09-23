import { Component } from '@angular/core';

import { LoginService } from '../login.service';

@Component({
    selector: 'miningmonitor-header',
    templateUrl: './header.component.html'
})
export class HeaderComponent {
    constructor(
        private loginService: LoginService) {
    }

    public get isLoggedIn() {
        return this.loginService.isLoggedIn;
    }
}
