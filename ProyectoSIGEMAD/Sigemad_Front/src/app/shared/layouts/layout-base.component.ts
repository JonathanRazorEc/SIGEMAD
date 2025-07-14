import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { CustomSidenavComponent } from '../../components/custom-sidenav/custom-sidenav.component';
import { BreadcrumbService } from '../../services/miga-de-pan.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MatToolbarModule, MatButtonModule, MatIconModule, MatSidenavModule, CustomSidenavComponent, CommonModule, RouterModule],

  templateUrl: './layout-base.component.html',
  styleUrl: './layout-base.component.scss',
})
export class LayoutBaseComponent {
  title = 'sigemad';

  public router = inject(Router);

  collapsed = signal(false);
  sidenavWidth = computed(() => (this.collapsed() ? '65px' : '300px'));

  breadcrumbs: Array<{ label: string; url: string }> = [];

  constructor(private breadcrumbService: BreadcrumbService) {}

  ngOnInit(): void {
    this.breadcrumbService.breadcrumbs$.subscribe(breadcrumbs => {
      // Verifica si el último breadcrumb es "catalogs"
      const last = breadcrumbs[breadcrumbs.length - 1];
      if (last && last.label.toLowerCase() === 'catalogs') {
        // Reemplaza el breadcrumb por el texto especial
        this.breadcrumbs = [
          ...breadcrumbs.slice(0, -1),
          { ...last, label: 'Catálogos / Mantenimiento' }
        ];
      } else {
        // Comportamiento por defecto
        this.breadcrumbs = breadcrumbs;
      }
    });
  }  

  goToRoute(url: string) {
    console.log('Redirigiendo a:', url);
    this.router.navigate([url]);
  }

}
