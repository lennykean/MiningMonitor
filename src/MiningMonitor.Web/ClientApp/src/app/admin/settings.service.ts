import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { BasePathService } from '../base-path.service';

@Injectable({ providedIn: 'root' })
export class SettingsService {
    private static readonly baseUrl = 'serversettings';

    constructor(
        private http: HttpClient,
        private basePathService: BasePathService) {
    }

    public async GetAll() {
        const url = `${this.basePathService.apiBasePath}${SettingsService.baseUrl}`;
        return await this.http.get<{ [key: string]: string }>(url).toPromise();
    }

    public async Update(settings: { [key: string]: string }) {
        const url = `${this.basePathService.apiBasePath}${SettingsService.baseUrl}`;
        return await this.http.put<{ [key: string]: string }>(url, settings).toPromise();
    }
}
