import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecialPlansActivationComponent } from './special-plans-activation.component';

describe('SpecialPlansActivationComponent', () => {
  let component: SpecialPlansActivationComponent;
  let fixture: ComponentFixture<SpecialPlansActivationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SpecialPlansActivationComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SpecialPlansActivationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
}); 