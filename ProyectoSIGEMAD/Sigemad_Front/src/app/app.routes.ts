import { Routes } from '@angular/router';
import { CommentsComponent } from './pages/comments/comments.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { FireEditComponent } from './pages/fire/fire-edit/fire-edit.component';
import { FireComponent } from './pages/fire/fire.component';
import { Login } from './pages/login/login.component';
import { LayoutBaseComponent } from './shared/layouts/layout-base.component';
import { OpePeriodosComponent } from './pages/ope/administracion/periodos/ope-periodos.component';
import { OpePuertosComponent } from './pages/ope/administracion/puertos/ope-puertos.component';
import { OpeLineasMaritimasComponent } from './pages/ope/administracion/lineas-maritimas/ope-lineas-maritimas.component';
import { OpeFronterasComponent } from './pages/ope/administracion/fronteras/ope-fronteras.component';
import { OpeAreasDescansoComponent } from './pages/ope/administracion/areas-descanso/ope-areas-descanso.component';
import { OpePuntosControlCarreterasComponent } from './pages/ope/administracion/puntos-control-carreteras/ope-puntos-control-carreteras.component';
import { OpeDatosFronterasComponent } from './pages/ope/datos/fronteras/ope-datos-fronteras.component';
import { OpeLayoutComponent } from '@shared/layouts/ope/ope-layout.component';
import { OpeComponent } from './pages/ope/ope.component';
import { OpeDatosEmbarquesDiariosComponent } from './pages/ope/datos/embarques-diarios/ope-datos-embarques-diarios.component';
import { OpeDatosAsistenciasComponent } from './pages/ope/datos/asistencias/ope-datos-asistencias.component';
import { OpeLogsComponent } from './pages/ope/administracion/logs/ope-logs.component';
import { FireAuditoriaComponent } from './pages/fire/fire-edit/fire-auditoria/fire-auditoria.component';
import { OpeCatalogosComponent } from './pages/ope/administracion/catalogos/ope-catalogos.component';
import { OpePorcentajesOcupacionAreasEstacionamientoComponent } from './pages/ope/administracion/porcents-ocup-areas-est/ope-porcentajes-ocupacion-areas-estacionamiento.component';
import { CatalogsComponent } from './pages/catalogs/catalogs.component';
import { OpeAreasEstacionamientoComponent } from './pages/ope/administracion/areas-estacionamiento/ope-areas-estacionamiento.component';

import { UserListComponent } from './pages/admin/users/user-list/user-list.component';
import { UserFormComponent } from './pages/admin/users/user-form/user-form.component';

