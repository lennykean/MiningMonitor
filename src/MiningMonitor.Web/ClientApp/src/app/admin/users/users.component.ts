import { Component, OnInit } from '@angular/core';

import { UserListItem } from '../../models/User';
import { UserService } from '../user.service';

@Component({
  templateUrl: './users.component.html',
})
export class UsersComponent implements OnInit {
  public users: UserListItem[];

  constructor(private userService: UserService) {}

  public async ngOnInit() {
    this.users = await this.userService.GetAll();
  }

  public async Delete(user: UserListItem) {
    await this.userService.Delete(user.username);
    this.users = await this.userService.GetAll();
  }
}
