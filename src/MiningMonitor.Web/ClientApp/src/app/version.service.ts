import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { BasePathService } from './base-path.service';
import { Version } from './models/Version';

@Injectable({
    providedIn: 'root'
})
export class VersionService {
    private static readonly baseUrl = 'version';

    constructor(
        private http: HttpClient,
        private basePathService: BasePathService) {
    }

    public async GetVersion() {
        return await this.http.get<Version>(`${this.basePathService.apiBasePath}${VersionService.baseUrl}`).toPromise();
    }
}
