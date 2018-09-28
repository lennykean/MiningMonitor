import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, ReplaySubject, BehaviorSubject } from 'rxjs';

import { Miner } from '../models/Miner';

@Injectable({ providedIn: 'root' })
export class MinerService {
    private static readonly baseUrl = '/api/miners';

    private _miners: BehaviorSubject<Miner[]>;

    constructor(
        private http: HttpClient) {
    }

    public get miners(): Observable<Miner[]> {
        if (!this._miners) {
            this._miners = new BehaviorSubject<Miner[]>([]);
            this.RefreshMiners();
        }
        return this._miners;
    }

    public async Get(id: string) {
        return await this.http.get<Miner>(`${MinerService.baseUrl}/${id}`).toPromise();
    }

    public async Create(miner: Miner) {
        miner = await this.http.post<Miner>(MinerService.baseUrl, miner).toPromise();
        this.RefreshMiners();

        return miner;
    }

    public async Update(miner: Miner) {
        miner = await this.http.put<Miner>(MinerService.baseUrl, miner).toPromise();
        this.RefreshMiners();

        return miner;
    }

    public async Delete(id: string) {
        await this.http.delete(`${MinerService.baseUrl}/${id}`).toPromise();
        this.RefreshMiners();
    }

    private RefreshMiners() {
        this.http.get<Miner[]>(MinerService.baseUrl).subscribe(miners => this._miners.next(miners));
    }
}
