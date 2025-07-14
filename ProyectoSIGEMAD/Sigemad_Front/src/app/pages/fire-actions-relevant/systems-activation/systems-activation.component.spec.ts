import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemsActivationComponent } from './systems-activation.component';

describe('SystemsActivationComponent', () => {
  let component: SystemsActivationComponent;
  let fixture: ComponentFixture<SystemsActivationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemsActivationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemsActivationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
