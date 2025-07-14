import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmergencyNationalComponent } from './emergency-national.component';

describe('EmergencyNationalComponent', () => {
  let component: EmergencyNationalComponent;
  let fixture: ComponentFixture<EmergencyNationalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmergencyNationalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EmergencyNationalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
