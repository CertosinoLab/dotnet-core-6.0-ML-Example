import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MLService {
  constructor(private httpClient: HttpClient) {}

  async train(): Promise<any> {
    try {
      return this.httpClient.get<any>(`${environment.apiUrl}/train`).toPromise();
    } catch (err) {
      throw err;
    }
  }

  async predict(): Promise<any> {
    try {
      return this.httpClient.get<any>(`${environment.apiUrl}/predict`).toPromise();
    } catch (err) {
      throw err;
    }
  }

}
