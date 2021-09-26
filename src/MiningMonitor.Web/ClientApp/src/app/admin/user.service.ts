import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { BasePathService } from '../base-path.service';
import { User, UserListItem } from '../models/User';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private static readonly baseUrl = 'users';

  constructor(
    private http: HttpClient,
    private basePathService: BasePathService
  ) {}

  public async GetAll() {
    const url = `${this.basePathService.apiBasePath}${UserService.baseUrl}`;
    return await this.http.get<UserListItem[]>(url).toPromise();
  }

  public async Create(user: User) {
    const url = `${this.basePathService.apiBasePath}${UserService.baseUrl}`;
    return await this.http.post<User>(url, user).toPromise();
  }

  public async Delete(username: string) {
    const url = `${this.basePathService.apiBasePath}${UserService.baseUrl}/${username}`;
    await this.http.delete(url).toPromise();
  }
}
