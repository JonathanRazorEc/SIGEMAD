import { CommonModule } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterModule, RouterOutlet } from '@angular/router';
import { BreadcrumbService } from '@services/miga-de-pan.service';
import { OpeMenuComponent } from '../../../pages/ope/menu/ope-menu.component';

@Component({
  selector: 'app-ope',
  standalone: true,
  imports: [RouterOutlet, MatToolbarModule, MatButtonModule, MatIconModule, MatSidenavModule, CommonModule, RouterModule, OpeMenuComponent],

  templateUrl: './ope-layout.component.html',
  styleUrl: './ope-layout.component.scss',
})
export class OpeLayoutComponent {
  title = 'sigemad';

  public router = inject(Router);

  menuExpandido = false;

  breadcrumbs: Array<{ label: string; url: string }> = [];

  constructor(private breadcrumbService: BreadcrumbService) {}

  ngOnInit(): void {
    this.breadcrumbService.breadcrumbs$.subscribe((breadcrumbs) => {
      this.breadcrumbs = breadcrumbs;
    });
  }

  goToRoute(url: string) {
    console.log('Redirigiendo a:', url);
    this.router.navigate([url]);
  }

  onMenuStateChange(isExpanded: boolean) {
    this.menuExpandido = isExpanded;
  }
}
