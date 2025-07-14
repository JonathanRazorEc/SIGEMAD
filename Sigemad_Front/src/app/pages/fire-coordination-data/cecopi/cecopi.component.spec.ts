import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CecopiComponent } from './cecopi.component';

describe('CecopiComponent', () => {
  let component: CecopiComponent;
  let fixture: ComponentFixture<CecopiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CecopiComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CecopiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
