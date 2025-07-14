import { CommonModule } from '@angular/common';
import { Component, inject, Input, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ActionsRelevantService } from '@services/actions-relevant.service';
import { EvolutionService } from '@services/evolution.service';
import { ImpactEvolutionService } from '@services/impact-evolution.service';
import { FlexLayoutModule } from '@angular/flex-layout';

@Component({
  selector: 'app-fire-grafico-historico',
  standalone: true,
  imports: [CommonModule, FlexLayoutModule],
  templateUrl: './fire-grafico-historico.component.html',
  styleUrl: './fire-grafico-historico.component.scss',
})
export class FireGraficoHistoricoComponent implements OnInit {
  @Input()
  dataSource!: MatTableDataSource<any>;

  // Mapas para las filas del gráfico
  listadoEvoluciones: any[] = [];
  listadoActuacionesRelevantes: any[] = [];
  filaDias: Map<string, { hora: string; estado: string }[]> = new Map();
  filaEvolucion: Map<string, { hora: string; estado: string }[]> = new Map();
  filaNivelSituacionOperativaEquivalente: Map<string, { hora: string; estado: string }[]> = new Map();
  filaActivacionSistemas: Map<string, { hora: string; estado: string }[]> = new Map();
  filaAfectaciones: Map<string, { hora: string; estado: string }[]> = new Map();
  filaMediosExtincionOrdinarios: Map<string, { hora: string; estado: string }[]> = new Map();
  filaMediosExtincionExtraordinariosNacionales: Map<string, { hora: string; estado: string }[]> = new Map();
  filaMediosExtincionExtraordinariosInternacionales: Map<string, { hora: string; estado: string }[]> = new Map();

  public evolutionService = inject(EvolutionService);
  public actuacionesRelevantesService = inject(ActionsRelevantService);
  public impactosEvolucionesService = inject(ImpactEvolutionService);

  async ngOnInit() {
    await this.cargarDatos();
  }

  async cargarDatos() {
    // PCD
    this.listadoEvoluciones = [];
    this.listadoActuacionesRelevantes = [];

    if (this.dataSource.data != null && this.dataSource.data.length > 0) {
      for (const actualizacion of this.dataSource.data) {
        if (actualizacion.tipoRegistro === 'Datos de evolución') {
          const evolucion: any = await this.evolutionService.getById(actualizacion.id);
          if (evolucion != null) {
            this.listadoEvoluciones.push(evolucion);
          }
        } else if (actualizacion.tipoRegistro === 'Actuaciones Relevantes') {
          const actuacionRelevante: any = await this.actuacionesRelevantesService.getById(actualizacion.id);
          if (actuacionRelevante != null) {
            this.listadoActuacionesRelevantes.push(actuacionRelevante);
          }
        }
      }
    }

    await this.cargarFilaEvolucion();
    await this.cargarFilaNivelSituacionOperativaEquivalente();
    await this.cargarFilaActivacionSistemas();
    await this.cargarFilaAfectaciones();
    await this.cargarFilaMediosExtincionOrdinarios();
    await this.cargarFilaMediosExtincionExtraordinariosNacionales();
    await this.cargarFilaEMediosExtincionExtraordinariosInternacionales();

    // Obtenemos el rango completo de fechas
    const menorFechaHistorico = this.obtenerMenorFechaHistorico(
      [
        this.filaEvolucion,
        this.filaNivelSituacionOperativaEquivalente,
        this.filaActivacionSistemas,
        this.filaAfectaciones,
        this.filaMediosExtincionOrdinarios,
        this.filaMediosExtincionExtraordinariosNacionales,
        this.filaMediosExtincionExtraordinariosInternacionales,
      ].filter((m) => m instanceof Map && m.size > 0) // Filtra mapas vacíos
    );

    const mayorFechaHistorico = this.obtenerMayorFechaHistorico(
      [
        this.filaEvolucion,
        this.filaNivelSituacionOperativaEquivalente,
        this.filaActivacionSistemas,
        this.filaAfectaciones,
        this.filaMediosExtincionOrdinarios,
        this.filaMediosExtincionExtraordinariosNacionales,
        this.filaMediosExtincionExtraordinariosInternacionales,
      ].filter((m) => m instanceof Map && m.size > 0) // Filtra mapas vacíos
    );

    if (menorFechaHistorico && mayorFechaHistorico) {
      await this.cargarFilaDias(menorFechaHistorico, mayorFechaHistorico);
    } else {
      console.error('Error: No se encontraron fechas válidas.');
    }

    return;
  }

  /********************* */
  /*    FIN FILA DIAS    */
  /********************* */
  async cargarFilaDias(menorFechaHistorico: string, mayorFechaHistorico: string) {
    if (!menorFechaHistorico.trim() || !mayorFechaHistorico.trim()) {
      return;
    }

    this.filaDias.clear();

    const [d1, m1, y1] = menorFechaHistorico.split('-').map(Number);
    const [d2, m2, y2] = mayorFechaHistorico.split('-').map(Number);

    const fechaInicio = new Date(y1, m1 - 1, d1);
    const fechaFin = new Date(y2, m2 - 1, d2);

    for (let fecha = new Date(fechaInicio); fecha <= fechaFin; fecha.setDate(fecha.getDate() + 1)) {
      const day = String(fecha.getDate()).padStart(2, '0');
      const month = String(fecha.getMonth() + 1).padStart(2, '0');
      const year = fecha.getFullYear();

      const fechaFormateada = `${day}-${month}-${year}`;

      if (!this.filaDias.has(fechaFormateada)) {
        this.filaDias.set(fechaFormateada, []);
      }
    }

    // Convertimos las claves del Map a un array para el *ngFor
    this.filaDiasArray = Array.from(this.filaDias.keys());
  }

