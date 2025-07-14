import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ActivationPlanComponent } from './activation-plan.component';

describe('ActivationPlanComponent', () => {
  let component: ActivationPlanComponent;
  let fixture: ComponentFixture<ActivationPlanComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ActivationPlanComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ActivationPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
