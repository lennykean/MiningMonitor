import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { User } from '../../models/User';
import { UserService } from '../user.service';

@Component({
    templateUrl: './user-create.component.html',
    styleUrls: ['./user-create.component.scss']
})
export class UserCreateComponent {
    public user: User = {
        username: null,
        password: null,
        email: null
    };
    public validationErrors: { [key: string]: string[] } = {};

    constructor(
        private userService: UserService,
        private router: Router) {
    }

    public async Save() {
        try {
            await this.userService.Create(this.user);
            this.router.navigateByUrl('admin/users');
        } catch (error) {
            if (error instanceof HttpErrorResponse && error.status === 400) {
                this.validationErrors = error.error;
            }
        }
    }
}
