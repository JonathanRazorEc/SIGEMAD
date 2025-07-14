import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { HttpErrorResponse, HttpRequest, HttpHandlerFn, HttpEvent } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, filter, switchMap, take } from 'rxjs/operators';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ConfigService } from '../app.config.service';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const apiUrlInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const snackBar = inject(MatSnackBar);
  const configService = inject(ConfigService);

  if (isAssetRequest(req.url)) return next(req);

  const modifiedReq = addAuthHeaders(req, configService);
  
  return next(modifiedReq).pipe(
    catchError((error: HttpErrorResponse) => handleHttpError(error, modifiedReq, next, authService, router, snackBar))
  );
};

// Verifica si la URL pertenece a assets (evita modificarla)
const isAssetRequest = (url: string): boolean => url.startsWith('/assets/');

// Agrega encabezados de autenticación si existe un token
const addAuthHeaders = (req: HttpRequest<any>, configService: ConfigService): HttpRequest<any> => {
  const token = sessionStorage.getItem('jwtToken');
  const updatedUrl = req.url.startsWith('http') ? req.url : `${configService.fullApiUrl}${req.url}`;

  return token 
    ? req.clone({ url: updatedUrl, setHeaders: { Authorization: `Bearer ${token}` } }) 
    : req.clone({ url: updatedUrl });
};

// Manejo de errores HTTP
const handleHttpError = (
  error: HttpErrorResponse, 
  req: HttpRequest<any>, 
  next: HttpHandlerFn, 
  authService: AuthService, 
  router: Router, 
  snackBar: MatSnackBar
): Observable<HttpEvent<any>> => {
  console.error('🔴 Error en la solicitud:', error);

  if (error.status === 401 && !req.url.includes('/refresh-token')) {
    return handle401Error(req, next, authService, router, snackBar);
  }

  return throwError(() => error);
};

// Manejo de errores 401 y refresh token
const handle401Error = (
  req: HttpRequest<any>, 
  next: HttpHandlerFn, 
  authService: AuthService, 
  router: Router, 
  snackBar: MatSnackBar
): Observable<HttpEvent<any>> => {
  if (isRefreshing) return waitForNewToken(req, next);

  isRefreshing = true;
  refreshTokenSubject.next(null);

  const token = sessionStorage.getItem('jwtToken');
  const refreshToken = sessionStorage.getItem('refreshToken');

  if (!token || !refreshToken) return logoutUser(router, snackBar);

  return authService.refreshToken(token, refreshToken).pipe(
    switchMap((response: any) => processNewToken(response, req, next)),
    catchError((err) => logoutUser(router, snackBar, err))
  );
};

// Espera hasta que el nuevo token esté disponible
const waitForNewToken = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> =>
  refreshTokenSubject.pipe(
    filter((token) => token != null),
    take(1),
    switchMap((token) => next(req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })))
  );

// Procesa el nuevo token y continúa la solicitud
const processNewToken = (response: any, req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  isRefreshing = false;
  refreshTokenSubject.next(response.token);
  sessionStorage.setItem('jwtToken', response.token);
  sessionStorage.setItem('refreshToken', response.refreshToken);

  console.log('✅ Token renovado, reintentando solicitud...');
  return next(req.clone({ setHeaders: { Authorization: `Bearer ${response.token}` } }));
};

// Cierra la sesión del usuario y redirige al login
const logoutUser = (router: Router, snackBar: MatSnackBar, error: any = null): Observable<never> => {
  console.warn('🔴 Sesión expirada. Redirigiendo al login...');

  sessionStorage.removeItem('jwtToken');
  sessionStorage.removeItem('refreshToken');

  snackBar.open('⚠️ Sesión finalizada. Inicia sesión nuevamente.', '', {
    duration: 5000,
    verticalPosition: 'top',
    horizontalPosition: 'center',
    panelClass: ['snackbar-error'],
  });

  router.navigate(['/login']);
  return throwError(() => error || new Error('Token o Refresh token no disponible'));
};