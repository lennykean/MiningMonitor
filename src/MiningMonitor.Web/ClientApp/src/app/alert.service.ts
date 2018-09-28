import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject, Observable } from 'rxjs';

import { Alert } from '../models/alert';

@Injectable({
    providedIn: 'root'
})
export class AlertService {
    private static readonly baseUrl = '/api/alerts';

    private _alerts: Alert[];
    private _alertsSubject: ReplaySubject<Alert[]>;

    constructor(
        private http: HttpClient) {
    }

    public get alerts(): Observable<Alert[]> {
        if (!this._alertsSubject) {
            this._alertsSubject = new ReplaySubject<Alert[]>();
            this.RefreshAlerts();
        }
        return this._alertsSubject;
    }

    public async Acknowledge(id: string) {
        const result = this.http.post(`${AlertService.baseUrl}/${id}/acknowledge`, null).toPromise();

        this._alerts = [...this._alerts];
        this._alerts.splice(this._alerts.findIndex(a => a.id === id), 1);
        this._alertsSubject.next(this._alerts);

        await result;
    }

    private RefreshAlerts() {
        this.http.get<Alert[]>(AlertService.baseUrl).subscribe(alerts => {
            this._alerts = alerts;
            this._alertsSubject.next(this._alerts);
        });
    }
}
