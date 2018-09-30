import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { AlertDefinition } from '../models/AlertDefinition';

@Injectable({
    providedIn: 'root'
})
export class AlertDefinitionsService {
    private static readonly baseUrl = '/api/alertdefinitions';

    constructor(
        private http: HttpClient) {
    }

    public async GetAll(minerId?: string) {
        if (minerId) {
            return await this.http.get<AlertDefinition[]>(`${AlertDefinitionsService.baseUrl}?minerId=${minerId}`).toPromise();
        }
        return await this.http.get<AlertDefinition[]>(AlertDefinitionsService.baseUrl).toPromise();
    }

    public async Get(id: string) {
        return await this.http.get<AlertDefinition>(`${AlertDefinitionsService.baseUrl}/${id}`).toPromise();
    }

    public async Create(alertdefinition: AlertDefinition) {
        return await this.http.post<AlertDefinition>(AlertDefinitionsService.baseUrl, alertdefinition).toPromise();
    }

    public async Update(alertdefinition: AlertDefinition) {
        return await this.http.put<AlertDefinition>(AlertDefinitionsService.baseUrl, alertdefinition).toPromise();
    }

    public async Delete(id: string) {
        await this.http.delete(`${AlertDefinitionsService.baseUrl}/${id}`).toPromise();
    }
}