  filaDiasArray: string[] = [];

  getDiasContinuos(): string[] {
    const fechas = this.listadoEvoluciones.map((evolucion: any) => this.fechaADDMMYY(evolucion.datoPrincipal?.fechaHora)); // Suponiendo que cada evolución tiene una propiedad 'fecha'

    if (fechas.length === 0) {
      return [];
    }

    const diasContinuos: string[] = [];
    const diasContinuos2: string[] = [];
    const [dayInicio, monthInicio, yearInicio] = fechas[0].split('-').map(Number);
    const [dayFin, monthFin, yearFin] = fechas[fechas.length - 1].split('-').map(Number);

    // No restamos 1 al mes, para que el primer día sea el correcto
    const fechaInicio = new Date(yearInicio, monthInicio - 1, dayInicio);
    const fechaFin = new Date(yearFin, monthFin - 1, dayFin);

    // Iterar desde fechaInicio hasta fechaFin
    for (let fecha = new Date(fechaInicio); fecha <= fechaFin; fecha.setDate(fecha.getDate() + 1)) {
      const day = String(fecha.getDate()).padStart(2, '0'); // Asegura que el día tenga dos dígitos
      const month = String(fecha.getMonth() + 1).padStart(2, '0'); // Asegura que el mes tenga dos dígitos
      const year = fecha.getFullYear();

      const fechaFormateadaFinal = `${day}-${month}-${year}`; // Formato final DD-MM-YYYY

      // Solo agregar la fecha si no está en filaEvolucion
      if (!this.filaEvolucion.has(fechaFormateadaFinal)) {
        this.filaEvolucion.set(fechaFormateadaFinal, []); // Añadir la fecha como entrada vacía
      }

      diasContinuos.push(fechaFormateadaFinal);
    }

    return diasContinuos;
  }
  /********************* */
  /*    FIN FILA DIAS    */
  /********************* */

  /********************* */
  /* FILA EVOLUCION  */
  /********************* */
  async cargarFilaEvolucion() {
    this.filaEvolucion.clear();

    if (this.listadoEvoluciones != null && this.listadoEvoluciones.length > 0) {
      this.listadoEvoluciones.sort((a, b) => {
        const fechaA = new Date(a.datoPrincipal?.fechaHora);
        const fechaB = new Date(b.datoPrincipal?.fechaHora);
        return fechaA.getTime() - fechaB.getTime();
      });
      for (const evolucion of this.listadoEvoluciones) {
        const fechaEvolucion: string = this.fechaADDMMYY(evolucion.datoPrincipal?.fechaHora);
        const horaEvolucion: string = this.fechaAHHMM(evolucion.datoPrincipal?.fechaHora);
        const estadoEvolucion: string = evolucion.parametro.estadoIncendio?.descripcion;
        if (estadoEvolucion) {
          this.actualizarFilaEvolucion(fechaEvolucion, horaEvolucion, estadoEvolucion);
        }
      }
    }
  }

  actualizarFilaEvolucion(fechaEvolucion: string, horaEvolucion: string, estadoEvolucion: string) {
    // Verificar si ya existe la fecha en el mapa
    if (this.filaEvolucion.has(fechaEvolucion)) {
      // Si ya existe, obtener el array de entradas para esa fecha
      const entradas = this.filaEvolucion.get(fechaEvolucion) || []; // Si es undefined, se asigna un array vacío

      // Añadir un nuevo estado a las entradas
      entradas.push({ hora: horaEvolucion, estado: estadoEvolucion });

      // Actualizar la entrada en el mapa con el nuevo valor
      this.filaEvolucion.set(fechaEvolucion, entradas);
    } else {
      // Si no existe la fecha, crear una nueva entrada
      this.filaEvolucion.set(fechaEvolucion, [{ hora: horaEvolucion, estado: estadoEvolucion }]);
    }
  }

