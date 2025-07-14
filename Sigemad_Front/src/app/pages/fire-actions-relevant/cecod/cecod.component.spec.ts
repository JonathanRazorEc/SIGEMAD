import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CecodComponent } from './cecod.component';

describe('CecodComponent', () => {
  let component: CecodComponent;
  let fixture: ComponentFixture<CecodComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CecodComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CecodComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
