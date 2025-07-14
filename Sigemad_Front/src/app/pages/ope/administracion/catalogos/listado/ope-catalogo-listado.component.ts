import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-ope-catalogo-listado',
  standalone: true,
  templateUrl: './ope-catalogo-listado.component.html',
  styleUrls: ['./ope-catalogo-listado.component.scss'],
  imports: [CommonModule, MatTableModule, MatPaginatorModule, MatSortModule],
})
export class TablaListadoComponent implements OnInit {
  @Input() displayedColumns: string[] = [];
  @Input() dataSource: MatTableDataSource<any> = new MatTableDataSource<any>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit(): void {
    // Si necesitas hacer algo con los datos cuando llegan por @Input, puedes hacerlo aquí
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    //this.setDataSourceAttributes();
  }

  /*
  setDataSourceAttributes() {
    if (this.sort) {
      this.dataSource.sort = this.sort;
      this.dataSource.sortingDataAccessor = (item, property) => {
        switch (property) {
          case 'opePuertoOrigen': {
            return item.opePuertoOrigen?.nombre || '';
          }
          case 'opePuertoDestino': {
            return item.opePuertoDestino?.nombre || '';
          }
          case 'opeFase': {
            return item.opeFase?.nombre || '';
          }
          default: {
            const result = item[property as keyof OpeLineaMaritima];
            return typeof result === 'string' || typeof result === 'number' ? result : '';
          }
        }
      };
      this.dataSource._updateChangeSubscription();
    }
  }
  */
}
