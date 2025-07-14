import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, NavigationEnd, Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { filter } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class BreadcrumbService {
  private breadcrumbs = new BehaviorSubject<Array<{ label: string; url: string }>>([]);
  breadcrumbs$ = this.breadcrumbs.asObservable();

  constructor(private router: Router) {
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        const root = this.router.routerState.snapshot.root;
        console.log(this.createBreadcrumbs(root)); // Esto imprimirá las migas generadas
        this.breadcrumbs.next(this.createBreadcrumbs(root));
      });
  }

  private createBreadcrumbs(
    route: ActivatedRouteSnapshot,
    url: string = '',
    breadcrumbs: Array<{ label: string; url: string }> = []
  ): Array<{ label: string; url: string }> {
    const routeUrl = route.url.map(segment => segment.path).join('/');
    const nextUrl = routeUrl ? `${url}/${routeUrl}` : url;

    if (route.data['breadcrumb']) {
      breadcrumbs.push({
        label: route.data['breadcrumb'],
        url: nextUrl,
      });
    }

    if (route.firstChild) {
      return this.createBreadcrumbs(route.firstChild, nextUrl, breadcrumbs);
    }

    return breadcrumbs;
  }
}
