import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { BasePathService } from '../base-path.service';
import { Collector } from '../models/Collector';

@Injectable({
  providedIn: 'root',
})
export class CollectorService {
  private static readonly baseUrl = 'collector';

  constructor(
    private http: HttpClient,
    private basePathService: BasePathService
  ) {}

  public async GetAll() {
    const url = `${this.basePathService.apiBasePath}${CollectorService.baseUrl}`;
    return await this.http.get<Collector[]>(url).toPromise();
  }

  public async Create(collector: Collector) {
    const url = `${this.basePathService.apiBasePath}${CollectorService.baseUrl}`;
    return await this.http.post<Collector>(url, collector).toPromise();
  }

  public async Update(collector: Collector) {
    const url = `${this.basePathService.apiBasePath}${CollectorService.baseUrl}`;
    return await this.http.put<Collector>(url, collector).toPromise();
  }

  public async Delete(id: string) {
    const url = `${this.basePathService.apiBasePath}${CollectorService.baseUrl}/${id}`;
    await this.http.delete(url).toPromise();
  }
}
