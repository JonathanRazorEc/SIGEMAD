import { Injectable } from '@angular/core';
import Swal, { SweetAlertOptions, SweetAlertResult } from 'sweetalert2';

@Injectable({
  providedIn: 'root',
})
export class AlertService {
  showAlert(options: SweetAlertOptions): Promise<SweetAlertResult<any>> {
    const updatedOptions: SweetAlertOptions = {
      ...options,
      allowOutsideClick: false,
      confirmButtonColor: '#186c93',
    };

    return Swal.fire(updatedOptions);
  }
}