  getBackgroundColorFilaEvolucion(fecha: string, horaIndex: number): string {
    // Obtener todas las fechas ordenadas
    const fechas = Array.from(this.filaEvolucionDiasCompletos.keys()).sort((a, b) => new Date(a).getTime() - new Date(b).getTime());

    // Obtener todas las entradas del día dado
    const entradas = this.filaEvolucionDiasCompletos.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el rango de horas
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el horaIndex está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          color = this.getColorPorEstadoFilaEvolucion(entrada.estado);
          return color;
        }
      }
    }

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaEvolucionDiasCompletos.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        const ultimaEntrada = entradasAnteriores[entradasAnteriores.length - 1];
        color = this.getColorPorEstadoFilaEvolucion(ultimaEntrada.estado);
        break;
      }
    }

    return color;
  }

  getColorPorEstadoFilaEvolucion(estado: string): string {
    estado = estado.toLowerCase();
    switch (estado) {
      case 'activo': // activo
        return 'red';
      case 'estabilizado':
        return '#FF8000';
      case 'controlado':
        return '#F9D705';
      case 'extinguido':
        return 'green';
      default:
        return 'transparent';
    }
  }

  get filaEvolucionDiasCompletos(): Map<string, { hora: string; estado: string }[]> {
    const completo = new Map<string, { hora: string; estado: string }[]>();

    // Iteramos sobre todas las fechas en filaDias
    for (const fecha of this.filaDias.keys()) {
      // Si la fecha existe en filaNivelSituacionOperativaEquivalente, la usamos
      if (this.filaEvolucion.has(fecha)) {
        completo.set(fecha, this.filaEvolucion.get(fecha)!);
      } else {
        // Si no existe, se agrega con un array vacío
        completo.set(fecha, []);
      }
    }

    return completo;
  }

  getEstadoFilaEvolucion(fecha: string, horaIndex: number): string | null {
    // Obtener todas las fechas ordenadas
    const fechas = Array.from(this.filaEvolucionDiasCompletos.keys()).sort((a, b) => new Date(a).getTime() - new Date(b).getTime());

    // Obtener todas las entradas del día dado
    const entradas = this.filaEvolucionDiasCompletos.get(fecha);

    if (entradas) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el estado correspondiente
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el `horaIndex` está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          return entrada.estado;
        }
      }
    }

    // Si no se encontró un estado en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaEvolucionDiasCompletos.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        return entradasAnteriores[entradasAnteriores.length - 1].estado;
      }
    }

    return null; // Si no hay estado disponible
  }

  /********************* */
  /* FIN FILA EVOLUCION  */
  /********************* */

  /********************************************* */
  /*   Fila NivelSituacionOperativaEquivalente   */
  /********************************************* */
  async cargarFilaNivelSituacionOperativaEquivalente() {
    this.filaNivelSituacionOperativaEquivalente.clear();

    if (this.listadoEvoluciones.length > 0) {
      this.listadoEvoluciones.sort((a, b) => {
        const fechaA = new Date(a.datoPrincipal?.fechaHora);
        const fechaB = new Date(b.datoPrincipal?.fechaHora);
        return fechaA.getTime() - fechaB.getTime();
      });
      for (const evolucion of this.listadoEvoluciones) {
        const fechaEvolucion: string = this.fechaADDMMYY(evolucion.datoPrincipal?.fechaHora);
        const horaEvolucion: string = this.fechaAHHMM(evolucion.datoPrincipal?.fechaHora);
        const nivelSituacionOperativaEquivalenteEvolucion: string = evolucion.parametro?.situacionEquivalente?.descripcion;

        if (nivelSituacionOperativaEquivalenteEvolucion) {
          this.actualizarFilaSituacionOperativaEquivalente(fechaEvolucion, horaEvolucion, nivelSituacionOperativaEquivalenteEvolucion);
        }
      }
    }
  }

  actualizarFilaSituacionOperativaEquivalente(fechaEvolucion: string, horaEvolucion: string, nivelSituacionOperativaEquivalenteEvolucion: string) {
    // Verificar si ya existe la fecha en el mapa
    if (this.filaNivelSituacionOperativaEquivalente.has(fechaEvolucion)) {
      // Si ya existe, obtener el array de entradas para esa fecha
      const entradas = this.filaNivelSituacionOperativaEquivalente.get(fechaEvolucion) || []; // Si es undefined, se asigna un array vacío

      // Añadir un nuevo estado a las entradas
      entradas.push({ hora: horaEvolucion, estado: nivelSituacionOperativaEquivalenteEvolucion });

      // Actualizar la entrada en el mapa con el nuevo valor
      this.filaNivelSituacionOperativaEquivalente.set(fechaEvolucion, entradas);
    } else {
      // Si no existe la fecha, crear una nueva entrada
      this.filaNivelSituacionOperativaEquivalente.set(fechaEvolucion, [{ hora: horaEvolucion, estado: nivelSituacionOperativaEquivalenteEvolucion }]);
    }
  }

  getBackgroundColorFilaNivelSituacionOperativaEquivalente(fecha: string, horaIndex: number): string {
    // Usamos filaNivelSituacionOperativaEquivalenteDiasCompletos para garantizar que todas las fechas estén
    const fechas = Array.from(this.filaNivelSituacionOperativaEquivalenteDiasCompletos.keys());

    // Obtener todas las entradas del día dado
    const entradas = this.filaNivelSituacionOperativaEquivalenteDiasCompletos.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas && entradas.length > 0) {
      // Ordenar las entradas del día por hora
      entradas.sort((a, b) => parseInt(a.hora.split(':')[0], 10) - parseInt(b.hora.split(':')[0], 10));

      // Buscar el color según el `horaIndex`
      for (let i = 0; i < entradas.length; i++) {
        const entrada = entradas[i];
        const horaSolo = parseInt(entrada.hora.split(':')[0], 10);

        if (horaSolo <= horaIndex && (i === entradas.length - 1 || horaIndex < parseInt(entradas[i + 1].hora.split(':')[0], 10))) {
          return this.getColorPorEstadoFilaNivelSituacionOperativaEquivalente(entrada.estado);
        }
      }
    }

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaNivelSituacionOperativaEquivalenteDiasCompletos.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        return this.getColorPorEstadoFilaNivelSituacionOperativaEquivalente(entradasAnteriores[entradasAnteriores.length - 1].estado);
      }
    }

    return color;
  }

  getColorPorEstadoFilaNivelSituacionOperativaEquivalente(estado: string): string {
    switch (estado) {
      case '0':
        return 'lightgreen';
      case '1':
        return 'yellow';
      case '2':
        return 'orange';
      case '3':
        return 'brown';
      default:
        return 'transparent';
    }
  }

  get filaNivelSituacionOperativaEquivalenteDiasCompletos(): Map<string, { hora: string; estado: string }[]> {
    const completo = new Map<string, { hora: string; estado: string }[]>();

    // Iteramos sobre todas las fechas en filaDias
    for (const fecha of this.filaDias.keys()) {
      // Si la fecha existe en filaNivelSituacionOperativaEquivalente, la usamos
      if (this.filaNivelSituacionOperativaEquivalente.has(fecha)) {
        completo.set(fecha, this.filaNivelSituacionOperativaEquivalente.get(fecha)!);
      } else {
        // Si no existe, se agrega con un array vacío
        completo.set(fecha, []);
      }
    }

    return completo;
  }

  /********************************************* */
  /* FIN Fila NivelSituacionOperativaEquivalente */
  /********************************************* */

  /********************************************* */
  /*           Fila ActivacionSistemas           */
  /********************************************* */

  async cargarFilaActivacionSistemas() {
    this.filaActivacionSistemas.clear();

    if (this.listadoActuacionesRelevantes.length > 0) {
      this.listadoActuacionesRelevantes.sort((a, b) => {
        const fechaA = new Date(a.fechaHora);
        const fechaB = new Date(b.fechaHora);
        return fechaA.getTime() - fechaB.getTime();
      });
      for (const actuacionRelevante of this.listadoActuacionesRelevantes) {
        const fechaActivacionSistema: string = this.fechaADDMMYY(actuacionRelevante.activacionSistemas[0]?.fechaHoraActivacion);
        const horaActivacionSistema: string = this.fechaAHHMM(actuacionRelevante.activacionSistemas[0]?.fechaHoraActivacion);
        const estadoSistema: string = actuacionRelevante.activacionSistemas[0]?.id?.toString();
        if (estadoSistema) {
          this.actualizarFilaActivacionSistemas(fechaActivacionSistema, horaActivacionSistema, estadoSistema);
        }
      }
    }
  }

  actualizarFilaActivacionSistemas(fechaActivacionSistema: string, horaActivacionSistema: string, estadoSistema: string) {
    // Verificar si ya existe la fecha en el mapa
    if (this.filaActivacionSistemas.has(fechaActivacionSistema)) {
      // Si ya existe, obtener el array de entradas para esa fecha
      const entradas = this.filaActivacionSistemas.get(fechaActivacionSistema) || []; // Si es undefined, se asigna un array vacío

      // Añadir un nuevo estado a las entradas
      entradas.push({ hora: horaActivacionSistema, estado: estadoSistema });

      // Actualizar la entrada en el mapa con el nuevo valor
      this.filaActivacionSistemas.set(fechaActivacionSistema, entradas);
    } else {
      // Si no existe la fecha, crear una nueva entrada
      this.filaActivacionSistemas.set(fechaActivacionSistema, [{ hora: horaActivacionSistema, estado: estadoSistema }]);
    }
  }

  getBackgroundIconFilaActivacionSistemas(fecha: string, horaIndex: number): string {
    const entradas = this.filaActivacionSistemasDiasCompletos.get(fecha);
    let iconUrl = ''; // Valor predeterminado (sin icono)

    if (entradas && entradas.length > 0) {
      // Buscar una coincidencia exacta de hora
      for (const entrada of entradas) {
        const [horaExacta] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        if (horaExacta === horaIndex) {
          iconUrl = this.getIconUrlFilaActivacionSistemasPorEstado(entrada.estado);
          return `url('${iconUrl}')`;
        }
      }
    }

    return 'none';
  }

  getIconUrlFilaActivacionSistemasPorEstado(estado: string): string {
    //estado = estado.toLowerCase();
    switch (estado) {
      case '0':
        return '/assets/assets/img/satelite.png';
      case '1':
        return '/assets/assets/img/satelite.png';
      case '2':
        return '/assets/assets/img/satelite.png';
      case '3':
        return '/assets/assets/img/satelite.png';
      default:
        return '/assets/img/logo-color.png';
    }
  }

  get filaActivacionSistemasDiasCompletos(): Map<string, { hora: string; estado: string }[]> {
    const completo = new Map<string, { hora: string; estado: string }[]>();
    // Iteramos sobre todas las fechas en filaDias
    for (const fecha of this.filaDias.keys()) {
      if (this.filaActivacionSistemas.has(fecha)) {
        completo.set(fecha, this.filaActivacionSistemas.get(fecha)!);
      } else {
        // Si no existe, se agrega con un array vacío
        completo.set(fecha, []);
      }
    }

    return completo;
  }

  /********************************************* */
  /* Fin Fila ActivacionSistemas */
  /********************************************* */

  /*************************/
  /* Fila Afectaciones */
  /*************************/
  async cargarFilaAfectaciones() {
    this.filaAfectaciones.clear();
    /*
    this.filaAfectaciones.set('29-01-2025', [
      { hora: '02:15', estado: 'PERSONAS' },
      { hora: '16:20', estado: 'CARRETERAS' },
    ]);
    this.filaAfectaciones.set('28-01-2025', [{ hora: '12:15', estado: 'MEDIOAMBIENTE' }]);
    */

    if (this.listadoEvoluciones == null || this.listadoEvoluciones.length == 0) {
      return;
    }

    for (const evolucion of this.listadoEvoluciones) {
      const afectacionesEvolucion: any = await this.impactosEvolucionesService.getImpactosPorEvolucion(evolucion.id);
      if (afectacionesEvolucion != null && afectacionesEvolucion.length > 0) {
        for (const afectacion of afectacionesEvolucion) {
          //console.log(JSON.stringify(afectacion, null, 2));
          //alert(afectacion.impactoClasificado.grupoImpacto);

          const fechaAfectacion: string = this.fechaADDMMYY(afectacion.fechaHora);
          const horaAfectacion: string = this.fechaAHHMM(afectacion.fechaHora);
          const estadoAfectacion: string = afectacion.impactoClasificado?.grupoImpacto;
          if (estadoAfectacion) {
            this.actualizarFilaAfectaciones(fechaAfectacion, horaAfectacion, estadoAfectacion);
          }
        }
      }
    }
  }

  actualizarFilaAfectaciones(fechaAfectacion: string, horaAfectacion: string, estadoAfectacion: string) {
    // Verificar si ya existe la fecha en el mapa
    if (this.filaAfectaciones.has(fechaAfectacion)) {
      // Si ya existe, obtener el array de entradas para esa fecha
      const entradas = this.filaAfectaciones.get(fechaAfectacion) || []; // Si es undefined, se asigna un array vacío

      // Añadir un nuevo estado a las entradas
      entradas.push({ hora: horaAfectacion, estado: estadoAfectacion });

      // Actualizar la entrada en el mapa con el nuevo valor
      this.filaAfectaciones.set(fechaAfectacion, entradas);
    } else {
      // Si no existe la fecha, crear una nueva entrada
      this.filaAfectaciones.set(fechaAfectacion, [{ hora: horaAfectacion, estado: estadoAfectacion }]);
    }
  }

  getBackgroundIconFilaAfectaciones(fecha: string, horaIndex: number): string {
    // Obtener todas las entradas del día dado desde filaAfectacionesDiasCompletos
    const entradas = this.filaAfectacionesDiasCompletos.get(fecha);
    let iconUrl = ''; // Valor predeterminado (sin icono)

    if (entradas && entradas.length > 0) {
      // Buscar una coincidencia exacta de hora
      for (const entrada of entradas) {
        const [horaExacta] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        if (horaExacta === horaIndex) {
          iconUrl = this.getIconUrlFilaAfectacionesPorEstado(entrada.estado);
          return `url('${iconUrl}')`;
        }
      }
    }

    return 'none';
  }
  //

  getIconUrlFilaAfectacionesPorEstado(estado: string): string {
    estado = estado.toLowerCase();
    switch (estado) {
      case 'personas':
        return '/assets/assets/img/persona.png';
      case 'carreteras':
        return '/assets/assets/img/vialidad.png';
      case 'medioambiente':
        return '/assets/assets/img/hoja.png';
      case 'varios':
        return '/assets/assets/img/varios_3.png';
      default:
        return '/assets/img/logo-color.png';
    }
  }

  get filaAfectacionesDiasCompletos(): Map<string, { hora: string; estado: string }[]> {
    const completo = new Map<string, { hora: string; estado: string }[]>();
    // Iteramos sobre todas las fechas en filaDias
    for (const fecha of this.filaDias.keys()) {
      if (this.filaAfectaciones.has(fecha)) {
        completo.set(fecha, this.filaAfectaciones.get(fecha)!);
      } else {
        // Si no existe, se agrega con un array vacío
        completo.set(fecha, []);
      }
    }

    return completo;
  }
  /*************************/
  /* Fin Fila Afectaciones */
  /*************************/

  /************************************** */
  /*   Fila Medios extinción ordinarios   */
  /************************************** */
  async cargarFilaMediosExtincionOrdinarios() {
    this.filaMediosExtincionOrdinarios.set('14-02-2025', [
      { hora: '12:15', estado: 'AVION' },
      { hora: '16:20', estado: 'BRIGADAS' },
    ]);
    //this.filaMediosExtincionOrdinarios.set('29-01-2025', [{ hora: '12:15', estado: 'BRIGADAS' }]);
  }

  getBackgroundColorFilaMediosExtincionOrdinarios(fecha: string, horaIndex: number): string {
    // Obtener todas las entradas del día dado desde filaMediosExtincionOrdinariosDiasCompletos
    const entradas = this.filaMediosExtincionOrdinariosDiasCompletos.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas && entradas.length > 0) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el rango de horas
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el horaIndex está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          color = '#00BFBF'; // Color específico
          return color;
        }
      }
    }

    // Obtener todas las fechas ordenadas
    const fechas = Array.from(this.filaMediosExtincionOrdinariosDiasCompletos.keys()).sort((a, b) => {
      const [dayA, monthA, yearA] = a.split('-').map(Number);
      const [dayB, monthB, yearB] = b.split('-').map(Number);
      const dateA = new Date(yearA, monthA - 1, dayA);
      const dateB = new Date(yearB, monthB - 1, dayB);
      return dateA.getTime() - dateB.getTime();
    });

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaMediosExtincionOrdinariosDiasCompletos.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        color = '#00BFBF'; // Color específico
        break;
      }
    }

    return color;
  }
  //

  get filaMediosExtincionOrdinariosDiasCompletos(): Map<string, { hora: string; estado: string }[]> {
    const completo = new Map<string, { hora: string; estado: string }[]>();

    // Iteramos sobre todas las fechas en filaDias
    for (const fecha of this.filaDias.keys()) {
      if (this.filaMediosExtincionOrdinarios.has(fecha)) {
        completo.set(fecha, this.filaMediosExtincionOrdinarios.get(fecha)!);
      } else {
        // Si no existe, se agrega con un array vacío
        completo.set(fecha, []);
      }
    }

    return completo;
  }

  /************************************** */
  /* Fin Fila Medios extinción ordinarios */
  /************************************** */

  /****************************************************** */
  /* Fila Medios extinción extraordinarios nacionales */
  /****************************************************** */
  async cargarFilaMediosExtincionExtraordinariosNacionales() {
    /*
    this.filaMediosExtincionExtraordinariosNacionales.set('03-02-2025', [
      { hora: '08:15', estado: 'AVION' },
      { hora: '16:20', estado: 'BRIGADAS' },
    ]);
    this.filaMediosExtincionExtraordinariosNacionales.set('04-02-2025', [{ hora: '14:15', estado: 'BRIGADAS' }]);
    */

    this.filaMediosExtincionExtraordinariosNacionales.clear();

    if (this.listadoActuacionesRelevantes.length > 0) {
      this.listadoActuacionesRelevantes.sort((a, b) => {
        const fechaA = new Date(a.fechaHora);
        const fechaB = new Date(b.fechaHora);
        return fechaA.getTime() - fechaB.getTime();
      });

      for (const actuacionRelevante of this.listadoActuacionesRelevantes) {
        if (actuacionRelevante.movilizacionMedios != null && actuacionRelevante.movilizacionMedios.length > 0) {
          for (const movilizacionMedios of actuacionRelevante.movilizacionMedios) {
            if (movilizacionMedios.pasos != null && movilizacionMedios.pasos.length > 0) {
              for (const paso of movilizacionMedios.pasos) {
                //console.log(JSON.stringify(actuacionRelevante, null, 2));
                const fechaGestionMedioExtraordinarioNacional: string = this.fechaADDMMYY(
                  paso.solicitudMedio?.fechaHoraSolicitud ||
                    paso.tramitacionMedio?.fechaHoraTramitacion ||
                    paso.ofrecimientoMedio?.fechaHoraOfrecimiento ||
                    paso.aportacionMedio?.fechaHoraAportacion ||
                    paso.despliegueMedio?.fechaHoraDespliegue ||
                    paso.finIntervencionMedio?.fechaHoraInicioIntervencion
                );
                const horaGestionMedioExtraordinarioNacional: string = this.fechaAHHMM(
                  paso.solicitudMedio?.fechaHoraSolicitud ||
                    paso.tramitacionMedio?.fechaHoraTramitacion ||
                    paso.ofrecimientoMedio?.fechaHoraOfrecimiento ||
                    paso.aportacionMedio?.fechaHoraAportacion ||
                    paso.despliegueMedio?.fechaHoraDespliegue ||
                    paso.finIntervencionMedio?.fechaHoraInicioIntervencion
                );
                const estadoGestionMedioExtraordinarioNacional: string = paso.pasoMovilizacion?.descripcion;
                if (estadoGestionMedioExtraordinarioNacional) {
                  this.actualizarFilaMediosExtincionExtraordinariosNacionales(
                    fechaGestionMedioExtraordinarioNacional,
                    horaGestionMedioExtraordinarioNacional,
                    estadoGestionMedioExtraordinarioNacional
                  );
                }
              }
            }
          }
        }
      }
    }
  }

  actualizarFilaMediosExtincionExtraordinariosNacionales(
    fechaGestionMedioExtraordinarioNacional: string,
    horaGestionMedioExtraordinarioNacional: string,
    estadoGestionMedioExtraordinarioNacional: string
  ) {
    // Verificar si ya existe la fecha en el mapa
    if (this.filaMediosExtincionExtraordinariosNacionales.has(fechaGestionMedioExtraordinarioNacional)) {
      // Si ya existe, obtener el array de entradas para esa fecha
      const entradas = this.filaMediosExtincionExtraordinariosNacionales.get(fechaGestionMedioExtraordinarioNacional) || []; // Si es undefined, se asigna un array vacío

      // Añadir un nuevo estado a las entradas
      entradas.push({ hora: horaGestionMedioExtraordinarioNacional, estado: estadoGestionMedioExtraordinarioNacional });

      // Actualizar la entrada en el mapa con el nuevo valor
      this.filaMediosExtincionExtraordinariosNacionales.set(fechaGestionMedioExtraordinarioNacional, entradas);
    } else {
      // Si no existe la fecha, crear una nueva entrada
      this.filaMediosExtincionExtraordinariosNacionales.set(fechaGestionMedioExtraordinarioNacional, [
        { hora: horaGestionMedioExtraordinarioNacional, estado: estadoGestionMedioExtraordinarioNacional },
      ]);
    }
  }

  getEstiloFilaMediosExtincionExtraordinariosNacionales(fecha: string, horaIndex: number): { [key: string]: string } {
    // Obtener todas las fechas ordenadas desde el getter
    const fechas = Array.from(this.filaMediosExtincionExtraordinariosNacionalesDiasCompletos.keys()).sort((a, b) => {
      const [dayA, monthA, yearA] = a.split('-').map(Number);
      const [dayB, monthB, yearB] = b.split('-').map(Number);
      const dateA = new Date(yearA, monthA - 1, dayA);
      const dateB = new Date(yearB, monthB - 1, dayB);
      return dateA.getTime() - dateB.getTime();
    });

    // Obtener todas las entradas del día dado
    const entradas = this.filaMediosExtincionExtraordinariosNacionalesDiasCompletos.get(fecha);

    let estilo: { [key: string]: string } = {
      'background-color': 'transparent',
      position: 'absolute',
      top: '8px',
      left: '0',
      width: '100%',
      height: '30%',
    };

    let fondoAportacionAplicado = false; // Variable para saber si ya se aplicó el fondo "Aportación"

    if (entradas && entradas.length > 0) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el rango de horas
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaExacta] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si encontramos un evento "Aportación", aplicamos el fondo
        if (entrada.estado.toLowerCase() === 'aportación' || (entrada.estado.toLowerCase() === 'despliegue' && !fondoAportacionAplicado)) {
          estilo = { 'background-color': '#00BFBF', position: 'absolute', top: '8px', left: '0', width: '100%', height: '30%' };
          fondoAportacionAplicado = true; // Marcamos que el fondo se ha aplicado
        }

        // Si el fondo "Aportación" ha sido aplicado, lo mantenemos activo hasta encontrar "fin de intervención"
        if (fondoAportacionAplicado) {
          // Si encontramos un evento "Fin de intervención", terminamos de aplicar el fondo
          if (entrada.estado.toLowerCase() === 'fin de intervención') {
            fondoAportacionAplicado = false; // Se desactiva el fondo
            break; // Salimos del bucle
          }
          continue; // Si el fondo ya está aplicado, lo mantenemos hasta encontrar el "fin de intervención"
        }

        // Si el horaExacta es igual a horaIndex
        if (horaExacta === horaIndex) {
          estilo = this.getEstiloPorEstadoFilaMediosExtincionExtraordinariosNacionales(entrada.estado);

          //
          // **Solo pintar el icono si hay una aportación**
          if (!this.tieneAportacion(this.filaMediosExtincionExtraordinariosNacionales)) {
            estilo = { 'background-color': 'transparent', position: 'absolute', top: '8px', left: '0', width: '100%', height: '30%' };
          }
          //

          return estilo;
        } else if (
          horaExacta <= horaIndex &&
          (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))
        ) {
          return estilo;
        }
      }

      // Si no se encuentra un evento "fin de intervención", mantenemos el fondo hasta el final del día
      if (fondoAportacionAplicado) {
        return estilo; // Retorna el estilo con fondo aún aplicado
      }
    }

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaMediosExtincionExtraordinariosNacionalesDiasCompletos.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        const ultimaEntrada = entradasAnteriores[entradasAnteriores.length - 1];
        if (ultimaEntrada.estado.toLowerCase() === 'aportación') {
          estilo = { 'background-color': '#00BFBF', position: 'absolute', top: '8px', left: '0', width: '100%', height: '30%' };
          break; // Salimos del bucle si encontramos "aportación" en días anteriores
        }
      }
    }

    return estilo;
  }

  tieneAportacion(filaMediosExtincionExtraordinariosNacionales: Map<string, { hora: string; estado: string }[]>): boolean {
    for (const entradas of filaMediosExtincionExtraordinariosNacionales.values()) {
      if (entradas.some((entrada) => entrada.estado.toLowerCase() === 'aportación')) {
        return true;
      }
    }
    return false;
  }

  // FIN TEST

  get filaMediosExtincionExtraordinariosNacionalesDiasCompletos(): Map<string, { hora: string; estado: string }[]> {
    const completo = new Map<string, { hora: string; estado: string }[]>();

    // Iteramos sobre todas las fechas en filaDias
    for (const fecha of this.filaDias.keys()) {
      if (this.filaMediosExtincionExtraordinariosNacionales.has(fecha)) {
        completo.set(fecha, this.filaMediosExtincionExtraordinariosNacionales.get(fecha)!);
      } else {
        // Si no existe, se agrega con un array vacío
        completo.set(fecha, []);
      }
    }

    return completo;
  }

  getEstiloPorEstadoFilaMediosExtincionExtraordinariosNacionales(estado: string): { [key: string]: string } {
    estado = estado.toLowerCase();
    switch (estado) {
      case 'solicitud':
        return {
          'background-image': 'url(/assets/img/movilizaciones/m-solicitud.svg)',
          'background-size': 'contain',
          'background-repeat': 'no-repeat',
          position: 'absolute',
          top: '2px',
          left: '0',
          width: '100%',
          height: '100%',
        };
      /*
      case 'tramitación':
        return {
          'background-image': 'url(/assets/img/movilizaciones/m-tramitacion.svg)',
          'background-size': 'contain',
          'background-repeat': 'no-repeat',
          position: 'absolute',
          top: '2px',
          left: '0',
          width: '100%',
          height: '100%',
        };
        */
      case 'ofrecimiento':
        return {
          'background-image': 'url(/assets/img/movilizaciones/m-ofrecimiento.svg)',
          'background-size': 'contain',
          'background-repeat': 'no-repeat',
          position: 'absolute',
          top: '2px',
          left: '0',
          width: '100%',
          height: '100%',
        };
      case 'aportación':
        return { 'background-color': '#00BFBF', position: 'absolute', top: '8px', left: '0', width: '100%', height: '30%' };
      case 'despliegue':
        return { 'background-color': '#00BFBF', position: 'absolute', top: '8px', left: '0', width: '100%', height: '30%' };
      case 'fin de intervención':
        return { 'background-color': 'transparent', position: 'absolute', top: '8px', left: '0', width: '100%', height: '30%' };
      default:
        return { 'background-color': 'transparent', position: 'absolute', top: '8px', left: '0', width: '100%', height: '30%' };
    }
  }

  /****************************************************** */
  /* Fin Fila Medios extinción extraordinarios nacionales */
  /****************************************************** */

  /*************************************************************/
  /*   Fila Medios extinción extraordinarios internacionales   */
  /*************************************************************/
  async cargarFilaEMediosExtincionExtraordinariosInternacionales() {
    this.filaMediosExtincionExtraordinariosInternacionales.set('14-02-2025', [
      { hora: '09:15', estado: 'AVION' },
      { hora: '16:20', estado: 'BRIGADAS' },
    ]);
    this.filaMediosExtincionExtraordinariosInternacionales.set('15-02-2025', [{ hora: '04:15', estado: 'BRIGADAS' }]);
  }

  getBackgroundColorFilaMediosExtincionExtraordinariosInternacionales(fecha: string, horaIndex: number): string {
    // Obtener todas las fechas ordenadas desde el getter
    const fechas = Array.from(this.filaMediosExtincionOrdinariosInternacionalesDiasCompletos.keys()).sort((a, b) => {
      const [dayA, monthA, yearA] = a.split('-').map(Number);
      const [dayB, monthB, yearB] = b.split('-').map(Number);
      const dateA = new Date(yearA, monthA - 1, dayA);
      const dateB = new Date(yearB, monthB - 1, dayB);
      return dateA.getTime() - dateB.getTime();
    });

    // Obtener todas las entradas del día dado
    const entradas = this.filaMediosExtincionOrdinariosInternacionalesDiasCompletos.get(fecha);

    let color = 'transparent'; // Color predeterminado

    if (entradas && entradas.length > 0) {
      // Ordenar las entradas por hora
      const entradasOrdenadas = entradas.sort((a, b) => {
        const [horaA] = a.hora.split(':').map((num) => parseInt(num, 10));
        const [horaB] = b.hora.split(':').map((num) => parseInt(num, 10));
        return horaA - horaB;
      });

      // Recorrer las entradas del día para buscar el rango de horas
      for (let i = 0; i < entradasOrdenadas.length; i++) {
        const entrada = entradasOrdenadas[i];
        const [horaSolo] = entrada.hora.split(':').map((num) => parseInt(num, 10));

        // Si el horaIndex está en el rango actual
        if (horaSolo <= horaIndex && (i === entradasOrdenadas.length - 1 || horaIndex < parseInt(entradasOrdenadas[i + 1].hora.split(':')[0], 10))) {
          color = '#00BFBF'; // Color específico
          return color;
        }
      }
    }

    // Si no se encontró un color en el día actual, buscar en días anteriores
    for (let i = fechas.indexOf(fecha) - 1; i >= 0; i--) {
      const fechaAnterior = fechas[i];
      const entradasAnteriores = this.filaMediosExtincionOrdinariosInternacionalesDiasCompletos.get(fechaAnterior);

      if (entradasAnteriores && entradasAnteriores.length > 0) {
        // Obtener la última entrada del día anterior
        color = '#00BFBF'; // Color específico
        break;
      }
    }

    return color;
  }
  //

  get filaMediosExtincionOrdinariosInternacionalesDiasCompletos(): Map<string, { hora: string; estado: string }[]> {
    const completo = new Map<string, { hora: string; estado: string }[]>();

    // Iteramos sobre todas las fechas en filaDias
    for (const fecha of this.filaDias.keys()) {
      if (this.filaMediosExtincionExtraordinariosInternacionales.has(fecha)) {
        completo.set(fecha, this.filaMediosExtincionExtraordinariosInternacionales.get(fecha)!);
      } else {
        // Si no existe, se agrega con un array vacío
        completo.set(fecha, []);
      }
    }

    return completo;
  }

  /*************************************************************/
  /* Fin Fila Medios extinción extraordinarios internacionales */
  /*************************************************************/

  /**********************************/
  /*      Funciones auxiliares      */
  /**********************************/
  fechaADDMMYY(fechaString: string): string {
    const fecha = new Date(fechaString);
    const dia = String(fecha.getDate()).padStart(2, '0'); // Asegura que el día tenga dos dígitos
    const mes = String(fecha.getMonth() + 1).padStart(2, '0'); // Los meses son 0-indexed, por eso sumamos 1
    const anio = fecha.getFullYear();
    const fechaFormateada = `${dia}-${mes}-${anio}`;
    return fechaFormateada;
  }

  fechaAHHMM(fechaString: string): string {
    const fecha = new Date(fechaString);

    const horas = String(fecha.getHours()).padStart(2, '0');
    const minutos = String(fecha.getMinutes()).padStart(2, '0');

    const fechaFormateada = `${horas}:${minutos}`;
    return fechaFormateada;
  }

  obtenerMenorFechaHistorico(mapas: Map<string, { hora: string; estado: string }[]>[]): string | null {
    let menor: Date | null = null;

    for (const mapa of mapas) {
      for (const clave of mapa.keys()) {
        const partes = clave.split('-'); // Suponiendo formato DD-MM-YYYY
        if (partes.length !== 3) continue;

        const fecha = new Date(
          Number(partes[2]), // Año
          Number(partes[1]) - 1, // Mes (base 0 en JavaScript)
          Number(partes[0]) // Día
        );

        if (!menor || fecha < menor) {
          menor = fecha;
        }
      }
    }

    if (!menor) return null;

    // Formatear salida a DD-MM-YYYY
    return menor.getDate().toString().padStart(2, '0') + '-' + (menor.getMonth() + 1).toString().padStart(2, '0') + '-' + menor.getFullYear();
  }

  obtenerMayorFechaHistorico(mapas: Map<string, { hora: string; estado: string }[]>[]): string | null {
    let mayor: Date | null = null;

    for (const mapa of mapas) {
      for (const clave of mapa.keys()) {
        const partes = clave.split('-'); // Suponiendo formato DD-MM-YYYY
        if (partes.length !== 3) continue;

        const fecha = new Date(
          Number(partes[2]), // Año
          Number(partes[1]) - 1, // Mes (base 0 en JavaScript)
          Number(partes[0]) // Día
        );

        if (!mayor || fecha > mayor) {
          mayor = fecha;
        }
      }
    }

    if (!mayor) return null;

    // Formatear salida a DD-MM-YYYY
    return mayor.getDate().toString().padStart(2, '0') + '-' + (mayor.getMonth() + 1).toString().padStart(2, '0') + '-' + mayor.getFullYear();
  }

  toggleVistaTabla() {
    const tablaHistorico = document.getElementById('tablaHistorico');
    tablaHistorico?.classList.toggle('vista-general');
  }
}
