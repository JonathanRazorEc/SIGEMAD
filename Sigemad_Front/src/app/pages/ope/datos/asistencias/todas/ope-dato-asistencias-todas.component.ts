import { CommonModule } from '@angular/common';
import {
  ChangeDetectorRef,
  Component,
  DoCheck,
  EventEmitter,
  Input,
  OnChanges,
  OnInit,
  Output,
  SimpleChanges,
  ViewChild,
  inject,
  signal,
} from '@angular/core';
import { FormBuilder, FormGroup, FormGroupDirective, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { UtilsService } from '@shared/services/utils.service';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OpeDatoAsistenciaTraduccion } from '@type/ope/datos/ope-dato-asistencia-traduccion.type';
import { OpeValidator } from '@shared/validators/ope/ope-validator';
import { OpeDatoAsistenciaSanitaria } from '@type/ope/datos/ope-dato-asistencia-sanitaria.type';
import { OpeDatoAsistenciaSocial } from '@type/ope/datos/ope-dato-asistencia-social.type';
import { OpeDatoAsistencia } from '@type/ope/datos/ope-dato-asistencia.type';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';

interface FormType {
  id?: number;
  numero: number;
  observaciones: string;
}

@Component({
  selector: 'app-ope-dato-asistencias-todas',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatButtonModule,
    MatTableModule,
    MatIconModule,
    NgxSpinnerModule,
    FlexLayoutModule,
    MatPaginatorModule,
    MatSortModule,
  ],
  templateUrl: './ope-dato-asistencias-todas.component.html',
  styleUrl: './ope-dato-asistencias-todas.component.scss',
})
export class OpeDatoAsistenciasTodasComponent implements OnChanges {
  @Input() opeDatoAsistencia: OpeDatoAsistencia | null = null;

  constructor(public utilsService: UtilsService) {}

  public dataSource = new MatTableDataSource<OpeDatoAsistencia>([]);
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  public displayedColumns: string[] = ['fecha'];

  ngOnInit(): void {
    if (this.opeDatoAsistencia) {
      this.dataSource.data = [this.opeDatoAsistencia];
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (this.opeDatoAsistencia) {
      this.dataSource.data = [this.opeDatoAsistencia];
    } else {
      this.dataSource.data = [];
    }
  }
}
