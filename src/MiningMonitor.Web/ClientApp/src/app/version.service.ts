import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Version } from './models/Version';

@Injectable({
    providedIn: 'root'
})
export class VersionService {
    private static readonly baseUrl = '/api/version';

    constructor(
        private http: HttpClient) {
    }

    public async GetVersion() {
        return await this.http.get<Version>(VersionService.baseUrl).toPromise();
    }
}
