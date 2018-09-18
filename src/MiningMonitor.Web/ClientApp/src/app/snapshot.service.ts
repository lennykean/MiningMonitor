import { HttpClient } from '@angular/common/http';
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

    public async GetByMiner(id: string) {
        return await this.http.get<Snapshot[]>(`${SnapshotService.baseUrl}/${id}`);
    }
}
