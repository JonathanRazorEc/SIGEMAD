import { Injectable, signal } from '@angular/core';
import { Province } from '../types/province.type';
import { Municipality } from '../types/municipality.type';
import { BaseItem } from '@type/base-item.type';

export interface InterventionTableItem {
  id?: number;
  caracterMedios: BaseItem | null;
  tipoIntervencionMedios: BaseItem | null;
  descripcion: string;
  medioNoCatalogado: string;
  numeroCapacidades: number;
  titularidadMedio: BaseItem | null;
  titular: string;
  provincia: Province | null;
  municipio: Municipality | null;
  fechaInicio: Date | null;
  fechaFin: Date | null;
  observaciones: string;
  geoPosicion?: any;
  detalleIntervencionMedios: any[];
  esModificado?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class InterventionDataService {
  public interventionTableData = signal<InterventionTableItem[]>([]);

  constructor() {}

  addInterventionItem(item: InterventionTableItem) {
    this.interventionTableData.update(items => [...items, item]);
  }

  updateInterventionItem(updatedItem: InterventionTableItem, index: number) {
    this.interventionTableData.update(items => {
      const update = [ ...items ];

      if (update[index]) {
        update[index] = updatedItem;
      }

      return update;
    });
  }

  removeInterventionItem(id: number) {
    this.interventionTableData.update(items => 
      items.filter(item => item.id !== id)
    );
  }

  setInterventionData(items: InterventionTableItem[]) {
    this.interventionTableData.set(items);
  }

  clearInterventionData() {
    this.interventionTableData.set([]);
  }
} 