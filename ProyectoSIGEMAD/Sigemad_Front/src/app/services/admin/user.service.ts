import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { User } from '@type/admin/user.interface';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

interface PaginationResponse<T> {
  count: number;
  pageIndex: number;
  pageSize: number;
  pageCount: number;
  data: T[];
}

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly base = '/AspNetUsers';
  private readonly rolesUrl = '/AspNetRoles';
  private readonly userRolesUrl = '/AspNetUsersRoles';

  constructor(private http: HttpClient) {}

  getUsers(pageIndex: number = 1, pageSize: number = 1000): Observable<User[]> {
    const params = new HttpParams()
      .set('PageIndex', pageIndex)
      .set('PageSize', pageSize);

    return this.http
      .get<PaginationResponse<User>>(this.base, { params })
      .pipe(map((resp) => resp.data));
  }

  getUser(id: any): Observable<User | undefined> {
    return this.getUsers().pipe(
      map((users) => users.find((u: any) => u.id === id))
    );
  }

  createUser(user: any): Observable<any> {
    return this.http.post<any>(this.base, user);
  }

  updateUser(id: string, user: any): Observable<any> {
    return this.http.put<any>(this.base, user);
  }

  deleteUser(id: any): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`);
  }

  getRoles(): Observable<any[]> {
    return this.http.get<any[]>(this.rolesUrl);
  }

  getUserRoles(): Observable<{ userId: string; roleId: string }[]> {
    const params = new HttpParams()
      .set('PageIndex', '1')
      .set('PageSize', '999999');

    return this.http
      .get<PaginationResponse<{ userId: string; roleId: string }>>(this.userRolesUrl, { params })
      .pipe(map((response) => response.data));
  }
}
