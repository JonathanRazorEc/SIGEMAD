import { CommonModule } from '@angular/common';
import { Component, effect, EventEmitter, inject, Input, OnChanges, Output, Renderer2, signal, SimpleChanges, ViewChild } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import moment from 'moment';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeDatoFrontera } from '@type/ope/datos/ope-dato-frontera.type';
import { OpeDatosFronterasService } from '@services/ope/datos/ope-datos-fronteras.service';
import { OpeDatoFronteraCreateEdit } from '../ope-dato-frontera-create-edit-form/ope-dato-frontera-create-edit-form.component';
import { OpeFrontera } from '@type/ope/administracion/ope-frontera.type';
import { RowHighlightDirective } from '@shared/directive/ope/row-highlight.directive';
import { MatSortNoClearDirective } from '@shared/directive/ope/mat-sort-no-clear.directive';
import { TooltipDirective } from '@shared/directive/tooltip/tooltip.directive';

interface FilaDataSourceMapaOpeDatosFronteras {
  clave: string;
  valores: OpeDatoFrontera[];
  total: number;
  fechaInicioFormateada: string;
  fechaFinFormateada: string;
}

@Component({
  selector: 'app-ope-datos-fronteras-table',
  standalone: true,
  imports: [
    MatProgressSpinnerModule,
    MatPaginatorModule,
    MatTableModule,
    CommonModule,
    MatSortModule,
    RowHighlightDirective,
    MatSortNoClearDirective,
    TooltipDirective,
  ],
  templateUrl: './ope-datos-fronteras-table.component.html',
  styleUrl: './ope-datos-fronteras-table.component.scss',
})
export class OpeDatosFronterasTableComponent implements OnChanges {
  @Input() opeDatosFronteras: OpeDatoFrontera[] = [];
  @Input() isLoading: boolean = true;
  @Input() refreshFilterForm: boolean = true;

  @Output() refreshFilterFormChange = new EventEmitter<boolean>();

  public dataSource = new MatTableDataSource<OpeDatoFrontera>([]);
  dataSourceMapaOpeDatosFronteras = new MatTableDataSource<FilaDataSourceMapaOpeDatosFronteras>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  public router = inject(Router);
  private dialog = inject(MatDialog);

  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public alertService = inject(AlertService);
  public snackBar = inject(MatSnackBar);
  public opeDatosFronterasService = inject(OpeDatosFronterasService);
  public routenav = inject(Router);

  public displayedColumns: string[] = ['opeFrontera', 'fechaInicio', 'fechaFin', 'numeroVehiculos', 'afluencia', 'opciones'];

  cabeceraDatosMapa: string = '';
  opeFronterasOrdenadas: OpeFrontera[] = [];
  //public displayedColumnsMapaOpeDatosFronterasRelacionadosPorFechaHora: string[] = ['enlacesEdicion', 'fechaInicio', 'fechaFin', 'datos', 'total'];
  //
  public displayedColumnsMapaOpeDatosFronterasRelacionadosPorFechaHora = [
    'enlacesEdicion',
    'fechaInicio',
    'fechaFin',
    //...this.opeFronterasOrdenadas.map((opeFrontera) => `opeFrontera_${opeFrontera.id}`),
    'total',
  ];
  //

  /*
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opeDatosFronteras'] && this.opeDatosFronteras) {
      this.dataSource.data = this.opeDatosFronteras;
      // PCD
      if (this.paginator) {
        this.paginator.length = this.getMapaOpeDatosFronterasRelacionadosPorFecha()?.length ?? 0;
      }
      // FIN PCD
    }
  }
  */

