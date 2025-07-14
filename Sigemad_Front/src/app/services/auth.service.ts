import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, firstValueFrom, map, throwError } from 'rxjs';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  post(body: any) {
    const endpoint = `/Account/Login`;
   

    return firstValueFrom(
      this.http.post(endpoint, body).pipe(
        map((response) => {
          return response;
        }),
        catchError((error) => {
          return throwError(error.error);
        })
      )
    );
  }

  refreshToken(token: string, refreshToken: string): Observable<any> {
    const endpointRefresh = `/Account/refresh-token`;
    return this.http.post(endpointRefresh, { token, refreshToken });
  }

  logout() {
    sessionStorage.removeItem('jwtToken');
    sessionStorage.removeItem('refreshToken');
    this.router.navigate([`/login`]);
  }
}
