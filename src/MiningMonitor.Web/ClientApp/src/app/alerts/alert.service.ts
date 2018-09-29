import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, timer, BehaviorSubject } from 'rxjs';

import { Alert } from '../../models/alert';

@Injectable({
    providedIn: 'root'
})
export class AlertService {
    private static readonly baseUrl = '/api/alerts';

    private _alertsSubject: BehaviorSubject<Alert[]>;

    constructor(
        private http: HttpClient) {
    }

    public get alerts(): Observable<Alert[]> {
        if (!this._alertsSubject) {
            this._alertsSubject = new BehaviorSubject<Alert[]>([]);
            timer(0, 15000).subscribe(() => this.RefreshAlerts());
        }
        return this._alertsSubject;
    }

    public async Get(id: string) {
        return await this.http.get<Alert>(`${AlertService.baseUrl}/${id}`).toPromise();
    }

    public async Acknowledge(id: string) {
        await this.http.post(`${AlertService.baseUrl}/${id}/acknowledge`, null).toPromise();
        this.RefreshAlerts();
    }

    private RefreshAlerts() {
        this.http.get<Alert[]>(AlertService.baseUrl).subscribe(alerts => {
            this._alertsSubject.next(alerts);
        });
    }
}
