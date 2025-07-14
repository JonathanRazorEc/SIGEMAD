import { MatListModule } from '@angular/material/list';
import { MatIconModule, MatIconRegistry } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterModule } from '@angular/router';
import { ChangeDetectorRef, Component, inject, signal, Input, computed, Renderer2, Output, EventEmitter, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { MenuItemActiveService } from '@services/menu-item-active.service';
import { AuthService } from '@services/auth.service';
import { Menu } from '@type/menu.types';
import { OpeMenuService } from '@services/ope/menu/ope-menu.service';

@Component({
  selector: 'app-ope-menu',
  standalone: true,
  imports: [CommonModule, MatListModule, MatIconModule, RouterLink, RouterModule, NgxSpinnerModule],
  templateUrl: './ope-menu.component.html',
  styleUrl: './ope-menu.component.scss',
})
export class OpeMenuComponent {
  public menuItemActiveService = inject(MenuItemActiveService);
  public opeMenuService = inject(OpeMenuService);
  public changeDetectorRef = inject(ChangeDetectorRef);
  public renderer = inject(Renderer2);
  public iconRegistry = inject(MatIconRegistry);
  public sanitizer = inject(DomSanitizer);
  private spinner = inject(NgxSpinnerService);
  public router = inject(Router);
  private authService = inject(AuthService);
  public title = 'sigemad';
  public active: string | undefined;
  public menuOpeBack = signal<Menu[]>([]);

  expandedMenuId: number | null = null;
  expandedSubMenuId: number | null = null;

  menuExpandido = signal(false);
  @Output() menuStateChange = new EventEmitter<boolean>();
  private el: ElementRef = inject(ElementRef);

  /*
  @Input() set collapsed(val: boolean) {
    this.sideNavCollapsed.set(val);
  }
  */

  iconMap: { [key: string]: string } = {
    // OPE - ADMINISTRACIÓN
    '/ope/administracion/periodos': 'ope-administracion-periodos',
    '/ope/administracion/puertos': 'ope-administracion-puertos',
    '/ope/administracion/lineas-maritimas': 'ope-administracion-lineas-maritimas',
    '/ope/administracion/fronteras': 'ope-administracion-fronteras',
    '/ope/administracion/puntos-control-carreteras': 'ope-administracion-puntos-control-carreteras',
    '/ope/administracion/areas-descanso': 'ope-administracion-areas-descanso',
    '/ope/administracion/areas-estacionamiento': 'ope-administracion-areas-estacionamiento',
    '/ope/administracion/porcentajes-ocupacion-areas-estacionamiento': 'ope-administracion-porcentajes-ocupacion-areas-estacionamiento',
    '/ope/administracion/catalogo/3': 'ope-administracion-catalogo',
    '/ope/administracion/log/4': 'ope-administracion-log',
    '/ope/administracion/historico-sige-2/5': 'ope-administracion-historico-sige-2',
    // OPE - DATOS
    '/ope/datos/embarques-diarios': 'ope-datos-embarques-diarios',
    '/ope/datos/asistencias': 'ope-datos-asistencias',
    '/ope/datos/fronteras': 'ope-datos-fronteras',
    '/ope/datos/afluencia-puntos-control-carreteras': 'ope-datos-afluencia-puntos-control-carreteras',
    '/ope/datos/areas-descanso': 'ope-datos-areas-descanso',
    '/ope/datos/areas-estacionamiento': 'ope-datos-areas-estacionamiento',
    // OPE - APBA
    '/ope/ope-apba-entrada-vehiculos-puertos': 'ope-apba-entrada-vehiculos-puertos',
    '/ope/ope-apba-embarques-vehiculos-intervalos-horarios': 'ope-apba-embarques-vehiculos-intervalos-horarios',
    // OPE - PLANIFICACIÓN
    '/ope/ope-planificacion-plan-flota': 'ope-planificacion-plan-flota',
    '/ope/ope-planificacion-participantes-age': 'ope-planificacion-participantes-age',
    // OPE - INCIDENCIAS
    '/ope/ope-incidencias-datos-inicio': 'ope-incidencias-datos-inicio',
    // OPE - INFORMES
    '/ope/ope-informes-prueba': 'ope-informes-prueba',
    // FIN PCD
  };

  registerIcons(): void {
    const icons = [
      // OPE - ADMINISTRACIÓN
      { name: 'ope-administracion-periodos', path: '/assets/assets/img/ope/administracion/periodo.svg' },
      { name: 'ope-administracion-puertos', path: '/assets/assets/img/ope/administracion/puerto.svg' },
      { name: 'ope-administracion-lineas-maritimas', path: '/assets/assets/img/ope/administracion/linea-maritima.svg' },
      { name: 'ope-administracion-fronteras', path: '/assets/assets/img/ope/administracion/frontera.svg' },
      { name: 'ope-administracion-puntos-control-carreteras', path: '/assets/assets/img/ope/administracion/punto-control-carretera.svg' },
      { name: 'ope-administracion-areas-descanso', path: '/assets/assets/img/ope/administracion/area-descanso.svg' },
      { name: 'ope-administracion-areas-estacionamiento', path: '/assets/assets/img/ope/administracion/aparcamiento.svg' },
      { name: 'ope-administracion-porcentajes-ocupacion-areas-estacionamiento', path: '/assets/assets/img/ope/administracion/porcentaje.svg' },
      { name: 'ope-administracion-catalogo', path: '/assets/assets/img/ope/administracion/catalogo.svg' },
      { name: 'ope-administracion-log', path: '/assets/assets/img/ope/administracion/lupa.svg' },
      { name: 'ope-administracion-historico-sige-2', path: '/assets/assets/img/ope/administracion/lupa.svg' },
      // OPE - DATOS
      { name: 'ope-datos-embarques-diarios', path: '/assets/assets/img/ope/datos/barco.svg' },
      { name: 'ope-datos-asistencias', path: '/assets/assets/img/ope/datos/asistencia.svg' },
      { name: 'ope-datos-fronteras', path: '/assets/assets/img/ope/administracion/frontera.svg' },
      { name: 'ope-datos-afluencia-puntos-control-carreteras', path: '/assets/img/ope.svg' },
      { name: 'ope-datos-areas-descanso', path: '/assets/img/ope.svg' },
      { name: 'ope-datos-areas-estacionamiento', path: '/assets/img/ope.svg' },
      // OPE - APBA
      { name: 'ope-apba-entrada-vehiculos-puertos', path: '/assets/img/search.svg' },
      { name: 'ope-apba-embarques-vehiculos-intervalos-horarios', path: '/assets/img/search.svg' },
      // OPE - PLANIFICACIÓN
      { name: 'ope-planificacion-plan-flota', path: '/assets/img/floods.svg' },
      { name: 'ope-planificacion-participantes-age', path: '/assets/img/floods.svg' },
      // OPE - INCIDENCIAS
      { name: 'ope-incidencias-datos-inicio', path: '/assets/img/dangerous-goods.svg' },
      // OPE - INFORMES
      { name: 'ope-informes-prueba', path: '/assets/img/catalogs.svg' },
      // FIN PCD
    ];

    icons.forEach((icon) => {
      this.iconRegistry.addSvgIcon(icon.name, this.sanitizer.bypassSecurityTrustResourceUrl(icon.path));
    });
  }

  toggleSubmenu(item: any, level: number): void {
    if (item.ruta) {
      this.closeMenu();
      this.redirectTo(item);
      return;
    }

    if (level === 1) {
      if (this.expandedMenuId === item.id) {
        this.expandedMenuId = null;
        item.isOpen = false;
        this.menuExpandido.set(false);
        this.menuStateChange.emit(false);
      } else {
        this.expandedMenuId = item.id;
        item.isOpen = true;
        this.menuExpandido.set(true);
        this.menuStateChange.emit(true);

        const menuItems = this.menuOpeBack();
        if (menuItems?.length) {
          menuItems.forEach((subItem: any) => {
            if (subItem.id !== item.id) {
              subItem.isOpen = false;
            }
          });

          menuItems.forEach((subItem: any) => {
            if (subItem?.subItems?.length) {
              subItem.subItems.forEach((subSubItem: any) => {
                subSubItem.isOpen = false;
              });
            }
          });
        }
      }
    } else if (level === 2) {
      if (this.expandedSubMenuId === item.id) {
        this.expandedSubMenuId = null;
        item.isOpen = false;
      } else {
        this.expandedSubMenuId = item.id;
        item.isOpen = true;

        const menuItems = this.menuOpeBack();
        if (menuItems?.length) {
          menuItems.forEach((subItem: any) => {
            if (subItem?.subItems?.length) {
              subItem.subItems.forEach((subSubItem: any) => {
                if (subSubItem.id !== item.id) {
                  subSubItem.isOpen = false;
                }
              });
            }
          });
        }
      }
    }
  }

  getActiveStyle(item: any, isActive: boolean): { [key: string]: string } {
    if (isActive && item.colorRgb) {
      return {
        backgroundColor: `rgba(${item.colorRgb}, 0.2)`,
        borderLeft: `4px solid rgb(${item.colorRgb})`,
      };
    }
    return {};
  }

  async ngOnInit() {
    this.spinner.show();
    const toolbar = document.querySelector('mat-toolbar');
    this.renderer.setStyle(toolbar, 'z-index', '1');
    this.registerIcons();

    this.menuItemActiveService.set.subscribe((data: string) => {
      this.active = data;
    });

    const respOpeMenu = await this.opeMenuService.get();

    this.menuOpeBack.set(respOpeMenu);
    this.spinner.hide();
    this.renderer.setStyle(toolbar, 'z-index', '5');
  }

  redirectTo(itemSelected: Menu) {
    this.router.navigate([`${itemSelected.ruta}`]);
  }

  logout() {
    console.log('🚀 ~ CustomSidenavComponent ~ logout ~ logout:', 'logout');
    this.authService.logout();
  }

  // Método para cerrar submenú cuando el ratón sale del área
  closeSubmenu(item: any, level: number) {
    if (level === 1 && this.expandedMenuId === item.id) {
      this.expandedMenuId = null; // Cierra el submenú de nivel 1
    } else if (level === 2 && this.expandedSubMenuId === item.id) {
      this.expandedSubMenuId = null; // Cierra el submenú de nivel 2
    }
  }

  // Método para cerrar el submenú
  /*
  closeMenu() {
    this.expandedMenuId = null; // Lógica para cerrar el menú
    this.menuExpandido.set(false);
  }
    */

  closeMenu() {
    const menuElement = this.el.nativeElement.querySelector('mat-nav-list'); // Obtenemos el elemento del menú
    this.renderer.addClass(menuElement, 'menuPlegado'); // Aplicamos la clase de plegado
    setTimeout(() => {
      // Aquí cambiamos el estado para asegurar que la transición se complete antes de cerrar el menú
      this.expandedMenuId = null;
      this.menuExpandido.set(false);
      this.menuStateChange.emit(false);
    }, 300);
  }

  // TEST
  abrirSubmenu(item: any): void {
    this.expandedMenuId = item.id;
    item.isOpen = true;
    this.menuExpandido.set(true);
    this.menuStateChange.emit(true);

    const menuItems = this.menuOpeBack();
    if (menuItems?.length) {
      menuItems.forEach((subItem: any) => {
        if (subItem.id !== item.id) {
          subItem.isOpen = false;
        }
      });

      menuItems.forEach((subItem: any) => {
        if (subItem?.subItems?.length) {
          subItem.subItems.forEach((subSubItem: any) => {
            subSubItem.isOpen = false;
          });
        }
      });
    }
  }
  // FIN TEST
}
