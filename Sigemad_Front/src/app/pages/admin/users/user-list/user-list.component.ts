import { Component, OnInit, Renderer2, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { RouterModule, Router } from '@angular/router';
import { User } from '@type/admin/user.interface';
import { UserService } from '@services/admin/user.service';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { UserFormComponent } from '../user-form/user-form.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AlertService } from '@shared/alert/alert.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { MatSnackBar } from '@angular/material/snack-bar';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    RouterModule,
    MatPaginatorModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
  ],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss'],
})
export class UserListComponent implements OnInit {
  displayedColumns: string[] = ['nombre', 'apellidos', 'email', 'username', 'telefono', 'roles', 'activo', 'actions']; // Added 'username' column
  dataSource = new MatTableDataSource<User>();
  availableRoles:any;
  formData: FormGroup;
  selectedRoles: string[] = [];
  selectedActivo: boolean | null = null;

  public alertService = inject(AlertService);
  private spinner = inject(NgxSpinnerService);
  public renderer = inject(Renderer2);
  public snackBar = inject(MatSnackBar);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private userService: UserService,
    private router: Router,
    private dialog: MatDialog,
    private fb: FormBuilder
  ) {
    this.formData = this.fb.group({
      roles: [[]],
      activo: [null],
      nombre: [''],
      apellidos: [''],
      email: [''],
      username: [''],
      telefono: [''],
    });
  }

  ngOnInit(): void {
    this.loadUsers();
    this.getRoles();
  }


    

  private getRoles() {
    this.userService.getRoles().subscribe((roles: any[]) => {
      // Si quieres usar el name como valor:
      this.availableRoles = roles.map(r => r.name);
      // O si prefieres usar el id:
      // this.availableRoles = roles.map(r => r.id);
    });
  }

 loadUsers(): void {
  forkJoin({
    users: this.userService.getUsers(),
    userRoles: this.userService.getUserRoles(),
    allRoles: this.userService.getRoles()
  }).subscribe(({ users, userRoles, allRoles }) => {
    const rolesById = Object.fromEntries(allRoles.map((r: any) => [r.id, r.name]));
    const rolesMap = userRoles.reduce((acc: Record<string,string[]>, { userId, roleId }) => {
      if (!acc[userId]) acc[userId] = [];
      acc[userId].push(rolesById[roleId]);
      return acc;
    }, {});
  
      this.dataSource.data = users.map((user:any) => ({
        ...user,
        username: user.userName,           // ← mapeo
        telefono: user.phoneNumber,
        roles: rolesMap[user.id] || [],
        activo: user.activo !== undefined ? user.activo : true,
      }));
  
      setTimeout(() => {
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      });
    });
  }
  //prueba

  async deleteUser(user: any) {
    this.alertService

      .showAlert({
        title: '¿Estás seguro de eliminar el registro?',
        showCancelButton: true,
        cancelButtonColor: '#d33',
        confirmButtonText: '¡Sí, eliminar!',
        cancelButtonText: 'Cancelar',
        customClass: {
          title: 'sweetAlert-fsize20',
        },
      })
      .then(async (result) => {
        if (result.isConfirmed) {
          this.spinner.show();
          this.userService.deleteUser(user.id).subscribe({
            next:(resp)=>{

              const toolbar = document.querySelector('mat-toolbar');
              this.renderer.setStyle(toolbar, 'z-index', '1');
              this.spinner.hide();
    
              this.loadUsers();
              this.snackBar.open('Datos eliminados correctamente!', '', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom',
                panelClass: ['snackbar-verde'],
              });
            },
            error:(resp)=>{
          this.spinner.hide();
          console.error(resp)
            },
          })

        } else {
          this.spinner.hide();
        }
      });
  }

  openUserForm(user?: any): void {
    console.log(user)
    const dialogRef = this.dialog.open(UserFormComponent, {
      width: '600px',
      disableClose: true,
      data: { user },
    });

    dialogRef.afterClosed().subscribe(() => {
      this.loadUsers();
    });
  }

  editUser(user: any): void {
    console.log(user)
    this.openUserForm(user);
  }

  addUser(): void {
    this.openUserForm();
  }

  applyFilter(event: Event | any, column: string): void {
    const filterValue = event.target
      ? String((event.target as HTMLInputElement).value)
          .trim()
          .toLowerCase()
      : String(event.value).trim().toLowerCase();

    if (!this.dataSource.filter) {
      this.dataSource.filter = '{}';
    }

    const currentFilter = JSON.parse(this.dataSource.filter);
    currentFilter[column] = filterValue;
    this.dataSource.filter = JSON.stringify(currentFilter);

    this.dataSource.filterPredicate = (data: User, filter: string): boolean => {
      const filters = JSON.parse(filter);

      return Object.keys(filters).every((key) => {
        const searchValue = filters[key];
        switch (key) {
          case 'nombre':
            return data.nombre?.toLowerCase().includes(searchValue) || false;
          case 'apellidos':
            return data.apellidos?.toLowerCase().includes(searchValue) || false;
          case 'email':
            return data.email?.toLowerCase().includes(searchValue) || false;
          case 'username':
            return data.username?.toLowerCase().includes(searchValue) || false;
          case 'telefono':
            return data.telefono?.toLowerCase().includes(searchValue) || false;
          case 'activo':
            return searchValue === 'true' ? data.activo === true : searchValue === 'false' ? data.activo === false : true;
          case 'roles':
            const selectedRoles = searchValue.split(',').map((role: string) => role.trim());
            return selectedRoles.every((selectedRole: string) => data.roles?.map((role: string) => role.toLowerCase()).includes(selectedRole));
          default:
            return true;
        }
      });
    };

    this.dataSource.filter = JSON.stringify(currentFilter);
  }

  clearFilters(): void {
    this.dataSource.filter = '';

    this.formData.reset({
      roles: [],
      activo: null,
      nombre: '',
      apellidos: '',
      email: '',
      username: '',
      telefono: '',
    });

    const inputFields = document.querySelectorAll('input[matInput]');
    inputFields.forEach((input) => {
      (input as HTMLInputElement).value = '';
      input.dispatchEvent(new Event('input'));
    });

    const selects = document.querySelectorAll('mat-select');
    selects.forEach((select) => {
      const selectElement = select.querySelector('mat-select-trigger');
      if (selectElement) {
        selectElement.textContent = '';
      }
    });

    this.dataSource.filterPredicate = (data: User, filter: string): boolean => true;

    this.loadUsers();

    this.selectedRoles = [];
    this.selectedActivo = null;
  }
}
