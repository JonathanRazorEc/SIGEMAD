import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FireCoordinationData } from './fire-coordination-data.component';

describe('FireCoordinationDataComponent', () => {
  let component: FireCoordinationData;
  let fixture: ComponentFixture<FireCoordinationData>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FireCoordinationData],
    }).compileComponents();

    fixture = TestBed.createComponent(FireCoordinationData);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
