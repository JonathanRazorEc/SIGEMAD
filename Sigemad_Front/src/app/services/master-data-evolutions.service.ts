import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { InputOutput } from '../types/input-output.type';
import { Media } from '../types/media.type';
import { OriginDestination } from '../types/origin-destination.type';
import { FireStatus } from '../types/fire-status.type';
import { SituationsEquivalent } from '../types/situations-equivalent.type';
import { TypesPlans } from '../types/types-plans.type';
import { SituationPlan } from '../types/situation-plan.type';

 
@Injectable({
  providedIn: 'root'
})
export class MasterDataEvolutionsService {

  private http = inject(HttpClient);
  
    getInputOutput() {
      const endpoint = '/entradas-salidas';
      return firstValueFrom(this.http.get<InputOutput[]>(endpoint).pipe((response) => response));
    }

    getMedia() {
      const endpoint = '/medios';
      return firstValueFrom(this.http.get<Media[]>(endpoint).pipe((response) => response));
    }

    getOriginDestination() {
      const endpoint = '/procedencias-destinos';
      return firstValueFrom(this.http.get<OriginDestination[]>(endpoint).pipe((response) => response));
    }

    getFireStatus() {
      const endpoint = '/estados-incendios';
      return firstValueFrom(this.http.get<FireStatus[]>(endpoint).pipe((response) => response));
    }

    getSituationEquivalent() {
      const endpoint = '/situaciones-equivalentes';
      return firstValueFrom(this.http.get<SituationsEquivalent[]>(endpoint).pipe((response) => response));
    }

    getTypesPlans(idCcaa:  number | string) {
      const endpoint = `/planes-emergencias?IdTipoRiesgo=15&IdCcaa=${idCcaa}`;
      return firstValueFrom(this.http.get<TypesPlans[]>(endpoint).pipe((response) => response));
    }

    getActivatedEmergencyPlans(idSuceso: number | string) {
      const endpoint = `/planes-emergencias/activados/${idSuceso}`;
      return firstValueFrom(this.http.get<TypesPlans[]>(endpoint).pipe((response) => response));
    }

    getTypesPlansByPlan(idTipoPlan: number | string, idCcaa?: number | string, idTipoSuceso?: number | string, IsFullDescription: boolean = false) {
      const endpoint = '/planes-emergencias';
      let params = new HttpParams()
        .set('IdTipoPlan', idTipoPlan.toString())
        .set('IsFullDescription', IsFullDescription)
        .set('IdAmbitoPlan', 2);
      
      if (idCcaa) {
        params = params.set('IdCcaa', idCcaa.toString());
      }

      if (idTipoSuceso) {
        params = params.set('idTipoSuceso', idTipoSuceso.toString());
      }
    
      return firstValueFrom(this.http.get<TypesPlans[]>(endpoint, { params }));
    }

    getPhases(plan_id: number | string) {
      const endpoint = `/fases-emergencia?idPlanEmergencia=${plan_id}`;
      return firstValueFrom(this.http.get<TypesPlans[]>(endpoint).pipe((response) => response));
    }

    getSituationsPlans(plan_id: number | string, phase_id: number | string) {
      const endpoint = `/plan-situacion-emergencia?idPlanEmergencia=${plan_id}&idFaseEmergencia=${phase_id}`;
      return firstValueFrom(this.http.get<SituationPlan[]>(endpoint).pipe((response) => response));
    }

}
