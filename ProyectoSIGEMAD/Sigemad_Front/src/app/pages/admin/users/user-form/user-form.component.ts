import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators, ValidationErrors } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { UserService } from '@services/admin/user.service';
import { User } from '@type/admin/user.interface';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { UserListComponent } from '../user-list/user-list.component';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { NgxSpinnerModule } from 'ngx-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';

@Component({
  selector: 'app-user-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    RouterModule,
    DragDropModule,
    MatDialogModule,
    NgxSpinnerModule,
    MatSelectModule,
    MatOptionModule,
  ],
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss'],
})
export class UserFormComponent implements OnInit {
  userForm: FormGroup;
  isEdit = false;
  userId?: any;
  availableRoles: any;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private route: ActivatedRoute,
    private router: Router,
    private dialogRef: MatDialogRef<UserFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.userForm = this.fb.group(
      {
        nombre: ['', Validators.required],
        apellidos: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        username: ['', Validators.required],
        password: ['', [Validators.required, Validators.minLength(5)]],
        repeatPassword: ['', Validators.required],
        telefono: ['', Validators.required],
        roleIds: [[], Validators.required],
        activo: [true, Validators.required],
      },
      { validators: this.passwordMatchValidator }
    );
  }

  ngOnInit(): void {
    this.getRoles();
  }

  private getRoles() {
    this.userService.getRoles().subscribe((roles: any[]) => {
      this.availableRoles = roles;

      if (this.data?.user) {
        this.isEdit = true;
        const u = this.data.user;

        const matchedRoleIds = (u.roles ?? [])
          .map((rName: string) => this.availableRoles.find((r: any) => r.name === rName)?.id)
          .filter((id: any) => !!id);

        this.userForm.patchValue({
          nombre: u.nombre ?? '',
          apellidos: u.apellidos ?? '',
          email: u.email ?? '',
          username: u.userName ?? '',
          password: '',
          repeatPassword: '',
          telefono: u.phoneNumber ?? '',
          roleIds: matchedRoleIds,
          activo: u.activo ?? false
        });

        this.userForm.get('password')!.clearValidators();
        this.userForm.get('repeatPassword')!.clearValidators();
        this.userForm.get('password')!.updateValueAndValidity();
        this.userForm.get('repeatPassword')!.updateValueAndValidity();
      }
    });
  }

  onSubmit(): void {
    console.log('Payload enviado:', this.userForm.value);
    if (this.userForm.invalid) return;
    const f = this.userForm.value;
    const payload: any = {
      ...(this.isEdit && this.data.user?.id ? { id: this.data.user.id } : {}),
      userName: f.username,
      email: f.email,
      phoneNumber: f.telefono,
      password: f.password,
      passwordConfirmed: f.repeatPassword,
      nombre: f.nombre,
      apellidos: f.apellidos,
      roleIds: f.roleIds,
      activo: f.activo,
    };

    const request = this.isEdit
      ? this.userService.updateUser(payload.id, payload)
      : this.userService.createUser(payload);

    request.subscribe({
      next: () => {
        this.closeModal();
        this.router.navigate(['/users']);
      },
      error: (error) => {
        const validationErrors = error?.error?.errors;
        if (validationErrors) {
          Object.keys(validationErrors).forEach((key) => {
            const formControl = this.userForm.get(this.mapField(key));
            if (formControl) {
              formControl.setErrors({ server: validationErrors[key][0] });
            }
          });
        }
      }
    });
  }

  goBack(): void {
    this.isEdit = false;
    this.router.navigate(['/users']);
  }

  closeModal(): void {
    this.dialogRef.close();
  }

  passwordMatchValidator(group: FormGroup): ValidationErrors | null {
    const password = group.get('password')?.value;
    const repeatPassword = group.get('repeatPassword')?.value;

    if (password && repeatPassword && password !== repeatPassword) {
      group.get('repeatPassword')?.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true };
    } else {
      group.get('repeatPassword')?.setErrors(null);
      return null;
    }
  }

  private mapField(field: string): string {
    const map: Record<string, string> = {
      UserName: 'username',
      Email: 'email',
      PhoneNumber: 'telefono',
      Password: 'password',
      PasswordConfirmed: 'repeatPassword',
      Nombre: 'nombre',
      Apellidos: 'apellidos',
      RoleIds: 'roleIds'
    };
    return map[field] || field;
  }
}