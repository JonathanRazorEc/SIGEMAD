import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FireTableComponent } from './fire-table.component';

describe('FireTableComponent', () => {
  let component: FireTableComponent;
  let fixture: ComponentFixture<FireTableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FireTableComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(FireTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
