import { Component } from '@angular/core';
import { faCog, faSignOutAlt } from "@fortawesome/free-solid-svg-icons";

import { LoginService } from '../login.service';

@Component({
    selector: 'mm-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
    public readonly faCog = faCog;
    public readonly faSignOutAlt = faSignOutAlt;

    constructor(
        private loginService: LoginService) {
    }

    public get isLoggedIn() {
        return this.loginService.isLoggedIn;
    }
}
