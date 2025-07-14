import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import { ActivatedRoute } from '@angular/router';
import { FireAuditoriaService } from '@services/fire-auditoria.service';
import { FireAuditoria } from '@type/fire-auditoria.type';
import moment from 'moment';

@Component({
  selector: 'app-fire-auditoria',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './fire-auditoria.component.html',
  styleUrl: './fire-auditoria.component.scss'
})


export class FireAuditoriaComponent {
  fireData!: any

  constructor(
    private route: ActivatedRoute,
    private fireAuditoriaService: FireAuditoriaService
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id')
      if (id) {
        this.getFireDataById(Number(id))
      }
      else return console.error('No se encuenta el encontrò el campo ID en los params')
    });
  }

  getFireDataById(id: number) {
    this.fireAuditoriaService.getFireDataById(id).subscribe({
      next: (response: FireAuditoria) => {
        const dataTransformed = this.mapDataToFireData(response);
        this.fireData = dataTransformed
      },
      error: (error) => {
        console.error(error.message)
        this.fireData = {}
      }
    })
  }

  /**
 * Transforma la respuesta 'getFireDataById' al formato 
 * esperado por el componente (con 'bloques').
 */
  mapDataToFireData(data: FireAuditoria): any {
    const formatFecha = (fecha: any) => this.getFechaInicio(fecha);
  
    const isFilaConDatos = (fila: string[]) =>
      fila.some(cell => cell !== 'N/A' && cell !== '' && cell !== null);
  
    const bloques = [
      {
        titulo: 'Movilización de medios extraordinarios',
        columnas: ['Solicitante / Aportación', 'Situación', 'Últ. actualización'],
        filas: data.movilizacionMediosExtraOrdinarios?.map((item: any) => [
          item.solicitante || 'N/A',
          'N/A',
          'N/A'
        ]) || [],
        selected: false
      },
      {
        titulo: 'Convocatorias CECOD',
        columnas: ['Fecha inicio', 'Fecha fin', 'Lugar', 'Convocados'],
        filas: data.convocatoriasCECOD?.map((item: any) => [
          formatFecha(item.fechaInicio) || 'N/A',
          formatFecha(item.fechaFin) || 'N/A',
          item.lugar || 'N/A',
          item.convocados || 'N/A',
        ]) || [],
        selected: false
      },
      {
        titulo: 'Activación de planes',
        columnas: ['Tipo de plan', 'Plan', 'Fecha inicio', 'Fecha final', 'Autoridad'],
        filas: data.activacionPlanEmergencias?.map((item: any) => [
          item.tipoPlan ?? 'N/A',
          item.planEmergencia || 'N/A',
          formatFecha(item.fechaInicio) || 'N/A',
          formatFecha(item.fechaFinal) || 'N/A',
          item.autoridad || 'N/A'
        ]) || [],
        selected: false
      },
      {
        titulo: 'Notificaciones oficiales',
        columnas: ['Tipo', 'Fecha', 'Órganos SNPC notificados', 'Órganos extranjeros notificados'],
        filas: data.notificacionesOficiales?.map((item: any) => [
          item.tipoNotificacion?.toString() || 'N/A',
          formatFecha(item.fecha) || 'N/A',
          item.organosSNPCNotificados || 'N/A',
          item.organismoInternacional || 'N/A'
        ]) || [],
        selected: false
      },
      {
        titulo: 'Activacion de sistemas',
        columnas: ['Tipo de Activacion', 'Fecha de activación/gestión', 'Fecha de actualización/confirmación'],
        filas: data.activacionSistemas?.map((item: any) => [
          item.tipoActivacion || 'N/A',
          formatFecha(item.fechaActivacion) || 'N/A',
          formatFecha(item.fechaActualizacion) || 'N/A'
        ]) || [],
        selected: false
      },
      {
        titulo: 'Declaración ZAGEP',
        columnas: ['Fecha', 'Denominación', 'Municipios afectados'],
        filas: data.declaracionZAGEP?.map((item: any) => [
          formatFecha(item.fecha) || 'N/A',
          item.denominacion || 'N/A',
          'N/A'
        ]) || [],
        selected: false
      },
      {
        titulo: 'Declaración Emergencia de interés Nacional',
        columnas: [
          'Autoridad solicitante',
          'Descripción de la solicitud',
          'Fecha de solicitud',
          'Fecha de declaración'
        ],
        filas: data.declaracionEmergenciaInteresNacional
          ? [[
              data.declaracionEmergenciaInteresNacional.autoridadSolicitante || 'N/A',
              data.declaracionEmergenciaInteresNacional.descripcionDeLaSolicitud || 'N/A',
              formatFecha(data.declaracionEmergenciaInteresNacional.fechaSolicitud) || 'N/A',
              formatFecha(data.declaracionEmergenciaInteresNacional.fechaDeclaracion) || 'N/A'
            ]]
          : [],
        selected: false
      },
      {
        titulo: 'Sucesos relacionados',
        columnas: ['Tipo de suceso', 'Fecha', 'Denominación', 'Estado del suceso'],
        filas: data.sucesoRelacionado
          ? [[
              data.sucesoRelacionado.tipoPlan?.toString() || 'N/A',
              formatFecha(data.sucesoRelacionado.fechaCreacion) || 'N/A',
              data.sucesoRelacionado.denominacion || 'N/A',
              data.sucesoRelacionado.estadoSuceso || 'N/A'
            ]]
          : [],
        selected: false
      }
    ];
  
    // ❗ Solo dejamos bloques que tengan al menos una fila con datos reales
    const bloquesFiltrados = bloques.filter(b =>
      b.filas?.some((fila: string[]) => isFilaConDatos(fila))
    );
  
    return {
      vistaPrimaria: data.vistaPrimaria,
      bloques: bloquesFiltrados
    };
  }

  toggleSelection(bloque: any, event?: any) {
    // Si usas checkbox:
    if (event) {
      bloque.selected = event.target.checked;
    } else {
      // Si usas ícono clicable
      bloque.selected = !bloque.selected;
    }
  }

  generatePDF() {
    // 1) Creamos un contenedor temporal en el DOM
    const tempContainer = document.createElement('div');
    tempContainer.style.padding = '20px';
    tempContainer.style.backgroundColor = '#fff';
    tempContainer.style.width = '800px'; // ejemplo de anchura fija
    tempContainer.id = 'tempCaptureContainer';
  
    tempContainer.innerHTML = `
      <h2 style="text-align:center; margin-bottom: 10px;">
        Informe de Incendio Forestal
      </h2>
      <p><strong>Fecha del Informe:</strong> ${new Date().toLocaleString()}</p>
      <hr style="margin: 10px 0;" />
      
      <!-- BLOQUE ENCABEZADO FLEX -->
      <div style="
          display: flex; 
          flex-wrap: wrap; 
          gap: 15px; 
          margin-bottom: 20px;
        "
      >
        <div style="flex: 1; min-width: 180px;">
          <strong>Municipio:</strong> ${this.fireData?.vistaPrimaria?.municipio ?? 'N/A'}
        </div>
        <div style="flex: 1; min-width: 180px;">
          <strong>Estado del suceso:</strong> ${this.fireData?.vistaPrimaria?.estadoSuceso ?? 'N/A'}
        </div>
        <div style="flex: 1; min-width: 180px;">
          <strong>Seguimiento:</strong> ${this.fireData?.vistaPrimaria?.seguimiento ?? 'N/A'}
        </div>
        <div style="flex: 1; min-width: 180px;">
          <strong>Inicio:</strong> ${this.fireData?.vistaPrimaria?.inicio ?? 'N/A'}
        </div>
        <div style="flex: 1; min-width: 180px;">
          <strong>Nota general:</strong> ${this.fireData?.vistaPrimaria?.notaGeneral ?? 'N/A'}
        </div>
        <div style="flex: 1; min-width: 180px;">
          <strong>Superficie afectada:</strong> ${this.fireData?.vistaPrimaria?.superficieAfectadaHa ?? 'N/A'}
        </div>
        <div style="flex: 1; min-width: 180px;">
          <strong>Clase de suceso:</strong> ${this.fireData?.vistaPrimaria?.claseSuceso ?? 'N/A'}
        </div>
      </div>
      <!-- FIN BLOQUE ENCABEZADO FLEX -->
      
      <div id="tempBlocks"></div>
    `;
  
    document.body.appendChild(tempContainer);
  
    // 2) Copiamos sólo los bloques seleccionados
    const selectedBlocksContainer = tempContainer.querySelector('#tempBlocks') as HTMLDivElement;
  
    this.fireData?.bloques?.forEach((bloque: any) => {
      if (bloque.selected) {
        const blockHTML = document.createElement('div');
        blockHTML.innerHTML = `
          <h3 style="margin: 10px 0;">${bloque.titulo}</h3>
          <table style="width: 100%; border-collapse: collapse; margin-bottom: 15px; font-size: 12px;">
            <thead>
              <tr>
                ${bloque.columnas.map((col: string) =>
                  `<th style="border: 1px solid #ddd; padding: 5px; background: #f5f5f5;">${col}</th>`
                ).join('')}
              </tr>
            </thead>
            <tbody>
              ${bloque.filas.map((fila: string[]) => `
                <tr>
                  ${fila.map((celda: string) =>
                    `<td style="border: 1px solid #ddd; padding: 5px;">${celda}</td>`
                  ).join('')}
                </tr>
              `).join('')}
            </tbody>
          </table>
        `;
        selectedBlocksContainer.appendChild(blockHTML);
      }
    });
  
    // 3) Capturamos el contenedor temporal con html2canvas
    html2canvas(tempContainer, { scale: 2 }).then((canvas) => {
      // 4) Generamos el PDF con jsPDF
      const imgData = canvas.toDataURL('image/png');
      const pdf = new jsPDF('p', 'mm', 'a4');
  
      const pageWidth = pdf.internal.pageSize.getWidth();
      const pageHeight = pdf.internal.pageSize.getHeight();
  
      const imgProps = pdf.getImageProperties(imgData);
      const pdfWidth = pageWidth - 20;  // margen horizontal
      const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;
  
      const posX = 10;
      const posY = 10;
  
      if (pdfHeight > (pageHeight - posY)) {
        const ratio = (pageHeight - posY - 10) / pdfHeight;
        pdf.addImage(imgData, 'PNG', posX, posY, pdfWidth * ratio, pdfHeight * ratio);
      } else {
        pdf.addImage(imgData, 'PNG', posX, posY, pdfWidth, pdfHeight);
      }
  
      // Pie de página (ejemplo)
      pdf.setFont('helvetica', 'normal');
      pdf.setFontSize(10);
      pdf.setTextColor(100);
      pdf.text('Informe generado automáticamente (demo).', 10, pageHeight - 10);
  
      // Guardamos el PDF
      pdf.save('InformeIncendio.pdf');
    }).finally(() => {
      // 5) Eliminamos el contenedor temporal para limpiar el DOM
      document.body.removeChild(tempContainer);
    });
  }

  getFechaInicio(fecha: any): string {
    if (!fecha || !moment(fecha).isValid()) {
      return 'N/A';
    }
    return moment(fecha).format('DD/MM/yyyy HH:mm');
  }


}
