import { Component, computed, signal, OnInit } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { CustomSidenavComponent } from './components/custom-sidenav/custom-sidenav.component';
import { CleanUrlStrategy } from './shared/strategies/clean-url-strategy';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MatToolbarModule, MatButtonModule, MatIconModule, MatSidenavModule, CustomSidenavComponent],

  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  title = 'sigemad';

  collapsed = signal(false);
  sidenavWidth = computed(() => (this.collapsed() ? '65px' : '300px'));

  constructor(
    private router: Router,
    private locationStrategy: CleanUrlStrategy
  ) {}

  ngOnInit() {
    // Recuperamos la ruta guardada y navegamos a ella si existe
    const currentRoute = this.locationStrategy.getCurrentRoute();
    if (currentRoute && currentRoute !== '/') {
      this.router.navigateByUrl(currentRoute, { replaceUrl: true });
    }
  }
}
