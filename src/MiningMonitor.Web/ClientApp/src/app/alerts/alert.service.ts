import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, timer } from 'rxjs';

import { BasePathService } from '../base-path.service';
import { Alert } from '../models/Alert';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  private static readonly baseUrl = 'alerts';

  private _alertsSubject: BehaviorSubject<Alert[]>;

  constructor(
    private http: HttpClient,
    private basePathService: BasePathService
  ) {}

  public get alerts(): Observable<Alert[]> {
    if (!this._alertsSubject) {
      this._alertsSubject = new BehaviorSubject<Alert[]>([]);
      timer(0, 15000).subscribe(() => this.RefreshAlerts());
    }
    return this._alertsSubject;
  }

  public async Acknowledge(id: string) {
    const url = `${this.basePathService.apiBasePath}${AlertService.baseUrl}/${id}/acknowledge`;

    await this.http.post(url, null).toPromise();
    this.RefreshAlerts();
  }

  private RefreshAlerts() {
    const url = `${this.basePathService.apiBasePath}${AlertService.baseUrl}`;

    this.http.get<Alert[]>(url).subscribe((alerts) => {
      this._alertsSubject.next(alerts);
    });
  }
}
