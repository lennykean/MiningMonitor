import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { Miner } from '../models/Miner';

@Injectable({ providedIn: 'root' })
export class MinerService {
    private static readonly baseUrl = '/api/miners';

    constructor(
        private http: HttpClient) {
    }

    public async GetAll() {
        return await this.http.get<Miner[]>(MinerService.baseUrl).toPromise();
    }

    public async Get(id: string) {
        return await this.http.get<Miner>(`${MinerService.baseUrl}/${id}`).toPromise();
    }

    public async Create(miner: Miner) {
        return await this.http.post<Miner[]>(MinerService.baseUrl, miner).toPromise();
    }

    public async Update(miner: Miner) {
        return await this.http.put<Miner[]>(MinerService.baseUrl, miner).toPromise();
    }

    public async Delete(id: string) {
        return await this.http.delete(`${MinerService.baseUrl}/${id}`).toPromise();
    }
}
