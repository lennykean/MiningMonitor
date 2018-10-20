import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Snapshot } from '../models/Snapshot';

@Injectable({
    providedIn: 'root'
})
export class SnapshotService {
    private static readonly baseUrl = '/api/snapshots';

    constructor(
        private http: HttpClient) {
    }

    public async GetByMiner(id: string, from?: Date, to?: Date) {
        let params = new HttpParams();
        if (from) {
            params = params.append('from', from.toISOString());
        }
        if (to) {
            params = params.append('to', to.toISOString());
        }
        return await this.http.get<Snapshot[]>(`${SnapshotService.baseUrl}/${id}`, { params }).toPromise();
    }

}
