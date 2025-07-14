import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CatalogService {
  private readonly baseUrl = '/catalog';

  constructor(private http: HttpClient) {}

  /*
  getCatalogTables(): Observable<any> {
    return this.http.get(`${this.baseUrl}/tables`);
  }
  */

  //
  getCatalogTables(IdTablaMaestraGrupo: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/tables`, {
      params: { idTablaMaestraGrupo: IdTablaMaestraGrupo.toString() },
    });
  }

  //

  getCatalogColumns(tableId: number, forEdit: boolean = false): Observable<any[]> {
    let params = new HttpParams().set('forEdit', String(forEdit));
    return this.http.get<any[]>(`${this.baseUrl}/${tableId}/columns`, { params });
  }

  getCatalogItems(
    tableId: number,
    column?: string,
    value?: string,
    showDeleted: boolean = false,
    page: number = 1,
    pageSize: number = 20,
    orderDirection: 'asc' | 'desc' = 'asc'
  ): Observable<any> {
    let params = new HttpParams().set('showDeleted', showDeleted).set('page', page).set('pageSize', pageSize).set('orderDirection', orderDirection);

    if (column) params = params.set('column', column);
    if (value) params = params.set('value', value);

    return this.http.get(`${this.baseUrl}/${tableId}/items`, { params });
  }

  getCatalogItemsWithFilter(tableId: number, params: any): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${tableId}/items`, { params });
  }

  createCatalogItem(tableId: number, data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/${tableId}/items`, data);
  }

  updateCatalogItem(tableId: number, id: number, data: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${tableId}/items/${id}`, data, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
    });
  }

  deleteCatalogItem(tableId: number, id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${tableId}/items/${id}`);
  }

  getWeatherForecast(): Observable<any> {
    return this.http.get(`/WeatherForecast`);
  }

  exportCatalogAsCsv(tableId: number): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/${tableId}/items/export`, {
      responseType: 'blob',
    });
  }

  exportCatalogAsExcel(tableId: number, showDeleted: boolean): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/${tableId}/items/exportExcel?showDeleted=${showDeleted}`, {
      responseType: 'blob',
    });
  }

  getSelectOptions(nombre: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/select-options/${nombre}`);
  }
}
