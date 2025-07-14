import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FireFilterFormComponent } from './fire-filter-form.component';

describe('FireFilterFormComponent', () => {
  let component: FireFilterFormComponent;
  let fixture: ComponentFixture<FireFilterFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FireFilterFormComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FireFilterFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
