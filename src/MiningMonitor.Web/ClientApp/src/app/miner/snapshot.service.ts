import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { BasePathService } from '../base-path.service';
import { Snapshot } from '../models/Snapshot';

@Injectable({
    providedIn: 'root'
})
export class SnapshotService {
    private static readonly baseUrl = 'snapshots';

    constructor(
        private http: HttpClient,
        private basePathService: BasePathService) {
    }

    public async GetByMiner(id: string, from?: Date, to?: Date) {
        const url = `${this.basePathService.apiBasePath}${SnapshotService.baseUrl}/${id}`;

        let params = new HttpParams();
        if (from) {
            params = params.append('from', from.toISOString());
        }
        if (to) {
            params = params.append('to', to.toISOString());
        }
        return await this.http.get<Snapshot[]>(url, { params }).toPromise();
    }

}
