import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { FireService } from '../../services/fire.service';

@Component({
  selector: 'app-modal-confirm',
  standalone: true,
  templateUrl: `./modalConfirm.component.html`,
  styleUrl: './modalConfirm.component.scss',
  imports: [MatButtonModule, NgxSpinnerModule],
})
export class ModalConfirmComponent {
  public routenav = inject(Router);

  constructor(
    private dialogRef: MatDialogRef<ModalConfirmComponent>,
    private fireService: FireService,
    private spinner: NgxSpinnerService
  ) {}

  dataProps = inject(MAT_DIALOG_DATA) as {
    fireId: any;
  };

  closeModal() {
    this.dialogRef.close();
  }

  confirm() {
    this.spinner.show();
    console.log(this.dataProps.fireId);
    this.fireService.delete(this.dataProps.fireId).then((res) => {
      this.spinner.hide();
      this.dialogRef.close();
      this.routenav.navigate([`/fire`]);
    });
  }
}
