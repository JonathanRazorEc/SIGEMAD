import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FireAuditoriaComponent } from './fire-auditoria.component';

describe('FireAuditoriaComponent', () => {
  let component: FireAuditoriaComponent;
  let fixture: ComponentFixture<FireAuditoriaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FireAuditoriaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FireAuditoriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
