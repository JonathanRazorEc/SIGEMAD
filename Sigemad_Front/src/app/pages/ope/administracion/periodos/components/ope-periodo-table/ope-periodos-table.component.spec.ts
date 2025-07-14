import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OpePeriodosTableComponent } from './ope-periodos-table.component';

describe('OpePeriodosTableComponent', () => {
  let component: OpePeriodosTableComponent;
  let fixture: ComponentFixture<OpePeriodosTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OpePeriodosTableComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(OpePeriodosTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
