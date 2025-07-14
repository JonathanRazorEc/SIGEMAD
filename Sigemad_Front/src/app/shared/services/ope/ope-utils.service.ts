import { inject, Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { COUNTRIES_ID } from '@type/constants';
import { NUMEROREGISTROSLOGS } from '@type/ope/ope-constants';
import { CatalogsComponent } from 'src/app/pages/catalogs/catalogs.component';

@Injectable({
  providedIn: 'root', // Hace que esté disponible en toda la aplicación
})
export class OpeUtilsService {
  // LOGS
  private dialog = inject(MatDialog);

  abrirLogs(idTabla: number) {
    this.dialog.open(CatalogsComponent, {
      width: '50vw',
      maxWidth: 'none',
      disableClose: true,
      data: {
        idTablaMaestraGrupo: 4,
        mostrarListadoTablas: false,
        idTablaInicialAMostrar: idTabla,
        numeroRegistrosAMostrar: NUMEROREGISTROSLOGS,
        mostrarFiltros: false,
        mostrarExportarExcel: false,
        mostrarPaginacion: false,
        mostrarAccionesRegistros: false,
        mostrarBottonCerrar: true,
      },
    });
  }
  // FIN LOGS
}
