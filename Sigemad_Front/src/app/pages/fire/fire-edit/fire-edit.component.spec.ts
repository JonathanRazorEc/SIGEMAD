import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FireEditComponent } from './fire-edit.component';

describe('FireEditComponent', () => {
  let component: FireEditComponent;
  let fixture: ComponentFixture<FireEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FireEditComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FireEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
