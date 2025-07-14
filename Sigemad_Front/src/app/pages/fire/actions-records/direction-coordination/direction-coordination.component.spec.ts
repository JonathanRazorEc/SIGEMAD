import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DirectionCoordinationComponent } from './direction-coordination.component';

describe('DirectionCoordinationComponent', () => {
  let component: DirectionCoordinationComponent;
  let fixture: ComponentFixture<DirectionCoordinationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DirectionCoordinationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DirectionCoordinationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
