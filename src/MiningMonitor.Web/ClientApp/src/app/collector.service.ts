import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Collector } from '../models/Collector';

@Injectable({
    providedIn: 'root'
})
export class CollectorService {
    private static readonly baseUrl = '/api/collector';

    constructor(
        private http: HttpClient) {
    }

    public async GetAll() {
        return await this.http.get<Collector[]>(CollectorService.baseUrl).toPromise();
    }

    public async Create(collector: Collector) {
        return await this.http.post<Collector>(CollectorService.baseUrl, collector).toPromise();
    }

    public async Update(collector: Collector) {
        return await this.http.put<Collector>(CollectorService.baseUrl, collector).toPromise();
    }

    public async Delete(id: string) {
        await this.http.delete(`${CollectorService.baseUrl}/${id}`).toPromise();
    }
}
