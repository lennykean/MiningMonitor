import { Component, OnInit } from '@angular/core';

import { User } from '../../../models/User';
import { UserService } from '../../user.service';

@Component({
    templateUrl: './users.component.html'
})
export class UsersComponent implements OnInit {
    public users: User[];

    constructor(
        private userService: UserService) {
    }

    public async ngOnInit() {
        this.users = await this.userService.GetAll();
    }
}
