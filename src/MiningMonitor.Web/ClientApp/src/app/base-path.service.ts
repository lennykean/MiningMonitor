import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BasePathService {
  public get apiBasePath(): string {
    return (window as any).apiBasePath;
  }
}