  // TEST
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['opeDatosFronteras'] && this.opeDatosFronteras) {
      this.dataSource.data = this.opeDatosFronteras;

      const agrupados = this.getMapaOpeDatosFronterasRelacionadosPorFecha(this.opeDatosFronteras);
      this.dataSourceMapaOpeDatosFronteras.data = agrupados;

      if (this.paginator) {
        this.dataSourceMapaOpeDatosFronteras.paginator = this.paginator;
        this.paginator.length = agrupados.length;
      }

      if (this.sort) {
        this.dataSourceMapaOpeDatosFronteras.sort = this.sort;
      }
    }
  }
  // FIN TEST

  /*
  ngAfterViewInit(): void {
    //this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.setDataSourceAttributes();
  }
  */

  //
  ngAfterViewInit(): void {
    this.dataSourceMapaOpeDatosFronteras.paginator = this.paginator;
    this.dataSourceMapaOpeDatosFronteras.sort = this.sort;
    this.setDataSourceAttributes();
  }
  //

  setDataSourceAttributes() {
    if (this.sort) {
      this.dataSourceMapaOpeDatosFronteras.sort = this.sort;
      this.dataSourceMapaOpeDatosFronteras.sortingDataAccessor = (item, property) => {
        switch (property) {
          case 'enlacesEdicion': {
            if (item.valores && item.valores.length > 0) {
              const nombresConcatenados = item.valores.map((v) => v.opeFrontera.nombre.toLowerCase()).join(' ');
              console.log(nombresConcatenados);
              return nombresConcatenados;
            }
            return '';
          }
          case 'fechaInicio': {
            return item.fechaInicioFormateada;
          }
          case 'fechaFin': {
            return item.fechaFinFormateada;
          }
          case 'total': {
            return item.total;
          }
          default: {
            if (property.startsWith('opeFrontera_')) {
              const id = parseInt(property.split('_')[1], 10);
              return this.obtenerNumeroVehiculos(item.valores, id);
            }

            const result = item[property as keyof typeof item];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };
      this.dataSource._updateChangeSubscription();
    }
  }

  goToEdit(frontera: OpeDatoFrontera) {
    //this.router.navigate([`fire/fire-national-edit/1`]);
  }

  goToEditDatoFrontera(opeDatoFrontera: OpeDatoFrontera) {}

  goModal() {
    const dialogRef = this.dialog.open(OpeDatoFronteraCreateEdit, {
      width: '90vw',
      height: '90vh',
      maxWidth: 'none',
      disableClose: true,
      data: {
        title: 'Nuevo - DatoFrontera',
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Modal result:', result);
      }
    });
  }

  /*
  getFechaFormateada(fecha: any) {
    if (!fecha) {
      return '';
    }
    return moment(fecha).format('DD/MM/yyyy');
  }
  */

  /*
  getFechaConHoraIntervaloFormateada(date: any, horaIntervalo: string) {
    //return moment(date).format('DD/MM/YY');
    const [hora, minuto] = horaIntervalo.split(':').map(Number); // Separa y convierte en números
    return moment(date).set({ hour: hora, minute: minuto, second: 0 }).format('DD/MM/YYYY HH:mm');
  }
  */

  //
  getFechaConHoraIntervaloFormateada(opeDatoFrontera: any, tipo: 'inicio' | 'fin') {
    // Si la fecha es personalizada (usando el intervalo personalizado), tomar la hora y minuto del valor personalizado
    const intervaloHorario = opeDatoFrontera?.intervaloHorarioPersonalizado
      ? opeDatoFrontera[`${tipo}IntervaloHorarioPersonalizado`]
      : opeDatoFrontera?.opeDatoFronteraIntervaloHorario?.[tipo];

    // Si no hay hora personalizada, usar el intervalo por defecto
    const [hora, minuto] = intervaloHorario?.split(':').map(Number) || [0, 0]; // Separa y convierte en números, si no tiene valor asignado a 0

    return moment(opeDatoFrontera?.fecha).set({ hour: hora, minute: minuto, second: 0 }).format('DD/MM/YYYY HH:mm');
  }

  //

  getHoraMinutoFormateada(hora: string): string {
    if (!hora) {
      return '';
    }
    // Esperamos formato tipo 'HH:mm:ss'
    return hora.substring(0, 5);
  }

  goModalEdit(opeDatoFrontera: OpeDatoFrontera) {
    const dialogRef = this.dialog.open(OpeDatoFronteraCreateEdit, {
      width: '65vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        //title: 'Modificar - DatoFrontera.',
        opeFrontera: opeDatoFrontera.opeFrontera,
        opeDatoFrontera: opeDatoFrontera,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result?.refresh) {
        const mensaje = result?.borrado ? 'Datos eliminados correctamente!' : 'Datos ingresados correctamente!';
        this.refreshFilterFormChange.emit(!this.refreshFilterForm);
        this.snackBar.open(mensaje, '', {
          duration: 3000,
          horizontalPosition: 'center',
          verticalPosition: 'bottom',
          panelClass: ['snackbar-verde'],
        });
      }
    });
  }

  async deleteOpeDatoFrontera(idOpeDatoFrontera: number) {
    this.alertService
      .showAlert({
        title: '¿Estás seguro de eliminar el registro?',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonText: '¡Sí, eliminar!',
        cancelButtonText: 'Cancelar',
        customClass: {
          title: 'sweetAlert-fsize20',
        },
      })

      .then(async (result) => {
        if (result.isConfirmed) {
          this.spinner.show();
          const toolbar = document.querySelector('mat-toolbar');
          this.renderer.setStyle(toolbar, 'z-index', '1');

          await this.opeDatosFronterasService.delete(idOpeDatoFrontera);
          setTimeout(() => {
            //PCD
            this.snackBar
              .open('Datos eliminados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              })
              .afterDismissed()
              .subscribe(() => {
                this.routenav.navigate(['/ope/datos/fronteras']).then(() => {
                  window.location.href = '/ope/datos/fronteras';
                });
                this.spinner.hide();
              });
            // FIN PCD
          }, 2000);
        } else {
          this.spinner.hide();
        }
      });
  }

  //getMapaOpeDatosFronterasRelacionadosPorFecha() {
  //
  getMapaOpeDatosFronterasRelacionadosPorFecha(data: OpeDatoFrontera[]): FilaDataSourceMapaOpeDatosFronteras[] {
    //
    const mapaOpeDatosFronterasRelacionadosPorFecha: Map<string, OpeDatoFrontera[]> = new Map();

    //this.dataSource.data.forEach((opeDatoFrontera: OpeDatoFrontera) => {
    //
    data.forEach((opeDatoFrontera: OpeDatoFrontera) => {
      //
      const fecha = moment(opeDatoFrontera.fecha).format('YYYY-MM-DD');

      let inicio: string;
      let fin: string;

      if (opeDatoFrontera.intervaloHorarioPersonalizado) {
        inicio = opeDatoFrontera.inicioIntervaloHorarioPersonalizado;
        fin = opeDatoFrontera.finIntervaloHorarioPersonalizado;
      } else {
        inicio = opeDatoFrontera.opeDatoFronteraIntervaloHorario?.inicio;
        fin = opeDatoFrontera.opeDatoFronteraIntervaloHorario?.fin;
      }

      // Si por alguna razón no hay datos, ponemos por defecto 00:00
      inicio = inicio || '00:00';
      fin = fin || '00:00';

      const clave = `${fecha} - ${inicio} a ${fin}h`;
      //

      if (!mapaOpeDatosFronterasRelacionadosPorFecha.has(clave)) {
        mapaOpeDatosFronterasRelacionadosPorFecha.set(clave, []);
      }
      //mapaOpeDatosFronterasRelacionadosPorFecha.get(clave)?.push(opeDatoFrontera);
      this.insertarOrdenadoPorNombreFrontera(mapaOpeDatosFronterasRelacionadosPorFecha, clave, opeDatoFrontera);
    });

    const mapaArray = Array.from(mapaOpeDatosFronterasRelacionadosPorFecha, ([clave, valores]) => ({
      clave,
      valores,
      // TEST
      total: this.getTotalMapa(valores),
      fechaInicioFormateada: this.getFechaConHoraIntervaloFormateada(valores[0], 'inicio'),
      fechaFinFormateada: this.getFechaConHoraIntervaloFormateada(valores[0], 'fin'),
      // FIN TEST
    }));

    if (mapaArray && mapaArray.length > 0) {
      this.opeFronterasOrdenadas = this.getTodasFronterasOrdenadas(mapaArray);
      //this.cabeceraDatosMapa = this.getCabeceraDatosMapa(this.opeFronterasOrdenadas);

      //
      this.displayedColumnsMapaOpeDatosFronterasRelacionadosPorFechaHora = [
        'enlacesEdicion',
        'fechaInicio',
        'fechaFin',
        ...this.opeFronterasOrdenadas.map((opeFrontera) => `opeFrontera_${opeFrontera.id}`),
        'total',
      ];
      //
    }

    // ORDENACIÓN (Ordenar por defecto según fecha y hora inicio descendente)
    mapaArray.sort((a, b) => {
      const fechaHoraA = moment(a.clave.split(' - ')[0] + ' ' + a.clave.split(' - ')[1].split(' a ')[0], 'YYYY-MM-DD HH:mm');
      const fechaHoraB = moment(b.clave.split(' - ')[0] + ' ' + b.clave.split(' - ')[1].split(' a ')[0], 'YYYY-MM-DD HH:mm');
      return fechaHoraB.diff(fechaHoraA); // Descendente
    });

    // FIN ORDENACIÓN

    return mapaArray;
  }

  insertarOrdenadoPorNombreFrontera(mapa: Map<string, OpeDatoFrontera[]>, clave: string, nuevoDato: OpeDatoFrontera): void {
    const lista = mapa.get(clave) ?? [];

    // Buscar la posición de inserción según el nombre
    const index = lista.findIndex((x) => x.opeFrontera.nombre.localeCompare(nuevoDato.opeFrontera.nombre) > 0);

    if (index === -1) {
      lista.push(nuevoDato);
    } else {
      lista.splice(index, 0, nuevoDato);
    }

    mapa.set(clave, lista);
  }

  getInicialNombreFrontera(nombre: string): string {
    if (nombre && nombre.length > 0) {
      return nombre.charAt(0).toUpperCase();
    }
    return ''; // Retorna una cadena vacía si el nombre es vacío o nulo
  }

  /*
  getCabeceraDatosMapa(opeFronterasOrdenadas: OpeFrontera[]): string {
    let cadenaHTML = '';
    if (opeFronterasOrdenadas && Array.isArray(opeFronterasOrdenadas)) {
      cadenaHTML += '<table class="estiloDatosTabla" style="width: 100%"><tr>';
      opeFronterasOrdenadas.forEach((opeFrontera) => {
        cadenaHTML += '<td class="estiloDatosTD">' + opeFrontera.nombre + '</td>';
      });
      cadenaHTML += '</tr></table>';
    }
    return cadenaHTML;
  }
  */

  /*
  getDatosTablaMapa(valores: OpeDatoFrontera[]): string {
    let cadenaHTML = '';
    if (valores && Array.isArray(valores) && this.opeFronterasOrdenadas && this.opeFronterasOrdenadas.length > 0) {
      cadenaHTML += '<table class="estiloDatosTabla"><tr>';

      // Iteramos sobre las fronteras ordenadas
      this.opeFronterasOrdenadas.forEach((opeFronteraOrdenada) => {
        // Comprobamos si hay algún dato en la tabla de valores para la frontera ordenada
        const opeDatoFrontera = valores.find((dato) => dato.opeFrontera.id === opeFronteraOrdenada.id);

        // Si encontramos el dato, mostramos el valor de los vehículos, si no, mostramos valores vacíos
        if (opeDatoFrontera) {
          //cadenaHTML += '<td class="estiloDatosTD">' + opeDatoFrontera.numeroVehiculos + '</td>';
          //cadenaHTML += '<td class="estiloDatosTD">' + opeDatoFrontera.numeroVehiculos + '</td>';
          cadenaHTML += '<td class="estiloDatosTD">' + opeDatoFrontera.numeroVehiculos + '</td>';
        } else {
          //cadenaHTML += '<td class="estiloDatosTD"> </td>'; // Espacio vacío para el caso de no encontrar datos
          //cadenaHTML += '<td class="estiloDatosTD"> </td>';
          cadenaHTML += '<td class="estiloDatosTD"> </td>';
        }
      });

      cadenaHTML += '</tr></table>';
    }

    return cadenaHTML;
  }
  */

  getTotalMapa(valores: OpeDatoFrontera[]): number {
    if (valores && Array.isArray(valores)) {
      return valores.reduce((total, opeDatoFrontera) => {
        return total + (opeDatoFrontera.numeroVehiculos || 0);
      }, 0);
    }

    return 0;
  }

  getTodasFronterasOrdenadas(mapaArray?: Array<{ clave: string; valores: OpeDatoFrontera[] }>): OpeFrontera[] {
    if (!mapaArray || !Array.isArray(mapaArray) || mapaArray.length === 0) {
      return [];
    }

    const opeFronterasMap: Map<number, OpeFrontera> = new Map();

    mapaArray.forEach((item) => {
      if (!item || !Array.isArray(item.valores) || item.valores.length === 0) {
        return;
      }

      item.valores.forEach((opeDatoFrontera) => {
        if (!opeDatoFrontera?.opeFrontera?.id || !opeDatoFrontera.opeFrontera.nombre) {
          return;
        }
        opeFronterasMap.set(opeDatoFrontera.opeFrontera.id, opeDatoFrontera.opeFrontera);
      });
    });

    return Array.from(opeFronterasMap.values()).sort((a, b) => (a.nombre ?? '').localeCompare(b.nombre ?? ''));
  }

  // TEST
  obtenerNumeroVehiculos(valores: OpeDatoFrontera[], idOpeFrontera: number): number | '' {
    const item = valores.find((v) => v.opeFrontera.id === idOpeFrontera);
    return item ? item.numeroVehiculos : '';
  }
  // FIN TEST
}
