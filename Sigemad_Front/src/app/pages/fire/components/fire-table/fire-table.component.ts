import { CommonModule } from '@angular/common';
import {
  AfterViewInit,
  Component,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
  ViewChild
} from '@angular/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Router } from '@angular/router';
import moment from 'moment';

import { FireService } from '@services/fire.service';
import { DEFAULT_PAGESIZE, DEFAULT_PAGESIZE_OPTIONS } from '@type/constants';
import { TooltipDirective } from '../../../../shared/directive/tooltip/tooltip.directive';
import { Fire } from '../../../../types/fire.type';
import { FireCreateComponent } from '../../../fire-evolution-create/fire-evolution-create.component';
import { FireCreateEdit } from '../fire-create-edit-form/fire-create-edit-form.component';

@Component({
  selector: 'app-fire-table',
  standalone: true,
  templateUrl: './fire-table.component.html',
  styleUrls: ['./fire-table.component.scss'],
  imports: [
    MatPaginatorModule,
    MatTableModule,
    MatDialogModule,
    CommonModule,
    MatProgressSpinnerModule,
    TooltipDirective,
    MatSortModule,
  ],
})
export class FireTableComponent implements OnChanges, AfterViewInit {
  @Input() fires: any;
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();
  @Output() pageChanged = new EventEmitter<{
    page: number;
    pageSize: number;
    sortField?: string;
    sortDirection?: string;
  }>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  public dataSource: any[] = [];
  public router = inject(Router);
  private dialog = inject(MatDialog);
  public fireService = inject(FireService);

  public displayedColumns: string[] = [
    'denominacion',
    'fechaInicio',
    'estado',
    'estadoIncendio',
    'ngp',
    'maxNgp',
    'ubicacion',
    'ultimoRegistro',
    'opciones',
  ];

  public pageIndex = 0;
  public pageSize = DEFAULT_PAGESIZE;
  public pageSizeOptions = DEFAULT_PAGESIZE_OPTIONS;
  public totalItems = 0;

  private lastActiveSort!: string;
  private lastDirectionSort!: string;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['fires'] && this.fires) {
      this.dataSource = this.fires.data;
      this.totalItems = this.fires.count ?? 0;
      this.pageIndex = this.fires.pageIndex ?? 0;

      if (this.paginator) {
        this.paginator.pageIndex = this.pageIndex;
        this.paginator.length = this.totalItems;
      }

      if (this.lastActiveSort && this.lastDirectionSort) {
        this.sortData(this.lastActiveSort, this.lastDirectionSort);
      }
    }
  }

  ngAfterViewInit(): void {
    this.paginator.page.subscribe((event: PageEvent) => {
      const nuevoPageSize = event.pageSize;
      sessionStorage.setItem('firePageSize', nuevoPageSize.toString());
      const nuevoPage = event.pageIndex + 1;

      if (this.pageSize !== nuevoPageSize) {
        this.pageSize = nuevoPageSize;
        this.pageIndex = 1;

        Promise.resolve().then(() => {
          if (this.paginator) {
            this.paginator.pageIndex = 0; // esto no dispara el evento .page
          }

          // 🔁 Emite evento al padre para recargar datos con nuevo tamaño
          this.pageChanged.emit({ page: 1, pageSize: nuevoPageSize });
        });
      } else {
        this.pageIndex = nuevoPage;
        this.pageSize = nuevoPageSize;

        this.pageChanged.emit({ page: nuevoPage, pageSize: nuevoPageSize });
      }
    });

    this.sort.sortChange.subscribe(() => {
      const { active, direction } = this.sort;
      this.sortData(active, direction);
      this.lastActiveSort = active;
      this.lastDirectionSort = direction;
    });
  }

  sortData(active: string, direction: string) {
    if (!active || direction === '') return;

    this.dataSource = [...this.dataSource].sort((a: any, b: any) => {
      let valueA: any;
      let valueB: any;

      switch (active) {
        case 'denominacion':
          valueA = a.denominacion;
          valueB = b.denominacion;
          break;
        case 'fechaInicio':
          valueA = new Date(a.fechaInicio);
          valueB = new Date(b.fechaInicio);
          break;
        case 'estado':
          valueA = a.estadoSuceso?.descripcion ?? '';
          valueB = b.estadoSuceso?.descripcion ?? '';
          break;
        case 'estadoIncendio':
          valueA = a.estadoIncendio ?? '';
          valueB = b.estadoIncendio ?? '';
          break;
        case 'ngp':
          valueA = a.sop ?? 0;
          valueB = b.sop ?? 0;
          break;
        case 'maxNgp':
          valueA = a.maxSop ?? 0;
          valueB = b.maxSop ?? 0;
          break;
        case 'ubicacion':
          valueA = a.ubicacion ?? '';
          valueB = b.ubicacion ?? '';
          break;
        case 'ultimoRegistro':
          valueA = a.fechaUltimoRegistro ? new Date(a.fechaUltimoRegistro) : new Date(0);
          valueB = b.fechaUltimoRegistro ? new Date(b.fechaUltimoRegistro) : new Date(0);
          break;
        default:
          return 0;
      }

      // Normalizar para comparar
      if (typeof valueA === 'string') valueA = valueA.toLowerCase();
      if (typeof valueB === 'string') valueB = valueB.toLowerCase();

      const comparison = valueA < valueB ? -1 : valueA > valueB ? 1 : 0;
      return direction === 'asc' ? comparison : -comparison;
    });
  }

  getLastUpdated(fire: Fire) {
    const { fechaUltimoRegistro } = fire;
    if (fechaUltimoRegistro) {
      return moment(fechaUltimoRegistro).format('DD/MM/yyyy HH:mm');
    } else {
      return '';
    }
  }

  getFechaInicio(fecha: any) {
    return moment.utc(fecha).local().format('DD/MM/YYYY HH:mm');
  }

  getUbicacion(fire: Fire) {
    switch (fire.idTerritorio) {
      case 1:
      case 2:
        return `${fire?.municipio?.descripcion}`;
      case 3:
        return `Transfronterizo`;
      default:
        return '';
    }
  }

  goToEdit(fire: Fire) {
    this.router.navigate([`fire/fire-national-edit/${fire.id}`]);
  }

  goToResumen(fire: Fire) {
    this.router.navigate([`fire/fire-auditoria/${fire.id}`]);
  }

  goModal() {
    const dialogRef = this.dialog.open(FireCreateComponent, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - Datos Evolución',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
      }
    });
  }

  goModalEdit(fire: Fire) {
    const dialogRef = this.dialog.open(FireCreateEdit, {
      width: '45vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Modificar - Incendio.',
        fire: fire,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result?.refresh) {
        this.refreshFilterFormChange.emit(!this.refreshFilterForm);
      }
    });
  }
}
