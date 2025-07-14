import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MobilizationComponent } from './mobilization.component';

describe('MobilizationComponent', () => {
  let component: MobilizationComponent;
  let fixture: ComponentFixture<MobilizationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MobilizationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MobilizationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
