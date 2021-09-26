import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { BasePathService } from '../base-path.service';
import { AlertDefinition } from '../models/AlertDefinition';

@Injectable({
  providedIn: 'root',
})
export class AlertDefinitionsService {
  private static readonly baseUrl = 'alertdefinitions';

  constructor(
    private http: HttpClient,
    private basePathService: BasePathService
  ) {}

  public async GetAll(minerId?: string) {
    let url = `${this.basePathService.apiBasePath}${AlertDefinitionsService.baseUrl}`;

    if (minerId) {
      url = `${url}?minerId=${minerId}`;
    }
    return await this.http.get<AlertDefinition[]>(url).toPromise();
  }

  public async Get(id: string) {
    const url = `${this.basePathService.apiBasePath}${AlertDefinitionsService.baseUrl}/${id}`;
    return await this.http.get<AlertDefinition>(url).toPromise();
  }

  public async Create(alertdefinition: AlertDefinition) {
    const url = `${this.basePathService.apiBasePath}${AlertDefinitionsService.baseUrl}`;
    return await this.http
      .post<AlertDefinition>(url, alertdefinition)
      .toPromise();
  }

  public async Update(alertdefinition: AlertDefinition) {
    const url = `${this.basePathService.apiBasePath}${AlertDefinitionsService.baseUrl}`;
    return await this.http
      .put<AlertDefinition>(url, alertdefinition)
      .toPromise();
  }

  public async Delete(id: string) {
    const url = `${this.basePathService.apiBasePath}${AlertDefinitionsService.baseUrl}/${id}`;
    await this.http.delete(url).toPromise();
  }
}
