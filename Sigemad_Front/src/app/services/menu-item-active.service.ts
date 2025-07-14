import { Injectable, signal, Output, EventEmitter } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class MenuItemActiveService {
  @Output() set: EventEmitter<any> = new EventEmitter();
}
