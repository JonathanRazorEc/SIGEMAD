import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItemActiveService } from '../../services/menu-item-active.service';
import { MenuService } from '../../services/menu.service';
import { Menu } from '../../types/menu.types';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
})
export class SidebarComponent {
  public menuItemActiveService = inject(MenuItemActiveService);
  public menuService = inject(MenuService);
  public changeDetectorRef = inject(ChangeDetectorRef);

  public router = inject(Router);

  public title = 'sigemad';

  public active: string | undefined;

  public menuBack = signal<Menu[]>([]);

  public user = {
    name: 'Manuel Ramos Gómez',
    role: 'Supervisor',
  };

  async ngOnInit() {
    this.menuItemActiveService.set.subscribe((data: string) => {
      this.active = data;
    });

    const respMenu = await this.menuService.get();

    this.menuBack.set(respMenu);
  }

  redirectTo(itemSelected: Menu) {
    if (itemSelected.subItems.length) {
      this.menuBack.update((menuActual) => menuActual.map((item) => (item.id === itemSelected.id ? { ...item, isOpen: !item.isOpen } : item)));
    } else {
      this.active = itemSelected.ruta;
      this.router.navigate([`${itemSelected.ruta}`]);
    }
  }
}
