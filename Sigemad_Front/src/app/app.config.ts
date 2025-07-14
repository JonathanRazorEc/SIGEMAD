import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { DatePipe, LocationStrategy } from '@angular/common';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { apiUrlInterceptor } from './interceptors/api-url.interceptor';
import { importProvidersFrom } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { getSpanishPaginatorIntl } from './shared/config/custom-paginator-intl';
import { CalendarConfigModule } from './shared/calendar/calendar-config.module';
import { CleanUrlStrategy } from './shared/strategies/clean-url-strategy';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    DatePipe,
    provideAnimations(),
    importProvidersFrom(MatButtonModule, MatDialogModule),
    provideHttpClient(withInterceptors([apiUrlInterceptor])),
    { provide: MatPaginatorIntl, useFactory: getSpanishPaginatorIntl },
    importProvidersFrom(CalendarConfigModule),
    { provide: LocationStrategy, useClass: CleanUrlStrategy }
  ],
};
