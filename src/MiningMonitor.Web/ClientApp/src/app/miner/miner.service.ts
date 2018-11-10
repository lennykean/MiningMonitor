import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

import { BasePathService } from '../base-path.service';
import { Miner } from '../models/Miner';

@Injectable({ providedIn: 'root' })
export class MinerService {
    private static readonly baseUrl = 'miners';

    private _miners: BehaviorSubject<Miner[]>;

    constructor(
        private http: HttpClient,
        private basePathService: BasePathService, ) {
    }

    public get miners(): Observable<Miner[]> {
        if (!this._miners) {
            this._miners = new BehaviorSubject<Miner[]>([]);
            this.RefreshMiners();
        }
        return this._miners;
    }

    public async Get(id: string) {
        const url = `${this.basePathService.apiBasePath}${MinerService.baseUrl}/${id}`;

        return await this.http.get<Miner>(url).toPromise();
    }

    public async Create(miner: Miner) {
        const url = `${this.basePathService.apiBasePath}${MinerService.baseUrl}`;

        miner = await this.http.post<Miner>(url, miner).toPromise();
        this.RefreshMiners();

        return miner;
    }

    public async Update(miner: Miner) {
        const url = `${this.basePathService.apiBasePath}${MinerService.baseUrl}`;

        miner = await this.http.put<Miner>(url, miner).toPromise();
        this.RefreshMiners();

        return miner;
    }

    public async Delete(id: string) {
        const url = `${this.basePathService.apiBasePath}${MinerService.baseUrl}/${id}`;

        await this.http.delete(url).toPromise();
        this.RefreshMiners();
    }

    private RefreshMiners() {
        const url = `${this.basePathService.apiBasePath}${MinerService.baseUrl}`;

        this.http.get<Miner[]>(url).subscribe(miners => this._miners.next(miners));
    }
}