export const routes: Routes = [
  { path: 'login', component: Login },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: '',
    component: LayoutBaseComponent,
    children: [
      { path: 'dashboard', component: DashboardComponent },
      {
        path: 'fire',
        component: FireComponent,
        data: { breadcrumb: 'Incendio forestal' },
        children: [{ path: 'fire-national-edit/:id', component: FireEditComponent, data: { breadcrumb: 'Panel de seguimiento' } }],
      },
      {
        path: 'fire',
        component: FireComponent,
        data: { breadcrumb: 'Incendio forestal' },
        children: [
          {
            path: 'fire-national-edit/:id',
            component: FireEditComponent,
            data: { breadcrumb: 'Panel de seguimiento' },
          },
          {
            path: 'fire-auditoria/:id',
            component: FireAuditoriaComponent,
            data: { breadcrumb: 'Resumen' },
          },
        ],
      },

      { path: 'earthquakes', component: CommentsComponent, data: { breadcrumb: 'earthquakes' } },
      { path: 'adverse-weather', component: CommentsComponent, data: { breadcrumb: 'adverse-weather' } },
      { path: 'volcanic-phenomena', component: CommentsComponent, data: { breadcrumb: 'volcanic-phenomena' } },
      { path: 'floods', component: CommentsComponent, data: { breadcrumb: 'floods' } },
      { path: 'chemical-risk', component: CommentsComponent, data: { breadcrumb: 'chemical-risk' } },
      { path: 'dangerous-goods', component: CommentsComponent, data: { breadcrumb: 'dangerous-goods' } },
      { path: 'nuclear-radiological-risk', component: CommentsComponent, data: { breadcrumb: 'nuclear-radiological-risk' } },
      { path: 'other-risks', component: CommentsComponent, data: { breadcrumb: 'other-risks' } },
      // PCD
      //
      {
        path: 'ope',
        component: OpeLayoutComponent,
        children: [
          //
          // OPE - PANTALLA PRINCIPAL
          { path: '', component: OpeComponent },
          // OPE - ADMINISTRACIÓN
          {
            path: 'administracion',
            data: { breadcrumb: 'Administración OPE' },
            children: [
              { path: '', redirectTo: '/ope', pathMatch: 'full' },
              { path: 'periodos', component: OpePeriodosComponent, data: { breadcrumb: 'Periodos' } },
              { path: 'puertos', component: OpePuertosComponent, data: { breadcrumb: 'Puertos' } },
              {
                path: 'lineas-maritimas',
                component: OpeLineasMaritimasComponent,
                data: { breadcrumb: 'Líneas marítimas' },
              },
              { path: 'fronteras', component: OpeFronterasComponent, data: { breadcrumb: 'Fronteras' } },

              {
                path: 'puntos-control-carreteras',
                component: OpePuntosControlCarreterasComponent,
                data: { breadcrumb: 'Puntos de Control en Carreteras' },
              },
              {
                path: 'areas-descanso',
                component: OpeAreasDescansoComponent,
                data: { breadcrumb: 'Áreas de descanso y puntos de información en carreteras' },
              },
              { path: 'areas-estacionamiento', component: OpeAreasEstacionamientoComponent, data: { breadcrumb: 'Áreas de estacionamiento' } },
              {
                path: 'porcentajes-ocupacion-areas-estacionamiento',
                component: OpePorcentajesOcupacionAreasEstacionamientoComponent,
                data: { breadcrumb: 'Porcentajes de ocupación de áreas de estacionamiento' },
              },
              /*
              {
                path: 'catalogo',
                component: OpeCatalogosComponent,
                data: { breadcrumb: 'Catálogo' },
              },
              {
                path: 'log',
                component: OpeLogsComponent,
                data: { breadcrumb: 'Log' },
              },
              */
              //
              { path: 'catalogo/:idTablaMaestraGrupo', component: CatalogsComponent, data: { breadcrumb: 'Catálogo' } },
              { path: 'log/:idTablaMaestraGrupo', component: CatalogsComponent, data: { breadcrumb: 'Log' } },
              { path: 'historico-sige-2/:idTablaMaestraGrupo', component: CatalogsComponent, data: { breadcrumb: 'Histórico SIGE2' } },
              //
            ],
          },
          // OPE - DATOS
          {
            path: 'datos',
            data: { breadcrumb: 'Datos' },
            children: [
              { path: '', redirectTo: '/ope', pathMatch: 'full' },

              {
                path: 'embarques-diarios',
                component: OpeDatosEmbarquesDiariosComponent,
                data: { breadcrumb: 'Embarques diarios' },
              },
              {
                path: 'asistencias',
                component: OpeDatosAsistenciasComponent,
                data: { breadcrumb: 'Asistencias' },
              },
              {
                path: 'fronteras',
                component: OpeDatosFronterasComponent,
                data: { breadcrumb: 'Fronteras' },
              },
              {
                path: 'afluencia-puntos-control-carreteras',
                component: OpePeriodosComponent,
                data: { breadcrumb: 'Afluencia a puntos de control en carreteras' },
              },
              {
                path: 'areas-descanso',
                component: OpePeriodosComponent,
                data: { breadcrumb: 'Áreas de descanso y puntos de información en carreteras' },
              },
              {
                path: 'areas-estacionamiento',
                component: OpePeriodosComponent,
                data: { breadcrumb: 'Ocupación de áreas de estacionamiento' },
              },
            ],
          },
          // OPE - APBA
          {
            path: 'ope-apba-entrada-vehiculos-puertos',
            component: OpePeriodosComponent,
            data: { breadcrumb: 'Entrada de vehículos en puertos APBA. Datos' },
          },
          {
            path: 'ope-apba-embarques-vehiculos-intervalos-horarios',
            component: OpePeriodosComponent,
            data: { breadcrumb: 'Embarques de vehículos en APBA por intervalos horarios. Datos' },
          },
          // OPE - PLANIFICACIÓN
          {
            path: 'ope-planificacion-plan-flota',
            component: OpePeriodosComponent,
            data: { breadcrumb: 'Plan de flota' },
          },
          {
            path: 'ope-planificacion-participantes-age',
            component: OpePeriodosComponent,
            data: { breadcrumb: '	Participantes AGE' },
          },
          // OPE - INCIDENCIAS
          {
            path: 'ope-incidencias-datos-inicio',
            component: OpePeriodosComponent,
            data: { breadcrumb: 'Incidencias. Datos de inicio' },
          },
          // OPE - INFORMES
          {
            path: 'ope-informes-prueba',
            component: OpePeriodosComponent,
            data: { breadcrumb: 'Informe de prueba' },
          },
        ],
      },
      // FIN PCD

      { path: 'documentation', component: CommentsComponent, data: { breadcrumb: 'documentation' } },
      { path: 'incidents', component: CommentsComponent, data: { breadcrumb: 'incidents' } },
      { path: 'config', component: CommentsComponent, data: { breadcrumb: 'config' } },
      { path: 'catalogs', component: CatalogsComponent, data: { breadcrumb: 'catalogs' } },
      { path: 'users', component: UserListComponent, data: { breadcrumb: 'Gestión de usuarios' } },
      { path: 'users/:id', component: UserFormComponent, data: { breadcrumb: 'Edición/Creación usuario' } },
      { path: 'search', component: CommentsComponent, data: { breadcrumb: 'search' } },
      { path: 'episodes', component: CommentsComponent, data: { breadcrumb: 'episodes' } },
    ],
  },
  { path: '**', redirectTo: 'login' },
];
