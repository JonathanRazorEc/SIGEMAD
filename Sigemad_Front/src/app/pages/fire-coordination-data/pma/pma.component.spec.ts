import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmaComponent } from './pma.component';

describe('PmaComponent', () => {
  let component: PmaComponent;
  let fixture: ComponentFixture<PmaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PmaComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(PmaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
