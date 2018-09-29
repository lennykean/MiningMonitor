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

    public async Update(settings: { [key: string]: string }) {
        return await this.http.put<{ [key: string]: string }>(SettingsService.baseUrl, settings).toPromise();
    }
}
