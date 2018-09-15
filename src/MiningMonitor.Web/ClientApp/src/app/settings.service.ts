import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class SettingsService {
    private static readonly baseUrl = '/api/serversettings';

    constructor(
        private http: HttpClient) {
    }

    public async GetAll() {
        return await this.http.get<{ [key: string]: string }>(SettingsService.baseUrl).toPromise();
    }
}
